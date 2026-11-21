using BFCAI.Application.Common.Exceptions;
using BFCAI.Application.Common.Interfaces;
using BFCAI.Domain.Constants;
using BFCAI.Domain.Entities;
using BFCAI.Domain.Enums;
using CommunityEntity = BFCAI.Domain.Entities.Community;

namespace BFCAI.Application.Features.Communities;

/// <summary>Legacy Customer/CommunityController (feed, posts, comments, replies, reactions).</summary>
public interface ICommunityService
{
    Task<FeedDto> GetFeedAsync(FeedFilter filter);
    Task<PostDto> CreatePostAsync(PostRequest request);
    Task<PostDto> UpdatePostAsync(int id, PostRequest request);
    Task DeletePostAsync(int id);
    Task<List<CommentDto>> GetCommentsAsync(int postId);
    Task<List<CommentDto>> GetRepliesAsync(int commentId);
    Task<CommentDto> AddCommentAsync(int postId, CommentRequest request);
    Task<CommentDto> AddReplyAsync(int commentId, CommentRequest request);
    Task<ReactionToggleDto> TogglePostReactionAsync(int postId, ReactRequest request);
    Task<ReactionToggleDto> ToggleCommentReactionAsync(int commentId);
    Task<List<ReactionDto>> GetPostReactionsAsync(int postId);
    Task<List<ReactionDto>> GetCommentReactionsAsync(int commentId);
}

public class CommunityService(IUnitOfWork unitOfWork, ICurrentUser currentUser, IFileStorage files) : ICommunityService
{
    private const string AttachmentsFolder = "uploads/attachments";
    private IGenericRepository<Post> Posts => unitOfWork.Repository<Post>();
    private IGenericRepository<Comment> Comments => unitOfWork.Repository<Comment>();
    private IGenericRepository<Reaction> Reactions => unitOfWork.Repository<Reaction>();
    private string Me => currentUser.RequiredUserId;

    public async Task<FeedDto> GetFeedAsync(FeedFilter filter)
    {
        var me = Me;
        var users = unitOfWork.Repository<ApplicationUser>();
        var communities = unitOfWork.Repository<CommunityEntity>();
        var word = filter.Word?.Trim().ToLower();

        var posts = await Posts.SelectAsync(
            p => new PostDto(p.Id, p.Content, p.Attachment, p.CreatedAt, p.CommunityId,
                new AuthorDto(p.User.Id, p.User.FullName, p.User.Picture),
                p.KeyWords.Select(k => k.Name).ToList(),
                p.Reactions.Count, p.Comments.Count,
                p.Reactions.Any(r => r.UserId == me), p.UserId == me),
            p => (filter.Id == null || p.CommunityId == filter.Id)
                 && (string.IsNullOrEmpty(filter.UserId) || p.UserId == filter.UserId)
                 && (string.IsNullOrEmpty(word) || p.Content.ToLower().Contains(word) || p.KeyWords.Any(k => k.Name.ToLower().Contains(word))),
            q => filter.MaxReact == true ? q.OrderByDescending(p => p.Reactions.Count) : q.OrderBy(p => p.Id));

        // Legacy "Top instructors": instructors who teach at least one course.
        var topInstructors = await users.SelectAsync(ToCard,
            u => u.UserType == Roles.Instructor && u.CoursesTaught.Any());

        var nameSearch = filter.UserName?.Trim().ToLower();
        var searchResults = string.IsNullOrEmpty(nameSearch)
            ? []
            : await users.SelectAsync(ToCard, u => u.FullName.ToLower().Contains(nameSearch));

        var selected = string.IsNullOrEmpty(filter.UserId)
            ? null
            : await users.SelectOneAsync(u => u.Id == filter.UserId,
                u => new SelectedUserDto(u.Id, u.FullName, u.Picture, u.UserType, u.JopDescription, u.Posts.Count));

        var current = await users.SelectOneAsync(u => u.Id == me, u => new AuthorDto(u.Id, u.FullName, u.Picture))
                      ?? new AuthorDto(me, "", null);

        return new FeedDto(
            await communities.SelectAsync(c => new CommunityDto(c.Id, c.Name, c.Description)),
            filter.Id is null ? null : await communities.SelectOneAsync(c => c.Id == filter.Id, c => new CommunityDto(c.Id, c.Name, c.Description)),
            posts, topInstructors, searchResults, selected, current);
    }

