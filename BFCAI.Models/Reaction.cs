using Banha_UniverCity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models
{
    public class Reaction
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty; // Enum-like values: Like, Love, etc.
        public string UserId { get; set; } = string.Empty; // FK to User
        public int? PostId { get; set; } // Nullable FK to Post
        public int? CommentId { get; set; } // Nullable FK to Comment

        // Navigation Properties
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;
        [ForeignKey(nameof(PostId))]
        public Post? Post { get; set; } // Nullable navigation to Post

        [ForeignKey(nameof(CommentId))]
        public Comment? Comment { get; set; } // Nullable navigation to Comment
    }
}
