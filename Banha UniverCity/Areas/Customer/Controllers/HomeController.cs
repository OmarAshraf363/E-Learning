using Banha_UniverCity.Areas.Admin.Controllers;
using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;
using Banha_UniverCity.ViewModels;
using BFCAI.Models.ViewModels;
using DataAccess.Repository.ModelsRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using Stripe;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Banha_UniverCity.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            AllModelsVM model = new AllModelsVM();
            model.Courses = _unitOfWork.courseRepository.Get(null,
                e => e.CourseCurricula,
                e => e.TopicsCovered,
                e => e.LearningObjectives,
                expression => expression.Department, expression => expression.Enrollments, e => e.Instructor).ToList();
            model.Departments = _unitOfWork.departmentRepository.Get(null, e => e.Courses).ToList();
            model.Feedbacks = _unitOfWork.feedbackRepository.Get(null, e => e.Course, e => e.ProviderUser).ToList();
            model.Events = _unitOfWork.eventRepository.Get(null, e => e.CreatedBy, e => e.Department).ToList();
            model.Tracks = _unitOfWork.trackRepository.Get(null, e => e.Courses).ToList();
            model.Enrollments = _unitOfWork.enrollmentRepository.Get().ToList();

            if (User.IsInRole(StaticData.role_Student))
            {

                var coursesEnrolledByStudent = _unitOfWork.enrollmentRepository.Get(e => e.StudentId == _userManager.GetUserId(User)).Select(e => e.Course).ToList();
                var trackIdes = coursesEnrolledByStudent.Select(e => e.TrackID).ToList();
                var StudentTrackCourses = _unitOfWork.trackRepository.GetOne(e => trackIdes.Contains(e.Id))?.Courses;
                if (StudentTrackCourses != null)
                {

                    model.RecommendationCourses = StudentTrackCourses;
                }
            }

            if (User.IsInRole(StaticData.role_Admin))
            {
                return RedirectToAction("Index", "DashBoard", new { Area = StaticData.role_Admin });
            }
            if (User.IsInRole(StaticData.role_Instructor))
            {
                return RedirectToAction("Dashboard", "Instructor", new { Area = StaticData.role_Instructor });
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult GetByDepTId(int? id)
        {
            var deptDescription = _unitOfWork.departmentRepository.GetOne(e => e.DepartmentID == id)?.DepartmentDescription;
            var courses = _unitOfWork.courseRepository.Get(null,
                e => e.CourseCurricula,
                e => e.TopicsCovered,
                e => e.LearningObjectives,
                expression => expression.Department, expression => expression.Enrollments, e => e.Instructor);
            if (id != 0 && id != null)
            {

                courses = courses.Where(e => e.DepartmentId == id);


                var courseVMs = courses.Select(course => new
                {
                    course.CourseID,
                    course.CourseName,
                    course.ImgCover,
                    course.Department?.DepartmentName,
                    departmentDescription = deptDescription,
                    Instructor = course.Instructor != null ? course.Instructor.FullName : "Unknown",
                    course.Price,
                    course.Description,
                    Rate = course.Rate ?? 0,
                    EnrollmentsCount = course.Enrollments.Count,
                    videosCount = course.CourseCurricula.Count,
                    TopicsCovered = course.TopicsCovered?.Select(topic => topic.Topic),
                    LearningObjectives = course.LearningObjectives?.Select(obj => obj.Objective)
                }).ToArray();

                return Json(courseVMs);
            }
            else
            {
                var courseVMs = courses.Select(course => new
                {
                    course.CourseID,
                    course.CourseName,
                    course.Department?.DepartmentName,
                    course.ImgCover,
                    departmentDescription = "",
                    Instructor = course.Instructor != null ? course.Instructor.FullName : "Unknown",
                    course.Price,
                    course.Description,
                    Rate = course.Rate ?? 0,
                    EnrollmentsCount = course.Enrollments.Count,
                    videosCount = course.CourseCurricula.Count,
                    TopicsCovered = course.TopicsCovered?.Select(topic => topic.Topic),
                    LearningObjectives = course.LearningObjectives?.Select(obj => obj.Objective)
                }).ToArray();
                return Json(courseVMs);

            }
        }

        public IActionResult Details(int id)
        {
            var course = _unitOfWork.courseRepository.GetCourseCurriculum(id);
            var model = new DetailsVM()
            {
                Course = course,
                RelatedCourses = _unitOfWork.courseRepository.Get(e => e.DepartmentId == course.DepartmentId && e.CourseID != course.CourseID).ToList(),
                InstructorCourses = _unitOfWork.courseRepository
                .Get(e => e.InstructorId == course.InstructorId && e.CourseID != course.CourseID).ToList(),
            };
            return View(model);
        }
        public IActionResult GetObjectives(int? id)
        {
            var objectives = _unitOfWork.learningObjectiveRepository.Get(e => e.CourseID == id);

            var objectivesVM = objectives.Select(e => new { e.Objective }).ToArray();

            return Json(objectivesVM);
        }
        public IActionResult GetTopics(int? id)
        {
            var Topics = _unitOfWork.topicCoveresRepository.Get(e => e.CourseID == id);

            var TopicsVM = Topics.Select(e => new { e.Topic }).ToArray();

            return Json(TopicsVM);
        }

        public IActionResult Courses(CoursePageVM model,string?instructorId ,int? id, int? categoryFilter, int? rate, decimal? price, string? search)
        {

            ViewBag.CourseID = id;
            if (id == null)
            {

                var listOfCourses = _unitOfWork.courseRepository
                    .Get(null,
                    e => e.Enrollments,
                    e=> e.TopicsCovered,
                    e => e.LearningObjectives,
                    e => e.Department,
                    e=>e.Instructor

                    );
                model.DepartmentList = _unitOfWork.departmentRepository.Get().ToList();
                model.CourseList = listOfCourses.ToList();
                if (!string.IsNullOrWhiteSpace(search))
                {
                    var lowerSearch = search.ToLower();
                    model.CourseList = model.CourseList.Where(e => e.CourseName.ToLower().IndexOf(lowerSearch, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                }

                // Rate filter
                if (rate.HasValue)
                {
                    model.CourseList = model.CourseList.Where(e => e.Rate >= rate.Value).ToList();
                }

                // Category filter
                if (categoryFilter.HasValue)
                {
                    model.CourseList = model.CourseList.Where(e => e.DepartmentId == categoryFilter.Value).ToList();
                }

                // Price filter
                if (price.HasValue)
                {
                    model.CourseList = model.CourseList.Where(e => e.Price <= price.Value).ToList();
                }
                if (!string.IsNullOrEmpty(instructorId))
                {
                    model.CourseList=model.CourseList.Where(e=>e.InstructorId== instructorId).ToList();
                    ViewBag.InstructorId = instructorId;
                }

                // set view bags 
                ViewBag.categoryFilter = categoryFilter;
                ViewBag.rate = rate;
                ViewBag.price = price;
                ViewBag.Search = search?.ToLower();
                


                return View(model);


            }
            else
            {
                var course = _unitOfWork.courseRepository.GetCourseCurriculum(id);
                var modelDetails = new DetailsVM()
                {
                    Course = course,
                    RelatedCourses = _unitOfWork.courseRepository.Get(e => e.DepartmentId == course.DepartmentId,e=>e.LearningObjectives).ToList(),
                    InstructorCourses = _unitOfWork.courseRepository
                    .Get(e => e.InstructorId == course.InstructorId && e.CourseID != course.CourseID,e=>e.LearningObjectives).ToList(),
                };
                if (course.TrackID != null)
                {
                    var sameCoursesInTrack = _unitOfWork.trackRepository.GetOne(e => e.Id == course.TrackID)?.Courses;
                    if (sameCoursesInTrack != null)
                    {

                        modelDetails.RelatedCourses.AddRange(sameCoursesInTrack);
                        modelDetails.RelatedCourses=modelDetails.RelatedCourses.Distinct().ToList();
                    }
                }
                return View("Details", modelDetails);
            }


        }

        public IActionResult Category(int? id)
        {
            if (id == null)
            {
                var listOfCategories = _unitOfWork.departmentRepository.Get();
                var modelDetails = new DepartmentCoursesVM()
                {
                    Departments = listOfCategories.ToList(),
                };
                return View(modelDetails);
            }
            else
            {

                var ides = _unitOfWork.courseRepository.Get(e => e.DepartmentId == id).Select(e => e.CourseID).ToList();
                var model = new DepartmentCoursesVM()
                {
                    Department = _unitOfWork.departmentRepository.GetOne(e => e.DepartmentID == id),
                    Courses = _unitOfWork.courseRepository.Get(e => e.DepartmentId == id, e => e.LearningObjectives).ToList(),
                    KeyWords = _unitOfWork.keyWordRepository.Get(e => ides.Contains((int)e.CourseId)).ToList(),

                };


                return View(model);
            }
        }

        public IActionResult TopicCourse(string? word)
        {
            var wordToLowwer = word.ToLower();
            var ides = _unitOfWork.keyWordRepository
                .Get(e => e.Name.ToLower().Contains(wordToLowwer)).Select(e => e.CourseId);
            var listOfCourses = _unitOfWork.courseRepository
                .Get(e => ides.Contains(e.CourseID) || e.CourseName.ToLower().Contains(wordToLowwer));
            ViewBag.word = word;
            return View(listOfCourses);
        }


        public IActionResult Filter(string? topic)
        {
            var courses = _unitOfWork.keyWordRepository.Get(e => e.Name.Contains(topic), e => e.Course).ToList();
            return View(courses);
        }

        public IActionResult TrackDetails(int id)
        {
            var track = _unitOfWork.trackRepository.GetOne(e => e.Id == id,
                e=>e.Courses);
            var trackCoursesIdes = _unitOfWork.courseRepository.Get(e => e.TrackID == id).Select(e=>e.CourseID).ToList();
            ViewBag.count=_unitOfWork.enrollmentRepository.Get(e=>trackCoursesIdes.Contains((int)e.CourseID)).Count();
            
            return View("~/Views/Shared/Details.cshtml", track);
        }

        public IActionResult GetRegister()
        {

            return PartialView("_RegisterPartial", new InstructorVM());
        }
        [HttpPost]
        public async Task<IActionResult> PostRegister(InstructorVM model)
        {
            JsonResult check;
            if (ModelState.IsValid)
            {
                // إنشاء المستخدم الجديد بناءً على البيانات التي تم إدخالها
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName

                };

                // منطق إنشاء المستخدم
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // إذا كان المستخدم يدرس في BFCAI، أضفه إلى دور Instructor

                    await _userManager.AddToRoleAsync(user, StaticData.role_Instructor);



                    // إرسال تأكيد البريد الإلكتروني أو تسجيل الدخول حسب الحاجة
                    // يمكن تعديل هذا الجزء بناءً على حاجاتك الخاصة
                    check = StaticData.CheckValidation(ModelState, Request, true);
                    return check;
                }
                else
                {
                    // في حالة وجود أخطاء في إنشاء المستخدم، قم بإضافة الأخطاء إلى ModelState
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("Email", error.Description);
                    }
                    check = StaticData.CheckValidation(ModelState, Request, false);
                    return check;
                }
            }
            else
            {


                check = StaticData.CheckValidation(ModelState, Request, false);
                return check;
            }
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
