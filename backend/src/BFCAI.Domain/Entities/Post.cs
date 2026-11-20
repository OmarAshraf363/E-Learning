using System.ComponentModel.DataAnnotations.Schema;

namespace BFCAI.Domain.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty; // FK to User
        public int? CommunityId {  get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public string? Attachment { get; set; } // Optional URL for files or images
        public List<KeyWord> KeyWords { get; set; } = new List<KeyWord>();

        // Navigation Properties
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;
        [ForeignKey(nameof(CommunityId))]
        public Community Community { get; set; }=null!;
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();

    }
}
