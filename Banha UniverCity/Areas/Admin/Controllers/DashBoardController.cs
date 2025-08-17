using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;
using Banha_UniverCity.ViewModels;
using DataAccess.Repository.IRepository.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Banha_UniverCity.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashBoardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ICourseService _courseService;

        public DashBoardController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, ICourseService courseService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _courseService = courseService;
        }

        public async Task<IActionResult> Index(AllModelsVM models)
        {
            var allUsers = _userManager.Users.ToList();

            models.PagedCourses = await _unitOfWork.courseRepository.GetListOfCoursesDetailsWithPagination();
            models.Departments=_unitOfWork.departmentRepository.Get().ToList();
            foreach(var user in allUsers)
            {
                models.ApplicationUsers.Add(user as ApplicationUser);
            }
            models.Enrollments=_unitOfWork.enrollmentRepository.Get().ToList();
            models.Events=_unitOfWork.eventRepository.Get(null,e=>e.CreatedBy).ToList();
            models.MaxEnrollmentCourses = await _courseService.GetCoursesWIthMaxEnrollments();


            return View(models);
        }
    }
}
