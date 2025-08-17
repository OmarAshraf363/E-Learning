using Banha_UniverCity;
using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;
using BFCAI.Models;
using DataAccess.Repository.IRepository.Service;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.ModelsRepository.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartService _cartService;
        private readonly IStripeGetWay _stripeGetWay;

        public PaymentService(IUnitOfWork unitOfWork, ICartService cartService, IStripeGetWay stripeGetWay)
        {
            _unitOfWork = unitOfWork;
            _cartService = cartService;
            _stripeGetWay = stripeGetWay;
        }

        public async Task<string?> CreateOrderAndStartCheckoutAsync(string userId, string successUrl, string cancelUrl)
        {
            var cart = await _unitOfWork.cartRepository.GetOneAsync(e=>e.UserId==userId);
            if (cart == null ) return null;
            //get cart items
            var cartItems = await _unitOfWork.cartItemRepository.GetAllAsync(e => e.CartId == cart.Id,includes:e=>e.Course);

            var order = await _unitOfWork.orderRepository
                .GetOneAsync(e => e.AppUserId == userId && e.OrderStatus == 0);

            if (order == null)
            {
                order = new Order
                {
                    AppUserId = userId,
                    OrderStatus = 0,
                    OrderDate = DateTime.Now
                };
                await _unitOfWork.orderRepository.AddAsync(order);
                _unitOfWork.Commit();
            }

            // Remove old items
            var oldItems = _unitOfWork.orderItemRepository.Get(e => e.OrderId == order.Id).ToList();
            _unitOfWork.orderItemRepository.DeleteRange(oldItems);

            foreach (var item in cartItems)
            {
                var orderItem = new OrderItems
                {
                    OrderId = order.Id,
                    CourseId = item.CourseId,
                    Quantity = item.Quantity,
                    ListPrice = item.Course?.Price,
                    TotalPrice = item.Quantity * item.Course?.Price
                };
                await _unitOfWork.orderItemRepository.AddAsync(orderItem);
            }

            _unitOfWork.Commit();

            // Start Stripe checkout
            var session = _stripeGetWay.CreateCheckoutSession(cartItems, successUrl, cancelUrl);
            order.StripeChargeId = session.Id;
            _unitOfWork.Commit();

            return session.Url;
        }

        public async Task<(bool,decimal?) > FinalizePaymentAndEnrollAsync(string userId)
        {
            var order = await _unitOfWork.orderRepository
                .GetOneAsync(e => e.AppUserId == userId && e.OrderStatus == 0, asNoTracking: false);

            if (order == null) return (false,0);

            var orderItems = _unitOfWork.orderItemRepository.Get(e => e.OrderId == order.Id).ToList();

            order.OrderStatus = 1;
            order.RequiredDate = DateTime.Now;
            order.ShippedDate = DateTime.Now;
            order.PaymentStatus = StaticData.StaticDataInProcessPayment;

            foreach (var item in orderItems)
            {
                item.TotalPrice = item.ListPrice * item.Quantity;
                var enrollment = new Enrollment
                {
                    StudentId = userId,
                    CourseID = item.CourseId
                };
                await _unitOfWork.enrollmentRepository.AddAsync(enrollment);
            }

            var cart = await _cartService.GetUserCart(userId);
            if (cart != null)
            {
                var cartItems = _unitOfWork.cartItemRepository.Get(e => e.CartId == cart.Id);
                _unitOfWork.cartItemRepository.DeleteRange(cartItems);
                await _unitOfWork.cartRepository.DeleteAsync(cart);
            }

            _unitOfWork.Commit();
            return (true,orderItems.Sum(e=>e.TotalPrice));
        }
    }
}
