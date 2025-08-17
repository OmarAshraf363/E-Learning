using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;
using DataAccess.Repository.IRepository.Service;
using DataAccess.Repository.ModelsRepository.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Banha_UniverCity.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class checkoutController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICartService _cartService;
        private readonly IPaymentService _paymentService;

        public checkoutController(UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork, ICartService cartService, IPaymentService paymentService)
        {
            this._userManager = userManager;
            this.unitOfWork = unitOfWork;
            _cartService = cartService;
            _paymentService = paymentService;
        }
        public async Task<IActionResult> Success()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var result = await _paymentService.FinalizePaymentAndEnrollAsync(userId);
            if (!result.Item1)
                return View("NotFound");
           
            ViewBag.TotalPrice = result.Item2 ?? 0;

            return View();
        }
        public IActionResult cancel()
        {



            return View();
        }
    }
}
