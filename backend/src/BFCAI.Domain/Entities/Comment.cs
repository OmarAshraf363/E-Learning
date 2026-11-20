using System.ComponentModel.DataAnnotations.Schema;

namespace BFCAI.Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; } // FK to Post
        public string Content { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public int? ParentCommentId { get; set; } // FK to another Comment (threaded comments)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; } = null!;
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;
        public Comment? ParentComment { get; set; }
        public ICollection<Comment> Replies { get; set; } = new List<Comment>();
        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
    }
}
