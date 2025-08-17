using BFCAI.Models;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository.Service
{
    public interface IPaymentService
    {
        Task<string?> CreateOrderAndStartCheckoutAsync(string userId, string successUrl, string cancelUrl);
        Task<(bool, decimal?)> FinalizePaymentAndEnrollAsync(string userId);
    }
}
