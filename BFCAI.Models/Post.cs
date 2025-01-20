using Banha_UniverCity.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models
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
        [Required(ErrorMessage = "At least one key word must be added.")]
        public List<KeyWord> KeyWords { get; set; } = new List<KeyWord>();

        // Navigation Properties
        [ForeignKey(nameof(UserId))]
        [ValidateNever]
        public ApplicationUser User { get; set; } = null!;
        [ForeignKey(nameof(CommunityId))]
        [ValidateNever]

        public Community Community { get; set; }=null!;
        [ValidateNever]

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        [ValidateNever]

        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
    

    }
}
