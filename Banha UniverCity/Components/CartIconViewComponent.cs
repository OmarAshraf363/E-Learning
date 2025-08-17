using Banha_UniverCity.Repository.IRepository;
using DataAccess.Repository.IRepository.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Banha_UniverCity.Components
{
    public class CartIconViewComponent : ViewComponent
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public CartIconViewComponent(UserManager<IdentityUser> userManager,  IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var cart = await _unitOfWork.cartRepository.GetOneAsync(e => e.UserId == userId,includes:e=>e.Items);
            var count = cart?.Items?.Count ?? 0;
            return View(count);
        }

    }
}
