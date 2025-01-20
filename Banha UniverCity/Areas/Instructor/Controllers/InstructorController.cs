using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;
using Banha_UniverCity.ViewModels;
using BFCAI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Banha_UniverCity.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    public class InstructorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public InstructorController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
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
        public IActionResult UpSert(int? id, int? deptId, int? trackId)
        {

            var listOfUsers = StaticData.GetUsers(_userManager);

            CourseVM model = new()
            {
                Departments = _unitOfWork.departmentRepository.Get().ToList(),
                Tracks = _unitOfWork.trackRepository.Get().ToList(),
                Users = listOfUsers.Where(e => e.UserType == StaticData.role_Instructor).ToList(),
                InstructorId=_userManager.GetUserId(User)
            };
            if (id == null || id == 0)
            {
                if (deptId != null)
                {
                    model.DepartmentId = deptId;
                    ViewBag.departmentName = _unitOfWork.departmentRepository.GetOne(e => e.DepartmentID == deptId)?.DepartmentName;

                }
                if (trackId != null)
                {
                    model.TrackId = trackId;
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
            model.TrackId = course.TrackID;
            model.Description = course.Description;
            model.EndDate = course.EndDate;
            model.StartDate = course.StartDate;
            model.Price = course.Price;
            model.NumOfEnrollments = course.NumOfEnrollments;
            
            model.DemoVideoUrl = course.DemoVideoUrl;
            model.TopicCovereds = course.TopicsCovered;
            model.ImgCover = course.ImgCover;
            model.Rate = course.Rate;
            model.LearningObjectives = course.LearningObjectives;
            model.TopicCovereds = _unitOfWork.topicCoveresRepository.Get(e => e.CourseID == id).ToList();
            model.KeyWords = _unitOfWork.keyWordRepository.Get(e => e.CourseId == id).ToList();
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
                        TrackID = model.TrackId,
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
                    _unitOfWork.keyWordRepository.AddCourseKeyWordsInKeyWordTable(model.KeyWords, course.CourseID, postId: null);

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
                    course.TrackID = model.TrackId;
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

                    var oldWords = _unitOfWork.keyWordRepository.Get(e => e.CourseId == course.CourseID);
                    _unitOfWork.keyWordRepository.DeleteRange(oldWords);
                    _unitOfWork.Commit();

                    //set topic
                    _unitOfWork.topicCoveresRepository.AddCourseTopicsInTopicTable(model.TopicCovereds, course.CourseID);


                    //set obJectives
                    _unitOfWork.learningObjectiveRepository.AddCourseOpjectivesInOpjectiveTable(model.LearningObjectives, course.CourseID);
                    //set new words
                    _unitOfWork.keyWordRepository.AddCourseKeyWordsInKeyWordTable(model.KeyWords, course.CourseID, postId: null);

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
                model.Tracks = _unitOfWork.trackRepository.Get().ToList();
                return View(model);
            }
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
            var courseVideo = new CourseVideo();

            if (id == null || id == 0)
            {
                courseVideo.CourseCurriculumID = curriculumId;
                courseVideo.CourseID = thisCourse.CourseID;
                return PartialView("_UpsertCourseVideo", courseVideo);
            }

            courseVideo = _unitOfWork.courseVideoRepository.GetOne(e => e.CourseVideoID == id && e.CourseCurriculumID == curriculumId && e.CourseID == thisCourse.CourseID);

            if (courseVideo == null)
            {
                return NotFound();
            }

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
