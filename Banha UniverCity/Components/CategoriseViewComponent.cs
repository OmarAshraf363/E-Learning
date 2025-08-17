using Banha_UniverCity.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace Banha_UniverCity.Components
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoriesViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _unitOfWork.departmentRepository.GetAllAsync(includes: e => e.Courses);
            return View("categoriesDefaultView",categories);
        }
    }
}
