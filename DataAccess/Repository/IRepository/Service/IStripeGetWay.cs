using BFCAI.Models;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository.Service
{
    public interface IStripeGetWay
    {
        Session CreateCheckoutSession(
       IEnumerable<CartItems> cartItems,
       string successUrl,
       string cancelUrl);
    }
}
