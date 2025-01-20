using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;
using BFCAI.Models;
using BFCAI.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Banha_UniverCity.Areas.Student.Controllers
{
    [Area("Student")]
    public class DashBoardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public DashBoardController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var studentId = _userManager.GetUserId(User);

           
            var listOfStudentCourse = _unitOfWork.enrollmentRepository
                .Get(e => e.StudentId == studentId,e=>e.Course).AsQueryable()
                .Include(e => e.Course) // تضمين الكورس
                    .ThenInclude(c => c.CourseCurricula) // تضمين المنهج
                    .ThenInclude(cc => cc.CourseResources) // تضمين الموارد
                .Include(e => e.Course) // تضمين الكورس
                    .ThenInclude(c => c.CourseCurricula) // تضمين المنهج
                    .ThenInclude(cc => cc.CourseVideos) // تضمين الفيديوهات
                .Include(e => e.Course) // تضمين الكورس
                    .ThenInclude(c => c.CourseCurricula) // تضمين المنهج
                    .ThenInclude(cc => cc.Assignments) // تضمين الواجبات
                .Select(e => e.Course) // الحصول على الكورس بعد تضمين العلاقات

             

                .ToList();


            var coursIds = listOfStudentCourse.Select(e => e.CourseID);



            var courseSchedules =_unitOfWork.classSchedulere.Get(e=>coursIds.Contains(e.CourseId));
         

        
            var sortedSchedules = courseSchedules
                .OrderBy(s => s.StartTime) 
                .ThenBy(s => s.StartTime)  
                .ToList();

            var listOfNonEnrolledCourse = _unitOfWork.courseRepository.Get(e => !coursIds.Contains(e.CourseID), e => e.Instructor).ToList();



            HomeStudentViewModels model = new HomeStudentViewModels
            {
                Courses = listOfStudentCourse,
                ClassSchedule = sortedSchedules,
                Feedbacks = _unitOfWork.feedbackRepository.Get(e => e.TargetStudentUserId == studentId,e=>e.ProviderUser).ToList(),
                SomeAvailableCourse = listOfNonEnrolledCourse,
                Enrollments=_unitOfWork.enrollmentRepository.Get().ToList()
                
            };

            return View(model);
        }

        public IActionResult CourseDetails(int? id)
        {
            var course=_unitOfWork.courseRepository.GetOne(e=>e.CourseID== id,e=>e.LearningObjectives,expression=>expression.TopicsCovered);
            return PartialView(course);
        }


        [HttpGet]
        public IActionResult GetContent(int? id)
        {
            var content = _unitOfWork.curriculumRepository.GetCurriculumWithIncluded(id);
          
            return PartialView(content);
        }
        public IActionResult GetVideo(int? id)
        {
            var video = _unitOfWork.courseVideoRepository.GetOne(e => e.CourseVideoID == id);
            return Json(video);
        }

        [HttpGet]
        public IActionResult UpsertFeedback(int? id, int courseId)
        {
            Feedback feedback;
            if (id == null||id==0)
            {
                feedback = new Feedback()
                {
                    CourseId = courseId,
                    ProviderUserId = _userManager.GetUserId(User),
                    FeedbackDate = DateTime.Now,

                };
                return PartialView("_UpsertFeedback", feedback);
            }
            feedback = _unitOfWork.feedbackRepository.GetOne(e => e.FeedbackID == id);
            return feedback == null ? NotFound() : View("_UpsertFeedback", feedback);
        }
        [HttpPost]
        public IActionResult UpsertFeedback(Feedback feedback)
        {
            JsonResult result;
            if(ModelState.IsValid)
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


    

        [HttpGet]
        public IActionResult GetQuestions(int id)
        {
            var exam = _unitOfWork.examRepository.GetOne(e => e.CurriculumId == id);
            if (exam == null)
            {
                return NotFound(new { message = "Exam not found" });
            }

            var questions = _unitOfWork.qusetionRepository
                .Get(e => e.ExamID == exam.ExamID, e => e.Choices);
                
            var questionsVM=questions.Select(e => new
            {
                e.QuestionText,
               choices=e.Choices.Select(e=>e.ChoiceText).ToArray()
            });
            if (questions == null || !questions.Any())
            {
                return Json(new { message = "No questions found", data = new List<Question>() });
            }

            return Json(questionsVM);
        }
        public IActionResult GetOne(int id)
        {
            var question = _unitOfWork.qusetionRepository.Get(e => e.QuestionID == id,e=>e.Choices);
            if (question == null)
            {
                return NotFound(new { message = "Question not found" });
            }
            var questionVM = question.Select(e => new
            {
                e.QuestionText,
                choices = e.Choices.Select(e => e.ChoiceText).ToArray()
            }).FirstOrDefault();
            return Json(questionVM);
        }


        public IActionResult Delete(int id,string Url)
        {
            var item = _unitOfWork.feedbackRepository.GetOne(e => e.FeedbackID == id);
            if (item == null) { return NotFound(); }
            _unitOfWork.feedbackRepository.Delete(item);
            _unitOfWork.Commit();
            return Redirect(Url);
        }

    }
}
