using Banha_UniverCity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public byte OrderStatus { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime RequiredDate { get; set; }

        public DateTime? ShippedDate { get; set; }
        public string? StripeChargeId { get; set; }


        public string? PaymentStatus { get; set; }

        public string? AppUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public ICollection<OrderItems>OrderItems { get; set; }=new List<OrderItems>();
    }
}
