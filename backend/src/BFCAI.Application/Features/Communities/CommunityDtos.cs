using BFCAI.Application.Common.Models;

namespace BFCAI.Application.Features.Communities;

public record CommunityDto(int Id, string Name, string Description);
public record AuthorDto(string Id, string FullName, string? Picture);
public record UserCardDto(string Id, string FullName, string? Picture, string UserType, string JopDescription);
public record SelectedUserDto(string Id, string FullName, string? Picture, string UserType, string JopDescription, int PostsCount);

public record PostDto(
    int Id,
    string Content,
    string? Attachment,
    DateTime CreatedAt,
    int? CommunityId,
    AuthorDto Author,
    List<string> KeyWords,
    int ReactionsCount,
    int CommentsCount,
    bool ReactedByMe,
    bool IsMine);

/// <summary>Legacy Community/Index filters.</summary>
public class FeedFilter
{
    /// <summary>Community id ("id" in the legacy URL).</summary>
    public int? Id { get; set; }
    public string? Word { get; set; }
    public bool? MaxReact { get; set; }
    public string? UserName { get; set; }
    public string? UserId { get; set; }
}

/// <summary>Legacy CommunityVM.</summary>
public record FeedDto(
    List<CommunityDto> Communities,
    CommunityDto? Community,
    List<PostDto> Posts,
    List<UserCardDto> TopInstructors,
    List<UserCardDto> SearchResults,
    SelectedUserDto? SelectedUser,
    AuthorDto CurrentUser);

public record CommentDto(int Id, int PostId, string Content, DateTime CreatedAt, AuthorDto Author, int ReactionsCount, int RepliesCount, bool ReactedByMe);

public record ReactionDto(string? UserFullName, string? UserPicture, string Type);

public record ReactionToggleDto(string State, string? TypeReact, int Count);

/// <summary>Create / edit a post. The attachment is optional; on edit, no new file keeps the current one.</summary>
public record PostRequest(string Content, List<string>? KeyWords, int? CommunityId, FileUpload? Attachment);

public record CommentRequest(string Content);

public record ReactRequest(string? TypeReact);
