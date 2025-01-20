using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;
using Banha_UniverCity.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Banha_UniverCity.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashBoardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public DashBoardController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index(AllModelsVM models)
        {
            var allUsers = _userManager.Users.ToList();

            models.Courses = _unitOfWork.courseRepository.Get().ToList();
            models.Departments=_unitOfWork.departmentRepository.Get().ToList();
            foreach(var user in allUsers)
            {
                models.ApplicationUsers.Add(user as ApplicationUser);
            }
            models.Enrollments=_unitOfWork.enrollmentRepository.Get().ToList();
            models.Events=_unitOfWork.eventRepository.Get(null,e=>e.CreatedBy).ToList();
           ViewBag.MostPopularCourse = _unitOfWork.enrollmentRepository.Get()
     .GroupBy(e => e.CourseID)
     .OrderByDescending(g => g.Count())
     .Select(g => new {
         Name=_unitOfWork.courseRepository.GetOne(e=>e.CourseID==g.Key)?.CourseName,
         EnrollmentCount = g.Count()
     })
     .FirstOrDefault();


            return View(models);
        }
    }
}
