using Banha_UniverCity.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Banha_UniverCity.Areas.Student.Controllers
{
    [Area("Student")]
    public class CoursesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public CoursesController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var courses=_unitOfWork.enrollmentRepository.Get(e=>e.StudentId==userId,e=>e.Course).Select(e=>e.Course).ToList();
            ViewBag.prog=_unitOfWork.progressRepository.Get();
            return View(courses);
        }
    }
}
