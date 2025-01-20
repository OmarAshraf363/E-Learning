using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;
using Banha_UniverCity.ViewModels;
using BFCAI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Stripe;

namespace Banha_UniverCity.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public CourseController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            ViewBag.mostPopularCourse = _unitOfWork.enrollmentRepository.Get()
                .GroupBy(e => e.CourseID)
                .OrderByDescending(e => e.Count())
                .Select(s => new
                {
                    Course = _unitOfWork.courseRepository.GetOne(e => e.CourseID == s.Key),
                    Count = s.Count()
                });

            return View(_unitOfWork.courseRepository.Get(null, e => e.Instructor, e => e.Department, e => e.Enrollments));

        }

        public IActionResult Details(int id) => View(_unitOfWork.courseRepository.GetOne(e => e.CourseID == id, e => e.Instructor, e => e.Department, e => e.Enrollments));

        [HttpGet]
        public IActionResult UpSert(int? id,int?deptId,int? trackId)
        {

            var listOfUsers = StaticData.GetUsers(_userManager);

            CourseVM model = new()
            {
                Departments = _unitOfWork.departmentRepository.Get().ToList(),
                Tracks=_unitOfWork.trackRepository.Get().ToList(),
                Users = listOfUsers.Where(e => e.UserType == StaticData.role_Instructor).ToList(),
            };
            if (id == null || id == 0)
            {
                if (deptId!=null)
                {
                    model.DepartmentId = deptId;
                    ViewBag.departmentName = _unitOfWork.departmentRepository.GetOne(e => e.DepartmentID == deptId)?.DepartmentName;

                }
                if (trackId!=null)
                {
                    model.TrackId=trackId;
                    ViewBag.trackName = _unitOfWork.trackRepository.GetOne(e => e.Id == trackId)?.Name;
                }
                return View(model);
            }
            var course = _unitOfWork.courseRepository.GetOne(e => e.CourseID == id);
            if (course == null) { return NotFound(); }
            model.CourseID = course.CourseID;
            model.CourseName = course.CourseName;
            model.Credits = course.Credits;
            model.DepartmentId = course.DepartmentId;
            model.TrackId=course.TrackID;
            model.Description = course.Description;
            model.EndDate = course.EndDate;
            model.StartDate = course.StartDate;
            model.Price = course.Price;
            model.NumOfEnrollments = course.NumOfEnrollments;
            model.InstructorId = course.InstructorId;
            model.DemoVideoUrl = course.DemoVideoUrl;
            model.TopicCovereds = course.TopicsCovered;
            model.ImgCover = course.ImgCover;
            model.Rate = course.Rate;
            model.LearningObjectives = course.LearningObjectives;
            model.TopicCovereds = _unitOfWork.topicCoveresRepository.Get(e => e.CourseID == id).ToList();
            model.KeyWords=_unitOfWork.keyWordRepository.Get(e => e.CourseId == id).ToList();
            model.LearningObjectives = _unitOfWork.learningObjectiveRepository.Get(e => e.CourseID == id).ToList();

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UpSert(CourseVM model, IFormFile ImgCover)
        {
            if (ModelState.IsValid)
            {


                if (ImgCover != null)
                {
                    // Define the path where the file will be saved
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Covers", ImgCover.FileName);

                    // Check if the file with the same name already exists
                    if (System.IO.File.Exists(filePath))
                    {
                        ModelState.AddModelError("ImgCover", "A file with the same name already exists.");
                    }
                    else
                    {
                        // Generate a unique file name
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + ImgCover.FileName;

                        // Define the new path where the file will be saved
                        var newFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Covers", uniqueFileName);

                        // Save the file
                        using (var stream = new FileStream(newFilePath, FileMode.Create))
                        {
                            await ImgCover.CopyToAsync(stream);  // Use ImgCover instead of Content
                        }

                        // Assign the unique file name to the model property
                        model.ImgCover = uniqueFileName;
                    }
                }
                else
                {
                    ModelState.AddModelError("ImgCover", "Please upload an image.");
                }

                Course course;
                if (model.CourseID == 0)
                {
                    course = new()
                    {
                        CourseName = model.CourseName,
                        Credits = model.Credits,
                        DepartmentId = model.DepartmentId,
                        TrackID=model.TrackId,
                        Description = model.Description,
                        EndDate = model.EndDate,
                        StartDate = model.StartDate,
                        Price = model.Price,
                        Rate = model.Rate,
                        ImgCover = model.ImgCover,
                        InstructorId = model.InstructorId,
                        DemoVideoUrl = model.DemoVideoUrl,
                        NumOfEnrollments = model.NumOfEnrollments,



                    };
                    _unitOfWork.courseRepository.Create(course);
                    _unitOfWork.Commit();


                    //set topic
                    _unitOfWork.topicCoveresRepository.AddCourseTopicsInTopicTable(model.TopicCovereds, course.CourseID);


                    //set obJectives
                    _unitOfWork.learningObjectiveRepository.AddCourseOpjectivesInOpjectiveTable(model.LearningObjectives, course.CourseID);
                    //set words
                    _unitOfWork.keyWordRepository.AddCourseKeyWordsInKeyWordTable(model.KeyWords, course.CourseID,postId:null);

                    _unitOfWork.Commit();

                    TempData["alert"] = "Added successfully";
                }
                else
                {
                    course = _unitOfWork.courseRepository.GetOne(e => e.CourseID == model.CourseID);
                    if (course == null) { return NotFound(); }

                    if (!string.IsNullOrEmpty(course.ImgCover) && ImgCover != null)
                    {
                        var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Covers", course.ImgCover);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            try
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                            catch (Exception ex)
                            {
                                ModelState.AddModelError("ImgCover", "An error occurred while deleting the old file.");
                                var errorResult = StaticData.CheckValidation(ModelState, Request, false);
                                return errorResult;
                            }
                        }
                    }


                    course.CourseName = model.CourseName;
                    course.Credits = model.Credits;
                    course.DepartmentId = model.DepartmentId;
                    course.TrackID=model.TrackId;
                    course.Description = model.Description;
                    course.EndDate = model.EndDate;
                    course.StartDate = model.StartDate;
                    course.Price = model.Price;
                    course.Rate = model.Rate;
                    course.ImgCover = model.ImgCover;
                    course.InstructorId = model.InstructorId;
                    course.DemoVideoUrl = model.DemoVideoUrl;
                    course.NumOfEnrollments = model.NumOfEnrollments;

                    _unitOfWork.courseRepository.Edit(course); ;
                    _unitOfWork.Commit();


                    //remove Olds
                    var oldTopics = _unitOfWork.topicCoveresRepository.Get(e => e.CourseID == course.CourseID);
                    _unitOfWork.topicCoveresRepository.DeleteRange(oldTopics);


                    var oldOpjectives = _unitOfWork.learningObjectiveRepository.Get(e => e.CourseID == course.CourseID);
                    _unitOfWork.learningObjectiveRepository.DeleteRange(oldOpjectives);
                    _unitOfWork.Commit();

                    var oldWords=_unitOfWork.keyWordRepository.Get(e => e.CourseId == course.CourseID);
                    _unitOfWork.keyWordRepository.DeleteRange(oldWords);
                    _unitOfWork.Commit();

                    //set topic
                    _unitOfWork.topicCoveresRepository.AddCourseTopicsInTopicTable(model.TopicCovereds, course.CourseID);


                    //set obJectives
                    _unitOfWork.learningObjectiveRepository.AddCourseOpjectivesInOpjectiveTable(model.LearningObjectives, course.CourseID);
                    //set new words
                    _unitOfWork.keyWordRepository.AddCourseKeyWordsInKeyWordTable(model.KeyWords, course.CourseID,postId:null);

                    _unitOfWork.Commit();
                    TempData["alert"] = "Edited successfully";

                }
                return RedirectToAction("Index");

            }
            else
            {
                model.Departments = _unitOfWork.departmentRepository.Get().ToList();
                var listOfUsers = StaticData.GetUsers(_userManager);
                model.Users = listOfUsers.Where(e => e.UserType == StaticData.role_Instructor);
                model.Tracks=_unitOfWork.trackRepository.Get().ToList();
                return View(model);
            }
        }


        public IActionResult Delete(int id)
        {
            var course = _unitOfWork.courseRepository.GetOne(e => e.CourseID == id);

            if (course != null)
            {
                if (course.ImgCover!=null)
                {

                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Covers", course.ImgCover);
                if (System.IO.File.Exists(oldFilePath))
                {

                    System.IO.File.Delete(oldFilePath);
                }
                }
                _unitOfWork.courseRepository.Delete(course);

                _unitOfWork.Commit();
                return RedirectToAction("Index");
            }
            else { return NotFound(); }
        }
    }
}
