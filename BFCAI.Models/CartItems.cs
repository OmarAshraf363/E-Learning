using Banha_UniverCity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models
{
    public class CartItems
    {
        public int Id { get; set; }
        public int? CartId { get; set; }
        public int? CourseId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }

        public Cart? Cart { get; set; }
        public Course? Course { get; set; }
    }
}
