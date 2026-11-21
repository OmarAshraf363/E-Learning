using System.Net;
using System.Net.Http.Json;
using BFCAI.Application.Features.Communities;

namespace BFCAI.Api.IntegrationTests;

public class CommunityTests(ApiFactory factory) : IClassFixture<ApiFactory>
{
    private async Task<HttpClient> NewUserAsync(string role = "Student")
    {
        var admin = await factory.AsAdminAsync();
        var email = TestHelpers.UniqueEmail(role);
        await admin.PostAsJsonAsync("/api/admin/users", new { userName = email, email, role, fullName = "U " + email[..8], password = "Pass@123" });
        return await factory.AuthenticatedAsync(email, "Pass@123");
    }

    private static MultipartFormDataContent PostForm(string content, bool withImage = false, params string[] keywords)
    {
        var form = new MultipartFormDataContent { { new StringContent(content), "Content" }, { new StringContent("1"), "CommunityId" } };
        foreach (var k in keywords) form.Add(new StringContent(k), "KeyWords");
        if (withImage) form.Add(new ByteArrayContent([1, 2, 3]), "Attachment", "pic.png");
        return form;
    }

    private static async Task<PostDto> CreatePostAsync(HttpClient client, string content, bool withImage = false, params string[] keywords)
        => (await (await client.PostAsync("/api/community/posts", PostForm(content, withImage, keywords))).Content.ReadFromJsonAsync<PostDto>())!;

    [Fact]
    public async Task Feed_requires_login()
        => Assert.Equal(HttpStatusCode.Unauthorized, (await factory.CreateClient().GetAsync("/api/community/feed")).StatusCode);

    [Fact]
    public async Task Create_post_with_keywords_and_image_then_filter_by_word_and_community()
    {
        var user = await NewUserAsync();
        var word = "kw" + Guid.NewGuid().ToString("N")[..6];
        var post = await CreatePostAsync(user, "Hello community", true, word, "angular");
        Assert.StartsWith("uploads/attachments/", post.Attachment);
        Assert.True(post.IsMine);

        var byWord = await user.GetFromJsonAsync<FeedDto>($"/api/community/feed?word={word}");
        Assert.Equal(post.Id, Assert.Single(byWord!.Posts).Id);

        var byCommunity = await user.GetFromJsonAsync<FeedDto>("/api/community/feed?id=1");
        Assert.Equal("Web Development Community", byCommunity!.Community!.Name);
        Assert.Contains(byCommunity.Posts, p => p.Id == post.Id);
    }

    [Fact]
    public async Task Editing_replaces_keywords_and_keeps_the_image_when_no_new_file()
    {
        var user = await NewUserAsync();
        var post = await CreatePostAsync(user, "v1", true, "old");

        var updated = await (await user.PutAsync($"/api/community/posts/{post.Id}", PostForm("v2", false, "new1", "new2")))
            .Content.ReadFromJsonAsync<PostDto>();
        Assert.Equal("v2", updated!.Content);
        Assert.Equal(["new1", "new2"], updated.KeyWords);
        Assert.Equal(post.Attachment, updated.Attachment);
    }

    [Fact]
    public async Task Only_the_author_can_edit_or_delete()
    {
        var author = await NewUserAsync();
        var other = await NewUserAsync();
        var post = await CreatePostAsync(author, "mine");

        Assert.Equal(HttpStatusCode.Forbidden, (await other.PutAsync($"/api/community/posts/{post.Id}", PostForm("hacked"))).StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, (await other.DeleteAsync($"/api/community/posts/{post.Id}")).StatusCode);
    }

    [Fact]
    public async Task Post_with_comments_replies_reactions_and_keywords_can_be_deleted()
    {
        var author = await NewUserAsync();
        var other = await NewUserAsync();
        var post = await CreatePostAsync(author, "to delete", false, "k");

        var comment = await (await other.PostAsJsonAsync($"/api/community/posts/{post.Id}/comments", new CommentRequest("Nice")))
            .Content.ReadFromJsonAsync<CommentDto>();
        await author.PostAsJsonAsync($"/api/community/comments/{comment!.Id}/replies", new CommentRequest("Thanks"));
        await other.PostAsJsonAsync($"/api/community/posts/{post.Id}/reactions", new ReactRequest("Like"));
        await author.PostAsync($"/api/community/comments/{comment.Id}/reactions", null);

        // legacy failed here with a FK error because of the comments / keywords
        Assert.Equal(HttpStatusCode.NoContent, (await author.DeleteAsync($"/api/community/posts/{post.Id}")).StatusCode);
    }

    [Fact]
    public async Task Comments_replies_and_reaction_toggle()
    {
        var user = await NewUserAsync();
        var post = await CreatePostAsync(user, "discuss");

        var comment = await (await user.PostAsJsonAsync($"/api/community/posts/{post.Id}/comments", new CommentRequest("First")))
            .Content.ReadFromJsonAsync<CommentDto>();
        await user.PostAsJsonAsync($"/api/community/comments/{comment!.Id}/replies", new CommentRequest("Reply"));
        Assert.Equal(HttpStatusCode.BadRequest,
            (await user.PostAsJsonAsync($"/api/community/comments/{comment.Id}/replies", new CommentRequest(""))).StatusCode);

        var comments = await user.GetFromJsonAsync<List<CommentDto>>($"/api/community/posts/{post.Id}/comments");
        Assert.Equal(1, Assert.Single(comments!).RepliesCount); // replies are not listed as top-level comments
        Assert.Single((await user.GetFromJsonAsync<List<CommentDto>>($"/api/community/comments/{comment.Id}/replies"))!);

        var added = await (await user.PostAsJsonAsync($"/api/community/posts/{post.Id}/reactions", new ReactRequest("Like")))
            .Content.ReadFromJsonAsync<ReactionToggleDto>();
        Assert.Equal(("Add", 1), (added!.State, added.Count));
        var who = await user.GetFromJsonAsync<List<ReactionDto>>($"/api/community/posts/{post.Id}/reactions");
        Assert.Equal("Like", Assert.Single(who!).Type);

        var removed = await (await user.PostAsJsonAsync($"/api/community/posts/{post.Id}/reactions", new ReactRequest("Love")))
            .Content.ReadFromJsonAsync<ReactionToggleDto>();
        Assert.Equal(("Remove", 0), (removed!.State, removed.Count));
    }

    [Fact]
    public async Task Top_instructors_and_user_search_and_user_filter()
    {
        var user = await NewUserAsync();
        var feed = await user.GetFromJsonAsync<FeedDto>("/api/community/feed?userName=instructor one&userId=2");
        Assert.Contains(feed!.TopInstructors, u => u.Id == "2"); // seed instructor teaches courses
        Assert.Contains(feed.SearchResults, u => u.Id == "2");
        Assert.Equal("2", feed.SelectedUser!.Id);
        Assert.All(feed.Posts, p => Assert.Equal("2", p.Author.Id));
    }
}