    public async Task<PostDto> CreatePostAsync(PostRequest request)
    {
        var post = new Post
        {
            Content = request.Content,
            UserId = Me, // technical fix: legacy bound the author from a hidden form field
            CommunityId = request.CommunityId,
            CreatedAt = DateTime.Now,
            KeyWords = ToKeyWords(request.KeyWords),
        };
        if (request.Attachment is not null) post.Attachment = await SaveAttachmentAsync(request);

        await Posts.AddAsync(post);
        await unitOfWork.SaveChangesAsync();
        return await GetPostAsync(post.Id);
    }

    /// <summary>
    /// Technical fixes: only the author can edit; the old keywords were deleted but the new ones never
    /// saved; editing without a new image deleted the current image file.
    /// </summary>
    public async Task<PostDto> UpdatePostAsync(int id, PostRequest request)
    {
        var post = await GetOwnPostAsync(id, p => p.KeyWords);
        post.Content = request.Content;
        post.UpdatedAt = DateTime.Now;

        unitOfWork.Repository<KeyWord>().DeleteRange(post.KeyWords);
        post.KeyWords = ToKeyWords(request.KeyWords);

        if (request.Attachment is not null)
        {
            var old = post.Attachment;
            post.Attachment = await SaveAttachmentAsync(request);
            await files.DeleteAsync(old);
        }

        await unitOfWork.SaveChangesAsync();
        return await GetPostAsync(id);
    }

    /// <summary>
    /// Technical fixes: only the author can delete (legacy: anyone, via GET); comments and keywords are
    /// removed too (legacy failed with a FK error when the post had any); the image file is deleted.
    /// </summary>
    public async Task DeletePostAsync(int id)
    {
        var post = await GetOwnPostAsync(id, p => p.KeyWords, p => p.Reactions);
        var comments = await Comments.GetAllAsync(c => c.PostId == id, tracked: true);
        var commentIds = comments.Select(c => c.Id).ToList();
        var commentReactions = await Reactions.GetAllAsync(r => r.CommentId != null && commentIds.Contains(r.CommentId.Value), tracked: true);

        Reactions.DeleteRange(commentReactions);
        Reactions.DeleteRange(post.Reactions);
        Comments.DeleteRange(comments.Where(c => c.ParentCommentId != null)); // replies first
        Comments.DeleteRange(comments.Where(c => c.ParentCommentId == null));
        unitOfWork.Repository<KeyWord>().DeleteRange(post.KeyWords);
        Posts.Delete(post);

        await unitOfWork.SaveChangesAsync();
        await files.DeleteAsync(post.Attachment);
    }

    public Task<List<CommentDto>> GetCommentsAsync(int postId)
    {
        var me = Me;
        return Comments.SelectAsync(ToCommentDto(me), c => c.PostId == postId && c.ParentCommentId == null, q => q.OrderBy(c => c.Id));
    }

    public Task<List<CommentDto>> GetRepliesAsync(int commentId)
    {
        var me = Me;
        return Comments.SelectAsync(ToCommentDto(me), c => c.ParentCommentId == commentId, q => q.OrderBy(c => c.Id));
    }

    public async Task<CommentDto> AddCommentAsync(int postId, CommentRequest request)
    {
        if (!await Posts.AnyAsync(p => p.Id == postId)) throw new NotFoundException(nameof(Post), postId);
        return await SaveCommentAsync(new Comment { PostId = postId, Content = request.Content });
    }

    public async Task<CommentDto> AddReplyAsync(int commentId, CommentRequest request)
    {
        var postId = await Comments.SelectOneAsync(c => c.Id == commentId, c => (int?)c.PostId)
                     ?? throw new NotFoundException(nameof(Comment), commentId);
        return await SaveCommentAsync(new Comment { PostId = postId, ParentCommentId = commentId, Content = request.Content });
    }

    /// <summary>Legacy AddReaction: if the user already reacted (any type) the reaction is removed, otherwise added.</summary>
    public async Task<ReactionToggleDto> TogglePostReactionAsync(int postId, ReactRequest request)
    {
        if (!await Posts.AnyAsync(p => p.Id == postId)) throw new NotFoundException(nameof(Post), postId);
        var type = string.IsNullOrWhiteSpace(request.TypeReact) ? ReactionType.Love.ToString() : request.TypeReact;
        return await ToggleAsync(r => r.PostId == postId, () => new Reaction { PostId = postId, Type = type }, type,
            () => Reactions.CountAsync(r => r.PostId == postId));
    }

