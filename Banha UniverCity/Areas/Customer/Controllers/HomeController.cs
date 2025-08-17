using Banha_UniverCity.Areas.Admin.Controllers;
using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;
using Banha_UniverCity.ViewModels;
using BFCAI.Models.ViewModels;
using BFCAI.Utility.Helper;
using BFCAI.Utility.Shared;
using DataAccess.Repository.IRepository.Service;
using DataAccess.Repository.ModelsRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using Stripe;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Banha_UniverCity.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ICourseService _courseService;
        private readonly ICacheService _cacheService;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, ICourseService courseService, ICacheService cacheService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _courseService = courseService;
            this._cacheService = cacheService;
        }

        public async Task<IActionResult> Index(PaginationParam? param)
        {
            AllModelsVM model = new AllModelsVM();
            var cachTopCoursesResult = await _cacheService.GetAsync<List<CourseDetailViewModel>>("top-courses");
            if (cachTopCoursesResult != null)
            {

                model.MaxEnrollmentCourses = cachTopCoursesResult;
            }
            else
            {

                var topCourses = await _courseService.GetCoursesWIthMaxEnrollments();
                model.MaxEnrollmentCourses = topCourses;

                // Add to cache for 30 minutes (or your preferred duration)
                await _cacheService.SetAsync("top-courses", topCourses, TimeSpan.FromMinutes(30));
            }
            var cachTopDepartmentsResult = await _cacheService.GetAsync<List<Department>>("top-departments");
            if (cachTopDepartmentsResult != null)
            {
                model.Departments = cachTopDepartmentsResult;
            }
            else
            {
                var topDepartments = await _unitOfWork.departmentRepository.GetTopDepartments();
                model.Departments = topDepartments.ToList();
                // Add to cache for 30 minutes (or your preferred duration)
                await _cacheService.SetAsync("top-departments", topDepartments.ToList(), TimeSpan.FromMinutes(30));
            }
            var cachTopTracksResult = await _cacheService.GetAsync<List<TrackDetailsVIewModel>>("top-tracks-v2");

            if (cachTopTracksResult != null)
            {
                model.Tracks = cachTopTracksResult;
            }
            else
            {
                var topTracks = await _unitOfWork.trackRepository.GetTopTracksAsync();
                model.Tracks = topTracks;
                // Add to cache for 30 minutes (or your preferred duration)
                await _cacheService.SetAsync("top-tracks", topTracks, TimeSpan.FromMinutes(30));
            }


            model.Feedbacks = await _unitOfWork.feedbackRepository.GetAllAsync(null, null, true, e => e.Course, e => e.ProviderUser);
            model.Events = await _unitOfWork.eventRepository.GetAllAsync(null, null, true, e => e.CreatedBy, e => e.Department);
            model.Enrollments = await _unitOfWork.enrollmentRepository.GetAllAsync();
            if (param != null)
            {
                model.PagedCourses = await _unitOfWork.courseRepository.GetListOfCoursesDetailsWithPagination(param: param);

            }
            else
            {
                model.PagedCourses = await _unitOfWork.courseRepository.GetListOfCoursesDetailsWithPagination();

            }




            if (User.IsInRole(StaticData.role_Student))
            {

                var recommendationCourses = await _courseService.GetRecommendationCoursesforSpacifcStudent(_userManager.GetUserId(User));
                model.RecommendationCourses = recommendationCourses;
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
        public async Task<IActionResult> GetByDepTId(int? id, PaginationParam? param)
        {
            var model = new CoursesByDepartmentVM();
            var courses = id == null || id == 0 ? await _unitOfWork.courseRepository.GetListOfCoursesDetailsWithPagination(param: param) : await _unitOfWork.courseRepository.GetListOfCoursesDetailsWithPagination(
                    e => e.DepartmentId == id, param);

            model.Courses = courses;

            if (id != null && id != 0)
            {
                var department = _unitOfWork.departmentRepository.GetOne(e => e.DepartmentID == id);
                if (department != null)
                {
                    model.DepartmentName = department.DepartmentName;
                    model.DepartmentDescription = department.DepartmentDescription;
                }
            }

            return PartialView("_partialCoursesbyDeoartment", model);
        }


        public async Task<IActionResult> Details(int id,int? starFilter)
        {
            var course = _unitOfWork.courseRepository.GetCourseCurriculum(id);
            var studentId = _userManager.GetUserId(User);
            var model = new DetailsVM()
            {
                Course = course,
                RelatedCourses = await _courseService.GetRelatedCourses(id, studentId),
                InstructorCourses = await _unitOfWork.courseRepository
                .Get(e => e.InstructorId == course.InstructorId && e.CourseID != course.CourseId).PrepareCoursesToViewDetailAsync().ToListAsync(),
                 Feedbacks=await _courseService.GetCourseFeedbackAndReviews(id,starFilter)
            };
            ViewBag.star=starFilter;
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

        public async Task<IActionResult> Courses(CourseParam param, PaginationParam? pageParam)
        {

            var model = new CoursePageVM();

            // Fetch all departments for filter sidebar
            model.DepartmentList = await _unitOfWork.departmentRepository.Get().ToListAsync();

            // Filter + Project + Paginate
            var filteredCourses = _unitOfWork.courseRepository.GetFilteredCourses(param);
            var projectedCourses = await _unitOfWork.courseRepository.GetListOfCoursesDetailsWithPagination(param: pageParam);

            model.PagedCourseList = projectedCourses;



            ViewBag.Filters = param;

            return View(model);
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
            if (string.IsNullOrWhiteSpace(word))
            {
                return RedirectToAction("Index");
            }
            var wordToLowwer = word.ToLower();
            var ides = _unitOfWork.keyWordRepository
                .Get(e => e.Name.ToLower().Contains(wordToLowwer)).Select(e => e.CourseId);
            var listOfCourses = _unitOfWork.courseRepository
                .Get(e => ides.Contains(e.CourseID) || e.CourseName.ToLower().Contains(wordToLowwer)).PrepareCoursesToViewDetailAsync().ToList();
            ViewBag.word = word;
            return View(listOfCourses);
        }


        public IActionResult Filter(string? topic)
        {
            var courses = _unitOfWork.keyWordRepository.Get(e => e.Name.Contains(topic), e => e.Course).ToList();
            return View(courses);
        }

        public async Task<IActionResult> TrackDetails(int id)
        {
            var studentId = User.IsInRole(StaticData.role_Student) ? _userManager.GetUserId(User) : null;

            var track = studentId == null ? await _unitOfWork.trackRepository.TrackDetailsProjection(trackId: id) :
                await _unitOfWork.trackRepository.TrackDetailsProjection(trackId: id, studentId: studentId);
            return View(track);
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
                        ModelState.AddModelError("glb", error.Description);
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
