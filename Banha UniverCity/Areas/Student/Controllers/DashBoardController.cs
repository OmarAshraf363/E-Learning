using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;
using BFCAI.Models;
using BFCAI.Models.ViewModels;
using DataAccess.Repository.IRepository.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Banha_UniverCity.Areas.Student.Controllers
{
    [Area("Student")]
    public class DashBoardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ICourseService _courseService;
        private readonly IPostService _postService;

        public DashBoardController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, ICourseService courseService, IPostService postService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _courseService = courseService;
            _postService = postService;
        }

        public async Task<IActionResult> Index()
        {
            var studentId = _userManager.GetUserId(User);
            if (studentId == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var listOfStudentCourse = await _unitOfWork.enrollmentRepository.GetStudentCourses(studentId);





            var coursIds = listOfStudentCourse.Select(e => e.CourseID);



            var courseSchedules = _unitOfWork.classSchedulere.Get(e => coursIds.Contains(e.CourseId));



            var sortedSchedules = courseSchedules
                .OrderBy(s => s.StartTime)
                .ThenBy(s => s.StartTime)
                .ToList();




            HomeStudentViewModels model = new HomeStudentViewModels
            {
                Courses = listOfStudentCourse,
                ClassSchedule = sortedSchedules,
                Feedbacks = await _unitOfWork.feedbackRepository.GetAllAsync(e => e.TargetStudentUserId == studentId, includes: e => e.ProviderUser),
                SomeAvailableCourse = await _courseService.GetRecommendationCoursesforSpacifcStudent(studentId),
                Enrollments = _unitOfWork.enrollmentRepository.Get().ToList(),
                Posts = await _postService.GetRelatedPostsByKeyWordsAsync(studentId)

            };
            var postsIdes = model.Posts.Select(e => e.CommunityId).ToList();
            model.Communities = await _unitOfWork.communityRepository.GetAllAsync(e => postsIdes.Contains(e.Id));

            return View(model);
        }

        public IActionResult CourseDetails(int? id)
        {
            var course = _unitOfWork.courseRepository.GetOne(e => e.CourseID == id, e => e.LearningObjectives, expression => expression.TopicsCovered);
            return View(course);
        }

        public async Task<IActionResult> CourseProcess(int id)
        {
            var result = await _unitOfWork.courseRepository
             .GetProcess(id);




            return View(result);

        }

        [HttpGet]
        public async Task<IActionResult> GetContent(int id)
        {
            var content = await _unitOfWork.curriculumRepository.GetCurriculumContentAsync(id);

            return PartialView(content);
        }
        public IActionResult GetVideo(int? id)
        {
            var video = _unitOfWork.courseVideoRepository.GetOne(e => e.CourseVideoID == id);
            return Json(video.VideoURL);
        }

        [HttpGet]
        public async Task<IActionResult> UpsertFeedback(int? id, int? courseId, int? curriculumId)
        {
            Feedback feedback = new Feedback();
            if (id == null || id == 0)
            {
                var userId = _userManager.GetUserId(User);
                if (userId == null)
                {
                    return RedirectToAction("Login", "Account", new { area = "Identity" });

                }
                feedback = new Feedback()
                {
                    CourseId = courseId ?? null,
                    CourseCurriculumId = curriculumId ?? null,
                    ProviderUserId = userId,

                };
                return PartialView(feedback);
            }
            feedback = await _unitOfWork.feedbackRepository.GetOneAsync(e => e.FeedbackID == id);
            return feedback == null ? NotFound() : PartialView(feedback);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpsertFeedback(Feedback feedback)
        {
            JsonResult result;

            if (ModelState.IsValid)
            {
                if (feedback.FeedbackID == 0)
                {
                    feedback.FeedbackDate = DateTime.Now;
                    await _unitOfWork.feedbackRepository.AddAsync(feedback);
                    TempData["success"] = "Feedback added successfully.";
                }
                else
                {
                    await _unitOfWork.feedbackRepository.UpdateAsync(feedback);
                    TempData["success"] = "Feedback updated successfully.";
                }

                _unitOfWork.Commit();
                result = StaticData.CheckValidation(ModelState, Request, true);
                if (feedback.CourseId != null)
                {
                    await _courseService.EditRatingCourseBasedOnReviews((int)feedback.CourseId);

                }
                return result;
            }
            else
            {
                result = StaticData.CheckValidation(ModelState, Request, false);
                return result;
            }


        }


        [HttpGet]


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

            var questionsVM = questions.Select(e => new
            {
                e.QuestionText,
                choices = e.Choices.Select(e => e.ChoiceText).ToArray()
            });
            if (questions == null || !questions.Any())
            {
                return Json(new { message = "No questions found", data = new List<Question>() });
            }

            return Json(questionsVM);
        }
        public IActionResult GetOne(int id)
        {
            var question = _unitOfWork.qusetionRepository.Get(e => e.QuestionID == id, e => e.Choices);
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


        public IActionResult Delete(int id, string Url)
        {
            var item = _unitOfWork.feedbackRepository.GetOne(e => e.FeedbackID == id);
            if (item == null) { return NotFound(); }
            _unitOfWork.feedbackRepository.Delete(item);
            _unitOfWork.Commit();
            return Redirect(Url);
        }







        [HttpGet]

        public async Task<IActionResult> QuickAccess(int id)
        {
            var course = await _unitOfWork.courseRepository.QuickAccessToSpacifcCourse(id);
            if (course == null)
                return BadRequest();
            return PartialView(course);

        }

    }
}
