using Banha_UniverCity.Repository.IRepository;
using BFCAI.Models;
using BFCAI.Models.ViewModels;
using DataAccess.Repository.IRepository;
using DataAccess.Repository.IRepository.Service;
using DataAccess.Repository.ModelsRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Stripe.Checkout;
using System.Threading.Tasks;

namespace Banha_UniverCity.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartService _cartService;
        private readonly IPaymentService _paymentService;

        public CartController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, ICartService cartService, IPaymentService paymentService)
        {

            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _cartService = cartService;
            _paymentService = paymentService;
        }
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
          var model = await _cartService.GetCartViewModelAsync(userId);
            return View(model);
        }
        public async Task<IActionResult> AddToCart(int?courseId,int?quantity)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
            await _cartService.AddToCartAsync(userId, courseId ?? 0, quantity ?? 1);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int id)
        {

            var cartItem = _unitOfWork.cartItemRepository.GetOne(e => e.Id == id);
            
            if (cartItem == null) { return NotFound(); }
            //var orderItem=_unitOfWork.orderItemRepository.GetOne(e=>e.CourseId == cartItem.CourseId);
            //if (orderItem == null) { return NotFound(); }
           await _unitOfWork.cartItemRepository.DeleteAsync(cartItem);
           //await _unitOfWork.orderItemRepository.DeleteAsync(orderItem);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Pay()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account", new { area = "Identity", returnUrl = Url.Action() });

            var url = await _paymentService.CreateOrderAndStartCheckoutAsync(
                userId,
                $"{Request.Scheme}://{Request.Host}/{StaticData.role_Customer}/checkout/success",
                $"{Request.Scheme}://{Request.Host}/{StaticData.role_Customer}/checkout/cancel"
            );

            if (string.IsNullOrEmpty(url))
            {
                TempData["Error"] = "Your cart is empty or failed to start payment session.";
                return RedirectToAction("Index");
            }

            return Redirect(url);
        }

    }
}
