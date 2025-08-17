using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;
using BFCAI.Models;
using DataAccess.Repository.IRepository.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Banha_UniverCity.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    public class AssinmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManger;
        private readonly IAssinmentService _assinmentService;


        public AssinmentController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManger, IAssinmentService assinmentService)
        {
            _unitOfWork = unitOfWork;
            _userManger = userManger;
            _assinmentService = assinmentService;
        }

        public IActionResult Index(int? courseId)
        {
            ViewBag.Courses = _unitOfWork.courseRepository.Get(e=>e.InstructorId==_userManger.GetUserId(User)).Select(e => new SelectListItem
            {
                Text = e.CourseName,
                Value = e.CourseID.ToString()
            }).ToList();
            if (courseId.HasValue)
            {
                var listOfCourseProcess = _unitOfWork.curriculumRepository
                    .Get(e => e.CourseID == courseId, e => e.Assignments);
                ViewBag.courseId = courseId;
                return View(listOfCourseProcess);

            }

            ViewBag.Message = "Please select a Course.";
            return View(new List<CourseCurriculum>());

        }
        [HttpGet]
        public IActionResult UpSert(int? id, int curriculumId)
        {
            
            Assignment? assignment = new();
            assignment.CourseCurriculumID = curriculumId;
          
            if (id == 0 || id == null)
            {
                return PartialView("_UpsertAssinment", assignment);
            }
            assignment = _unitOfWork.assinmentRepository.GetOne(e => e.AssignmentID == id);
            if (assignment == null) { return NotFound(); }
            return PartialView("_UpsertAssinment", assignment);
        }

        [HttpPost]
        public async Task<IActionResult> UpSert(Assignment assignment, IFormFile Content)
        {
            JsonResult result;

            if (!ModelState.IsValid)
            {
                result = StaticData.CheckValidation(ModelState, Request, false);
                return result;
            }
           var msg=await _assinmentService.UploadAssignmentAsync(Content);
            if (msg == null)
            {
                ModelState.AddModelError("Content", "Invalid file format or size. Please upload a valid file.");
                result = StaticData.CheckValidation(ModelState, Request, false);
                return result;
            }
            assignment.Content = msg; 

            // إنشاء أو تعديل الـ Assignment بناءً على وجود ID
            if (assignment.AssignmentID == 0)
            {
                _unitOfWork.assinmentRepository.Create(assignment);
            }
            else
            {
                _unitOfWork.assinmentRepository.Edit(assignment);
            }

         
         
          
            _unitOfWork.Commit();

          result= StaticData.CheckValidation(ModelState, Request, true);
            return result;
        }
        

        public IActionResult DeleteConfirmed(int id)
        {
            var assignment = _unitOfWork.assinmentRepository.GetOne(e => e.AssignmentID == id);
            if (assignment == null)
            {
                return NotFound();
            }

            // Optionally: Remove associated files if needed
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assignments", assignment.Content);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _unitOfWork.assinmentRepository.Delete(assignment);
            _unitOfWork.Commit();

            return RedirectToAction(nameof(Index));
        }





        public IActionResult GetAssignmentDetails(int id)
        {
            var assignment = _unitOfWork.assinmentRepository.GetOne(e => e.AssignmentID == id);
            if (assignment == null)
            {
                return NotFound();
            }

            return PartialView("_AssignmentDetailsPartial", assignment); // Partial view for assignment details
        }






        public IActionResult DownloadAssignment(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assignments", fileName);

            if (System.IO.File.Exists(filePath))
            {
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, "application/pdf", fileName);
            }

            return NotFound();
        }





        [HttpGet]
        public IActionResult GetSubmitions(int id)
        {
            var submissions = _unitOfWork.assinmentSubmitionRepository.Get(e => e.AssignmentID == id, e => e.ApplicationUser);
            //if (submissions == null || !submissions.Any())
            //{
            //    return NotFound("No submissions found for this assignment.");
            //}
            return PartialView("_SubmissionsPartial", submissions); // Partial view for displaying submissions





        }












        }
}
