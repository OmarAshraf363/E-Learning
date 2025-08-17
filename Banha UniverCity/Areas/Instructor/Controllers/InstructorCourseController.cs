using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;
using Banha_UniverCity.ViewModels;
using BFCAI.Utility.Helper;
using BFCAI.Utility.Shared;
using DataAccess.Repository.IRepository.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Banha_UniverCity.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    public class InstructorCourseController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICourseService _courseService;

        private readonly UserManager<IdentityUser> _userManager;

        public InstructorCourseController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, ICourseService courseService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _courseService = courseService;
        }

        public async Task<IActionResult> Index(PaginationParam? param)
        {
            var listOfCourses = _unitOfWork.courseRepository.Get(e => e.InstructorId == _userManager.GetUserId(User), e => e.CourseCurricula);
            var pagedList = await listOfCourses.ToPagedListAsync(param.Page, param.PageSize); // Assuming you have a method to handle pagination
            return View(pagedList);
        }
        [HttpGet]
        public async Task<IActionResult> UpSert(int? id, int? deptId, int? trackId)
        {
            var instructorId = _userManager.GetUserId(User);
            var model = await _courseService.PrepareCourseViewModelAsync(id, deptId, trackId, instructorId);

            if (model == null)
                return NotFound();

            return View("UpsertCourseByInstructor", model);
        }

        [HttpPost]
        public async Task<IActionResult> UpSert(CourseVM model, IFormFile ImgCover)
        {
            if (!ModelState.IsValid)
            {
                var instructorId = _userManager.GetUserId(User);
                model = await _courseService.PrepareCourseViewModelAsync(model.CourseID, model.DepartmentId, model.TrackId, instructorId);
                return View("UpsertCourseByInstructor", model);
            }

            var (success, errorMessage) = await _courseService.SaveCourseAsync(model, ImgCover);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, errorMessage);
                ViewBag.errorr = errorMessage;
                return View("UpsertCourseByInstructor", model);
            }

            TempData["alert"] = model.CourseID == 0 ? "Added successfully" : "Edited successfully";
            return RedirectToAction("Index");
        }



    }
}