    /// <summary>Legacy addReactInComment: comments only have "Love".</summary>
    public async Task<ReactionToggleDto> ToggleCommentReactionAsync(int commentId)
    {
        if (!await Comments.AnyAsync(c => c.Id == commentId)) throw new NotFoundException(nameof(Comment), commentId);
        var type = ReactionType.Love.ToString();
        return await ToggleAsync(r => r.CommentId == commentId, () => new Reaction { CommentId = commentId, Type = type }, type,
            () => Reactions.CountAsync(r => r.CommentId == commentId));
    }

    public Task<List<ReactionDto>> GetPostReactionsAsync(int postId)
        => Reactions.SelectAsync(r => new ReactionDto(r.User.FullName, r.User.Picture, r.Type), r => r.PostId == postId);

    public Task<List<ReactionDto>> GetCommentReactionsAsync(int commentId)
        => Reactions.SelectAsync(r => new ReactionDto(r.User.FullName, r.User.Picture, r.Type), r => r.CommentId == commentId);

    // ---------------------------------------------------------------- helpers

    private static readonly System.Linq.Expressions.Expression<Func<ApplicationUser, UserCardDto>> ToCard =
        u => new UserCardDto(u.Id, u.FullName, u.Picture, u.UserType, u.JopDescription);

    private static System.Linq.Expressions.Expression<Func<Comment, CommentDto>> ToCommentDto(string me)
        => c => new CommentDto(c.Id, c.PostId, c.Content, c.CreatedAt, new AuthorDto(c.User.Id, c.User.FullName, c.User.Picture),
            c.Reactions.Count, c.Replies.Count, c.Reactions.Any(r => r.UserId == me));

    private async Task<ReactionToggleDto> ToggleAsync(System.Linq.Expressions.Expression<Func<Reaction, bool>> target,
        Func<Reaction> create, string type, Func<Task<int>> count)
    {
        var me = Me;
        var existing = (await Reactions.GetAllAsync(target, tracked: true)).FirstOrDefault(r => r.UserId == me);
        string state;
        if (existing is null)
        {
            var reaction = create();
            reaction.UserId = me;
            await Reactions.AddAsync(reaction);
            state = "Add";
        }
        else
        {
            Reactions.Delete(existing);
            state = "Remove";
        }
        await unitOfWork.SaveChangesAsync();
        return new ReactionToggleDto(state, state == "Add" ? type : null, await count());
    }

    private async Task<CommentDto> SaveCommentAsync(Comment comment)
    {
        comment.UserId = Me; // technical fix: legacy bound the author from a hidden form field
        comment.CreatedAt = DateTime.Now;
        await Comments.AddAsync(comment);
        await unitOfWork.SaveChangesAsync();
        return (await Comments.SelectOneAsync(c => c.Id == comment.Id, ToCommentDto(Me)))!;
    }

    private async Task<Post> GetOwnPostAsync(int id, params System.Linq.Expressions.Expression<Func<Post, object?>>[] includes)
    {
        var post = await Posts.GetOneAsync(p => p.Id == id, tracked: true, includes) ?? throw new NotFoundException(nameof(Post), id);
        if (post.UserId != Me) throw new ForbiddenException();
        return post;
    }

    private async Task<PostDto> GetPostAsync(int id)
    {
        var me = Me;
        return (await Posts.SelectOneAsync(p => p.Id == id, p => new PostDto(p.Id, p.Content, p.Attachment, p.CreatedAt, p.CommunityId,
            new AuthorDto(p.User.Id, p.User.FullName, p.User.Picture), p.KeyWords.Select(k => k.Name).ToList(),
            p.Reactions.Count, p.Comments.Count, p.Reactions.Any(r => r.UserId == me), p.UserId == me)))!;
    }

    private static List<KeyWord> ToKeyWords(List<string>? words)
        => (words ?? []).Where(w => !string.IsNullOrWhiteSpace(w)).Select(w => new KeyWord { Name = w.Trim() }).ToList();

    /// <summary>Legacy: wwwroot/uploads/attachments/{guid}{ext}, stored as "uploads/attachments/..." (no leading slash).</summary>
    private async Task<string> SaveAttachmentAsync(PostRequest request)
    {
        var name = await files.SaveAsync(request.Attachment!, AttachmentsFolder, $"{Guid.NewGuid()}{request.Attachment!.Extension}");
        return $"{AttachmentsFolder}/{name}";
    }
}
