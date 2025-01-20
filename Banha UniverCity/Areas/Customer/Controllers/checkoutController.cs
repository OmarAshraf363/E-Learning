using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Banha_UniverCity.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class checkoutController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IUnitOfWork unitOfWork;

        public checkoutController(UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
        }
        public IActionResult success()
        {

            var userId = userManager.GetUserId(User);
            var userOrder = unitOfWork.orderRepository.Get(e => e.AppUserId == userId && e.OrderStatus == 0)?.FirstOrDefault();
            if (userOrder != null)
            {


                var orderItem = unitOfWork.orderItemRepository.GetOne(e => e.OrderId == userOrder.Id);
                if (orderItem != null)
                {

                    orderItem.TotalPrice = orderItem.ListPrice * orderItem.Quantity;
                }

                userOrder.OrderStatus = 1;
                userOrder.RequiredDate = DateTime.Now;
                userOrder.ShippedDate = DateTime.Now;
                userOrder.PaymentStatus = StaticData.StaticDataInProcessPayment;
                var courseIDES = unitOfWork.orderItemRepository.Get(e => e.OrderId == userOrder.Id).Select(e => e.CourseId);
                foreach (var id in courseIDES)
                {

                    var enroll = new Enrollment()
                    {
                        StudentId = userId,
                        CourseID = id,

                    };
                    unitOfWork.enrollmentRepository.Create(enroll);
                }
                unitOfWork.Commit();
                var cart=unitOfWork.cartRepository.GetOne(e=>e.UserId == userId);
                var cartItems = unitOfWork.cartItemRepository.Get(e => e.CartId == cart.Id);
                unitOfWork.cartItemRepository.DeleteRange(cartItems);
                unitOfWork.cartRepository.Delete(cart);
                unitOfWork.Commit();
            }
            else
            {

                return View("NotFound");

            }
            return View();
        }
        public IActionResult cancel()
        {



            return View();
        }
    }
}
