using Banha_UniverCity.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models
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
        [ValidateNever]

        public Post Post { get; set; } = null!;
        [ForeignKey(nameof(UserId))]
        [ValidateNever]
        public ApplicationUser User { get; set; } = null!;
        public Comment? ParentComment { get; set; }
        [ValidateNever]
        public ICollection<Comment> Replies { get; set; } = new List<Comment>();
        [ValidateNever]
        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
    }
}
