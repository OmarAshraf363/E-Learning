using BFCAI.Api.Infrastructure;
using BFCAI.Application.Features.Communities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BFCAI.Api.Controllers.Community;

/// <summary>Legacy Customer/CommunityController. Any logged-in user (the legacy page required login).</summary>
[ApiController]
[Route("api/community")]
[Authorize]
public class CommunityController(ICommunityService service, IValidator<PostRequest> postValidator) : ControllerBase
{
    /// <summary>?id=communityId &amp;word= &amp;maxReact=true &amp;userName= &amp;userId=</summary>
    [HttpGet("feed")] public Task<FeedDto> Feed([FromQuery] FeedFilter filter) => service.GetFeedAsync(filter);

    public class PostForm
    {
        public string Content { get; set; } = "";
        public List<string>? KeyWords { get; set; }
        public int? CommunityId { get; set; }
        public IFormFile? Attachment { get; set; }
    }

    [HttpPost("posts")]
    public async Task<PostDto> CreatePost([FromForm] PostForm form) => await service.CreatePostAsync(await ToRequestAsync(form));

    [HttpPut("posts/{id:int}")]
    public async Task<PostDto> UpdatePost(int id, [FromForm] PostForm form) => await service.UpdatePostAsync(id, await ToRequestAsync(form));

    [HttpDelete("posts/{id:int}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        await service.DeletePostAsync(id);
        return NoContent();
    }

    [HttpGet("posts/{id:int}/comments")] public Task<List<CommentDto>> Comments(int id) => service.GetCommentsAsync(id);
    [HttpPost("posts/{id:int}/comments")] public Task<CommentDto> AddComment(int id, CommentRequest request) => service.AddCommentAsync(id, request);
    [HttpGet("comments/{id:int}/replies")] public Task<List<CommentDto>> Replies(int id) => service.GetRepliesAsync(id);
    [HttpPost("comments/{id:int}/replies")] public Task<CommentDto> AddReply(int id, CommentRequest request) => service.AddReplyAsync(id, request);

    [HttpPost("posts/{id:int}/reactions")] public Task<ReactionToggleDto> ReactToPost(int id, ReactRequest request) => service.TogglePostReactionAsync(id, request);
    [HttpGet("posts/{id:int}/reactions")] public Task<List<ReactionDto>> PostReactions(int id) => service.GetPostReactionsAsync(id);
    [HttpPost("comments/{id:int}/reactions")] public Task<ReactionToggleDto> ReactToComment(int id) => service.ToggleCommentReactionAsync(id);
    [HttpGet("comments/{id:int}/reactions")] public Task<List<ReactionDto>> CommentReactions(int id) => service.GetCommentReactionsAsync(id);

    private async Task<PostRequest> ToRequestAsync(PostForm form)
    {
        var request = new PostRequest(form.Content ?? "", form.KeyWords, form.CommunityId, form.Attachment.ToFileUpload());
        await postValidator.ValidateAndThrowAsync(request);
        return request;
    }
}
