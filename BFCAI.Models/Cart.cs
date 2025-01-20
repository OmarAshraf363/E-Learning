using Banha_UniverCity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string? UserId { get; set; } = string.Empty;
        public ApplicationUser? ApplicationUser {  get; set; }
        
        public ICollection<CartItems> Items { get; set; }=new List<CartItems>();
    }
}
