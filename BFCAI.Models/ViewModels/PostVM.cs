using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models.ViewModels
{
    public class PostVM
    {
        public string Content { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string UserProfilePic { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? Attachment { get; set; }
        public int ? CommunityId { get; set; }
    }
}
