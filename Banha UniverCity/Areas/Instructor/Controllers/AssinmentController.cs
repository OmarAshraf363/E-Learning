using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;
using BFCAI.Models;
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

        public AssinmentController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManger)
        {
            _unitOfWork = unitOfWork;
            _userManger = userManger;
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
        public IActionResult UpSert(int? id, int? curriculumId,string?fromCourseArea)
        {
            ViewBag.bindId = curriculumId;
            Assignment? assignment = new();
            ViewBag.fromCourse= fromCourseArea;
            if (id == 0 || id == null)
            {
                return PartialView("_UpsertAssinment", assignment);
            }
            assignment = _unitOfWork.assinmentRepository.GetOne(e => e.AssignmentID == id);
            if (assignment == null) { return NotFound(); }
            return PartialView("_UpsertAssinment", assignment);
        }

        [HttpPost]
        public async Task<IActionResult> UpSert(Assignment assignment, IFormFile Content, string? fromCourseArea)
        {

            if (!ModelState.IsValid)
            {
                var validationErrorResult = StaticData.CheckValidation(ModelState, Request, false);
                if (validationErrorResult != null)
                {
                    return validationErrorResult;
                }
                return BadRequest();
            }
            // تحقق من وجود ملف مرفوع
            if (Content != null)
            {
                // التحقق من حجم الملف (حد أقصى 5MB)
                if (Content.Length > 5 * 1024 * 1024) // 5 MB
                {
                    ModelState.AddModelError("Content", "File size exceeds the limit of 5MB.");
                    var resultFile = StaticData.CheckValidation(ModelState, Request, false);
                    if (resultFile != null)
                    {
                        return resultFile;
                    }
                    return View(assignment);
                }

                // التحقق من نوع الملف إذا كان مطلوبًا (مثال: PDF فقط)
                if (Content.ContentType != "application/pdf")
                {
                    ModelState.AddModelError("Content", "Only PDF files are allowed.");
                    var resultFile = StaticData.CheckValidation(ModelState, Request, false);
                    if (resultFile != null)
                    {
                        return resultFile;
                    }
                    return View(assignment);
                }

                // تحديد مسار التخزين في مجلد wwwroot/assignments
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assignments", Content.FileName);

                // التحقق من وجود ملف بنفس الاسم لمنع الكتابة فوقه
                if (System.IO.File.Exists(filePath))
                {
                    ModelState.AddModelError("Content", "A file with the same name already exists.");
                    var resultFile = StaticData.CheckValidation(ModelState, Request, false);
                    if (resultFile != null)
                    {
                        return resultFile;
                    }
                    return View(assignment);
                }
                var result = StaticData.CheckValidation(ModelState, Request, true);
                if (result != null)
                {
                    return result;
                }

                // حفظ الملف بشكل غير متزامن
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Content.CopyToAsync(stream);
                }

                // تخزين اسم الملف في خاصية داخل الموديل
                assignment.Content = Content.FileName;
            }

            // إنشاء أو تعديل الـ Assignment بناءً على وجود ID
            if (assignment.AssignmentID == 0)
            {
                _unitOfWork.assinmentRepository.Create(assignment);
            }
            else
            {
                _unitOfWork.assinmentRepository.Edit(assignment);
            }

            // التحقق من الفاليديشن حسب الـ StaticData (إن وجد)
         
            // تنفيذ التغييرات في قاعدة البيانات
            _unitOfWork.Commit();
            if (fromCourseArea!=null)
            {
                return RedirectToAction("Courses", "Instructor");

            }
            else
            {

            return RedirectToAction("Index");
            }
        }
        // POST: Assignment/Delete/5

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
        public IActionResult UploadSoluation(int id)
        {
            var assignment = new AssignmentSubmission()
            {
                AssignmentID = id,
                SubmissionDate = DateTime.Now,
                ApplicationUserID = _userManger.GetUserId(User)
                
            };
            
            return PartialView("_UploadSoluation",assignment);
        }

        [HttpPost]
        public IActionResult UploadSoluation(AssignmentSubmission model ,string url) 
        {
            
            
            if (ModelState.IsValid)
            {
                
                _unitOfWork.assinmentSubmitionRepository.Create(model);
                var result = StaticData.CheckValidation(ModelState, Request, true);
                if (result != null)
                {
                    return result;
                }

                _unitOfWork.Commit();
                TempData["success"] = "Your assignment has been successfully submitted!";
                return Redirect(url);
            }
            else
            {
                var result = StaticData.CheckValidation(ModelState, Request, false);
                if (result != null)
                {
                    return result;
                }
                return BadRequest();

            }

        }





















        //[HttpPost]
        //public IActionResult UpSert(Assignment assignment)
        //{
        //    if(ModelState.IsValid)
        //    {
        //        if (assignment.AssignmentID == 0)
        //        {
        //            _unitOfWork.assinmentRepository.Create(assignment);

        //        }
        //        else
        //        {
        //            _unitOfWork.assinmentRepository.Edit(assignment);

        //        }
        //        var result=StaticData.CheckValidation(ModelState,Request,true);
        //        if(result!=null) { return result; }
        //        _unitOfWork.Commit();
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        var result = StaticData.CheckValidation(ModelState, Request, false);
        //        if (result != null) { return result; }
        //        return BadRequest();
        //    }
        //}


    }
}
