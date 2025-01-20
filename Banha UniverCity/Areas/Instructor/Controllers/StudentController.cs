using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Banha_UniverCity.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    public class StudentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public StudentController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public IActionResult Index(int? courseId)
        {
            var students = _unitOfWork.enrollmentRepository.Get(e => e.CourseID == courseId, e => e.Student)
                   .Select(e => e.Student)
                   .ToList();
            ViewBag.listOfCourses = _unitOfWork.courseRepository.Get(e => e.InstructorId == _userManager.GetUserId(User)).Select(e => new SelectListItem
            {
                Text = e.CourseName,
                Value = e.CourseID.ToString()
            });
            if (courseId.HasValue)
            {

                ViewBag.courseId = courseId.Value;
            }
            return View(students);
        }
        [HttpGet]
        public IActionResult InsertFeedBack(string? id)
        {
            Feedback feedback = new Feedback()
            {
                TargetStudentUserId=id,
                ProviderUserId=_userManager.GetUserId(User),
                FeedbackDate=DateTime.Now,
                
            };
            return PartialView("InsertFeedBack", feedback);
        }
        [HttpPost]
        public IActionResult InsertFeedBack(Feedback feedback)
        {
            JsonResult result;
            if (ModelState.IsValid)
            {
                if (feedback.FeedbackID == 0)
                {
                    _unitOfWork.feedbackRepository.Create(feedback);
                    TempData["success"] = "Added Successfully";
                }
                else
                {
                    _unitOfWork.feedbackRepository.Edit(feedback);
                    TempData["success"] = "Added Successfully";

                }
                _unitOfWork.Commit();
                result = StaticData.CheckValidation(ModelState, Request, true);
                return result;
            }
            else
            {
                result = StaticData.CheckValidation(ModelState, Request, false);
                return result;
            }

        }
    }
}
