using Banha_UniverCity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models.ViewModels
{
    public class CommunityVM
    {
        public ICollection<Community> Communities { get; set; }= new List<Community>();
        public Community Community { get; set; } = null!;
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Reaction> Reactions { get; set; }=new List<Reaction>();
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }=new List<ApplicationUser>();
        public ApplicationUser SelectedUser { get; set; } = null!;

        public ICollection<ApplicationUser> SearchResult { get; set; }=new List<ApplicationUser>();
        public ApplicationUser ApplicationUser { get; set; }=null!;

    }
}
