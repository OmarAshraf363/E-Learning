using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;
using BFCAI.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Banha_UniverCity.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EnrollmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public EnrollmentController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var courses = _unitOfWork.courseRepository.Get().Select(c => new SelectListItem
            {
                Text = c.CourseName,
                Value = c.CourseID.ToString()
            }).ToList();

            var enrollments = _unitOfWork.enrollmentRepository.Get(null,e=>e.Course,e=>e.Student)
                                   .OrderByDescending(e => e.EnrollmentID)
                                   .Take(10) 
                                   .ToList();

            var totalEnrollments = _unitOfWork.enrollmentRepository.Get().Count();

            var model = new EnrollmentViewModel
            {
                Courses = courses,
                Enrollments = enrollments,
                TotalEnrollments = totalEnrollments
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> EnrollByEmail(EnrollmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    TempData["Error"] = "Student with this email does not exist.";
                    return RedirectToAction(nameof(Index));
                }

                var enrollment = new Enrollment
                {
                    CourseID = model.CourseId,
                    StudentId = user.Id
                };

                _unitOfWork.enrollmentRepository.Create(enrollment);
                 _unitOfWork.Commit();

                TempData["Success"] = "Student has been successfully enrolled.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Invalid data provided.";
            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        public async Task<IActionResult> UploadCsv(IFormFile CsvFile)
        {
            if (CsvFile != null && CsvFile.Length > 0)
            {
                using (var stream = new StreamReader(CsvFile.OpenReadStream()))
                {
                    string line;
                    while ((line = await stream.ReadLineAsync()) != null)
                    {
                        var fields = line.Split(',');

                        if (fields.Length >= 2)
                        {
                            var studentId = fields[0];
                            var courseId = int.Parse(fields[1]);

                            // تأكد من وجود الطالب والكورس
                            var student = _userManager.Users.FirstOrDefault(e => e.Id == studentId);
                            var course = _unitOfWork.courseRepository.GetOne(e=>e.CourseID==courseId);

                            if (student != null && course != null)
                            {
                                // قم بعملية التسجيل (Enrollment)
                                var enrollment = new Enrollment
                                {
                                    StudentId = studentId,
                                    CourseID = courseId,
                                
                                };

                                _unitOfWork.enrollmentRepository.Create(enrollment);
                            }
                        }
                    }
                }

                // حفظ التغييرات
            _unitOfWork.Commit();

                return RedirectToAction(nameof(Index));
            }

            return View();
        }


    }


}
