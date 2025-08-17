using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;
using Banha_UniverCity.ViewModels;
using BFCAI.Models;
using BFCAI.Models.ViewModels;
using DataAccess.Repository.IRepository.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Banha_UniverCity.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    public class InstructorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IvedioService _vedioService;

        public InstructorController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, IvedioService vedioService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _vedioService = vedioService;
        }
        public IActionResult Dashboard()
        {

            // Get the instructor's ID from the logged-in user
            var instructorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Fetch data related to the instructor
            var totalCourses = _unitOfWork.courseRepository.Get(c => c.InstructorId == instructorId).Count();
            var totalExams = _unitOfWork.examRepository.Get(e => e.InstructorId == instructorId).Count();
            var totalQuestions = _unitOfWork.qusetionRepository.Get(q => q.Exam.InstructorId == instructorId).Count();
            var totalStudents = _unitOfWork.enrollmentRepository.Get(e => e.Course.InstructorId == instructorId).Select(e => e.StudentId).Distinct().Count();

            // Pass data to the view model
            var model = new BFCAI.Models.ViewModels.InstructorDashboardViewModel
            {
                TotalCourses = totalCourses,
                TotalExams = totalExams,
                TotalQuestions = totalQuestions,
                TotalStudents = totalStudents
            };

            return View(model);
        }

        public IActionResult Courses()
        {
            var listOfCourses = _unitOfWork.courseRepository.Get(e => e.InstructorId == _userManager.GetUserId(User),e=>e.CourseCurricula);
            return View(listOfCourses);
        }

   


        [HttpGet]
        public IActionResult UpsertCourseCurriculum(int? id, int courseId)
        {
            CourseCurriculum model = new CourseCurriculum
            {
                CourseID = courseId
            };

            if (id.HasValue)
            {
                model = _unitOfWork.curriculumRepository.GetOne(e=>e.CourseCurriculumID==id.Value);
                if (model == null)
                {
                    return NotFound();
                }
            }

            return PartialView("_UpsertCourseCurriculum", model);
        }

        [HttpPost]
        public IActionResult UpsertCourseCurriculum(CourseCurriculum model)
        {
            if (ModelState.IsValid)
            {
                if (model.CourseCurriculumID == 0)
                {
                    
                    // إضافة جديد
                    _unitOfWork.curriculumRepository.Create(model);
                }
                else
                {
                    // تعديل المنهج الحالي
                    _unitOfWork.curriculumRepository.Edit(model);
                }

                _unitOfWork.Commit();
                return RedirectToAction("Details", "Course", new { id = model.CourseID });
            }

            return PartialView("_UpsertCourseCurriculum", model);
        }

        public IActionResult GetCourseCurriculum(int courseId)
        {
            var course = _unitOfWork.courseRepository.GetCourseCurriculum(courseId);
            if (course == null)
            {
                return NotFound();
            }

            return PartialView("_CourseCurriculumPartial", course);
        }
        
        [HttpGet]
        public IActionResult UpsertCourseVideo(int? id, int curriculumId)
        {
            var thisCourse = _unitOfWork.curriculumRepository.GetOne(e => e.CourseCurriculumID == curriculumId);
            var courseVideo = new UpsertVideoVM();

           
                courseVideo.CurriculumId = curriculumId;
                courseVideo.CourseId = thisCourse.CourseID;
                return PartialView("_UpsertCourseVideo", courseVideo);
            
        }

        [HttpPost]
        public IActionResult UpsertCourseVideo(CourseVideo courseVideo)
        {
            if (ModelState.IsValid)
            {
                if (courseVideo.CourseVideoID == 0)
                {
                    _unitOfWork.courseVideoRepository.Create(courseVideo);
                }
                else
                {
                    _unitOfWork.courseVideoRepository.Edit(courseVideo);
                }
                var result = StaticData.CheckValidation(ModelState, Request, true);
                if (result != null) { return result; }
                _unitOfWork.Commit();

                return RedirectToAction(nameof(Courses));

               
                

            }
            else
            {
                var result = StaticData.CheckValidation(ModelState, Request, false);
                  return result; 
               
            }
        }



        [HttpGet]
        public IActionResult UpsertReference(int? id, int curriculumId)
        {
            CourseResource reference = new();

            
            if (id == null || id == 0)
            {
                reference.CourseCurriculumID = curriculumId;
                return PartialView("_UpsertReference", reference);
            }

            reference = _unitOfWork.courseResourceRepository.GetOne(e => e.CourseResourceID == id && e.CourseCurriculumID == curriculumId);
            if (reference == null)
            {
                return NotFound();
            }

            return PartialView("_UpsertReference", reference);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpsertReference(CourseResource reference)
        {
            if (ModelState.IsValid)
            {
                if (reference.CourseResourceID == 0)
                {
                    _unitOfWork.courseResourceRepository.Create(reference);
                }
                else
                {
                    _unitOfWork.courseResourceRepository.Edit(reference);
                }


                // تأكيد التغييرات في قاعدة البيانات
                var result = StaticData.CheckValidation(ModelState, Request, true);
                if(result!=null) { return result; }
                _unitOfWork.Commit();
                return RedirectToAction(nameof(Courses));

             

            }
            else
            {

                var result = StaticData.CheckValidation(ModelState, Request, false);
                return result;
            }
        }


        [HttpGet]
        public IActionResult UpsertQuiz(int? id,int? curriculumId)
        {
            Exam exam = new Exam();
         
            if (id == null||id==0)
            {
            exam.CourseID = _unitOfWork.curriculumRepository.GetOne(e => e.CourseCurriculumID == curriculumId).CourseID;
            exam.InstructorId = _userManager.GetUserId(User);
            exam.CurriculumId = curriculumId;
                return PartialView("_UpSertQuiz", exam);
            }

            exam = _unitOfWork.examRepository.GetOne(e => e.ExamID == id);
            if (exam == null)
            {
                return NotFound();
            }
            
            return PartialView("_UpSertQuiz", exam);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpsertQuiz(Exam exam)
        {
            if (ModelState.IsValid)
            {

                if (exam.ExamID == 0)
                {
                    _unitOfWork.examRepository.Create(exam);
                }
                else
                {
                    _unitOfWork.examRepository.Edit(exam);
                }

                _unitOfWork.Commit();
                var result = StaticData.CheckValidation(ModelState, Request, true);
                return result;
            }
            else
            {

                var result = StaticData.CheckValidation(ModelState, Request, false);
                return result;
            }

        }

        [HttpPost]
        public async Task<IActionResult> UploadVedio(UpsertVideoVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var result = StaticData.CheckValidation(ModelState, Request, false);
                    return result;

                }
                var videoSrc = await _vedioService.UploadVedioAsync(model.VedioUrl);
                if (videoSrc != null)
                {
                var vedio = new CourseVideo();
                    vedio.VideoTitle = model.Title;
                    vedio.VideoURL = videoSrc;
                    vedio.CourseCurriculumID = model.CurriculumId;
                    vedio.CourseID = model.CourseId;
                    _unitOfWork.courseVideoRepository.Create(vedio);
                    _unitOfWork.Commit();
                    var result = StaticData.CheckValidation(ModelState, Request, true);
                    return result ;

                }
                return BadRequest();

            }
            catch (Exception)
            {

                return BadRequest();
            }
         


        }



        public IActionResult GetQuestions(int id) 
        {
            var listOfQuestions=_unitOfWork.qusetionRepository
                .Get(e=>e.ExamID==id,e=>e.Choices);
            ViewBag.ExamId = id;
            return PartialView("Questions",listOfQuestions);
        }

        public IActionResult DeleteQuiz(int id) { 
            var exam=_unitOfWork.examRepository.GetOne(e=>e.ExamID == id);
            if(exam == null) { return NotFound(); }
            _unitOfWork.examRepository.Delete(exam);
            _unitOfWork.Commit();
             return RedirectToAction(nameof(Dashboard));

        }
        public IActionResult DeleteReference(int id,int curriculumId)
        {
            var item = _unitOfWork.courseResourceRepository.GetOne(e => e.CourseResourceID == id && e.CourseCurriculumID == curriculumId);
            if(item == null)
            {
                return NotFound();
            }
            _unitOfWork.courseResourceRepository.Delete(item);
            _unitOfWork.Commit();
            return RedirectToAction(nameof(Courses));
        }
        public IActionResult DeleteVideo(int id, int curriculumId,int courseId)
        {
            var item = _unitOfWork.courseVideoRepository.GetOne(e => e.CourseVideoID == id && e.CourseCurriculumID == curriculumId&&e.CourseID==courseId);
            if (item == null)
            {
                return NotFound();
            }
            _unitOfWork.courseVideoRepository.Delete(item);
            _unitOfWork.Commit();
            return RedirectToAction(nameof(Courses));
        }
    }
}
