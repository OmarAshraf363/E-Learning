using Banha_UniverCity;
using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;
using Banha_UniverCity.ViewModels;
using BFCAI.Models.ViewModels;
using BFCAI.Utility.Helper;
using DataAccess.Repository.IRepository.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.ModelsRepository.Service
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IImageService _imageService;

        public CourseService(IUnitOfWork unitOfWork, IWebHostEnvironment env, IImageService imageService, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _env = env;
            _imageService = imageService;
            _userManager = userManager;
        }

        public async Task<List<CourseDetailViewModel>> GetRelatedCourses(int courseId, string? studentId)
        {
            var recommendedCourseIds = new List<int>();
            if (!string.IsNullOrEmpty(studentId))
            {
                var recomendationCOurses = await GetRecommendationCoursesforSpacifcStudent(studentId);
                recommendedCourseIds = recomendationCOurses.Select(c => c.CourseID).ToList();
            }
            var course = await _unitOfWork.courseRepository.GetOneAsync(e => e.CourseID == courseId);
            if (course == null)
                return new List<CourseDetailViewModel>();

            var relatedCoursesAndRecommendation = _unitOfWork.courseRepository
                .Get(e => e.CourseID != courseId && (e.TrackID == course.TrackID && e.DepartmentId == course.DepartmentId || recommendedCourseIds.Contains(e.CourseID)));

            var noRepeatedRelatedCourses = relatedCoursesAndRecommendation
                .GroupBy(e => e.CourseID)
                .Select(g => g.FirstOrDefault());

            var projectedRelatedCoureses = relatedCoursesAndRecommendation.PrepareCoursesToViewDetailAsync().ToList();



            return projectedRelatedCoureses;
        }
        public async Task<IReadOnlyList<CourseDetailViewModel>> GetRecommendationCoursesforSpacifcStudent(string studentId)
        {
            var studentCourses = await _unitOfWork.enrollmentRepository.GetStudentCourses(studentId);
            var coursesIdes = studentCourses.Select(e => e.CourseID).Distinct().ToList();
            var trackIDS = studentCourses.Select(c => c.TrackID).Distinct().ToList();
            var catIdes = studentCourses.Select(e => e.DepartmentId).Distinct().ToList();
            var keys = studentCourses.SelectMany(e => e.Keywords).Select(k => k.CourseId).Distinct().ToList();

            //courses from man tracks that the student is enrolled in, excluding courses taught by the same instructor

            var recommendedCourses = await _unitOfWork.courseRepository
                .GetListOfCoursesDetailsWithPagination
                (
                e => (trackIDS.Contains(e.TrackID) ||
                catIdes.Contains(e.DepartmentId) ||
               keys.Contains(e.CourseID)) &&
                (e.InstructorId != studentId && !coursesIdes.Contains(e.CourseID))
                );
            return recommendedCourses.Items;
        }
        public async Task<IReadOnlyList<CourseDetailViewModel>> GetCoursesWIthMaxEnrollments()
        {
            var courses = _unitOfWork.enrollmentRepository.Get()
                .GroupBy(c => c.CourseID)
                .Select(g => new { CourseId = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Take(5);
            var courseIds = courses.Select(c => c.CourseId).ToList();
            var listOfCoursesWitthMaxEnrollments = _unitOfWork.courseRepository.Get(e => courseIds.Contains(e.CourseID));



            return await _unitOfWork.courseRepository.ProjectCourses(listOfCoursesWitthMaxEnrollments).ToListAsync();

        }

        public async Task<CourseVM> PrepareCourseViewModelAsync(int? id, int? deptId, int? trackId, string instructorId)
        {
            var model = new CourseVM
            {
                Departments = _unitOfWork.departmentRepository.Get(e => e.Users.Any(x => x.Id == instructorId)).ToList(),
                Tracks = _unitOfWork.trackRepository.Get(e => e.Courses.Any(c => c.InstructorId == instructorId)).ToList(),
                Users = StaticData.GetUsers(_userManager).Where(u => u.UserType == StaticData.role_Instructor).ToList(),
                InstructorId = instructorId
            };

            if (id == null || id == 0)
            {
                model.DepartmentId = deptId ?? 0;
                model.TrackId = trackId ?? 0;
                return model;
            }

            var course = _unitOfWork.courseRepository.GetOne(e => e.CourseID == id);
            if (course == null) return null;

            model.CourseID = course.CourseID;
            model.CourseName = course.CourseName;
            model.Credits = course.Credits;
            model.Description = course.Description;
            model.EndDate = course.EndDate;
            model.StartDate = course.StartDate;
            model.Price = course.Price;
            model.NumOfEnrollments = course.NumOfEnrollments;
            model.DemoVideoUrl = course.DemoVideoUrl;
            model.ImgCover = course.ImgCover;
            model.Rate = course.Rate;
            model.DepartmentId = course.DepartmentId;
            model.TrackId = course.TrackID;
            model.TopicCovereds = _unitOfWork.topicCoveresRepository.Get(e => e.CourseID == id).ToList();
            model.KeyWords = _unitOfWork.keyWordRepository.Get(e => e.CourseId == id).ToList();
            model.LearningObjectives = _unitOfWork.learningObjectiveRepository.Get(e => e.CourseID == id).ToList();

            return model;
        }

        public async Task<(bool, string)> SaveCourseAsync(CourseVM model, IFormFile ImgCover)
        {
            try
            {


                // Upload image
                if (ImgCover != null)
                {
                    var result = await _imageService.UploadImageAsync(ImgCover, "Covers");
                    if (!result.Item1)
                    {
                        return (false, result.Item2);
                    }
                    model.ImgCover = result.Item2;
                }

                Course course;
                if (model.CourseID == 0)
                {
                    course = new Course
                    {
                        CourseName = model.CourseName,
                        Credits = model.Credits,

                        TrackID = model.TrackId,
                        Description = model.Description,
                        EndDate = model.EndDate,
                        StartDate = model.StartDate,
                        Price = model.Price,

                        ImgCover = model.ImgCover,
                        InstructorId = model.InstructorId,
                        DemoVideoUrl = model.DemoVideoUrl,
                        NumOfEnrollments = model.NumOfEnrollments,
                    };

                    _unitOfWork.courseRepository.Create(course);
                    _unitOfWork.Commit();

                    _unitOfWork.topicCoveresRepository.AddCourseTopicsInTopicTable(model.TopicCovereds, course.CourseID);
                    _unitOfWork.learningObjectiveRepository.AddCourseOpjectivesInOpjectiveTable(model.LearningObjectives, course.CourseID);
                    _unitOfWork.keyWordRepository.AddCourseKeyWordsInKeyWordTable(model.KeyWords, course.CourseID, postId: null);
                    _unitOfWork.Commit();
                }
                else
                {
                    course = _unitOfWork.courseRepository.GetOne(e => e.CourseID == model.CourseID);
                    if (course == null) return (false, "Course not found");

                    // Replace old image
                    if (!string.IsNullOrEmpty(course.ImgCover) && ImgCover != null)
                    {
                        var result = await _imageService.DeleteImageAsync(course.ImgCover);
                        course.ImgCover = model.ImgCover;
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
                    course.DemoVideoUrl = model.DemoVideoUrl;
                    course.NumOfEnrollments = model.NumOfEnrollments;

                    _unitOfWork.courseRepository.Edit(course);
                    _unitOfWork.Commit();

                    _unitOfWork.topicCoveresRepository.DeleteRange(_unitOfWork.topicCoveresRepository.Get(e => e.CourseID == course.CourseID));
                    _unitOfWork.learningObjectiveRepository.DeleteRange(_unitOfWork.learningObjectiveRepository.Get(e => e.CourseID == course.CourseID));
                    _unitOfWork.keyWordRepository.DeleteRange(_unitOfWork.keyWordRepository.Get(e => e.CourseId == course.CourseID));
                    _unitOfWork.Commit();

                    _unitOfWork.topicCoveresRepository.AddCourseTopicsInTopicTable(model.TopicCovereds, course.CourseID);
                    _unitOfWork.learningObjectiveRepository.AddCourseOpjectivesInOpjectiveTable(model.LearningObjectives, course.CourseID);
                    _unitOfWork.keyWordRepository.AddCourseKeyWordsInKeyWordTable(model.KeyWords, course.CourseID, postId: null);
                    _unitOfWork.Commit();
                }

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<CourseReviewSectionVM> GetCourseFeedbackAndReviews(int courseId,int? star=null)
        {
            var feedbacks = _unitOfWork.feedbackRepository.Get(e => e.CourseId == courseId);
            var reviewVM = new ReviewVM
            {
                TotalReviews = feedbacks.Count(),
                TotalReviewWith5 = feedbacks.Where(e => e.Rating == 5).Count(),
                TotalReviewWith4 = feedbacks.Where(e => e.Rating == 4).Count(),
                TotalReviewWith3 = feedbacks.Where(e => e.Rating == 3).Count(),
                TotalReviewWith2 = feedbacks.Where(e => e.Rating == 2).Count(),
                TotalReviewWith1 = feedbacks.Where(e => e.Rating == 1).Count(),
                FinalRate = feedbacks.Any() ? (int)Math.Round(feedbacks.Average(f => f.Rating.Value)) : 0
            };
            if (star != null) {
            
               feedbacks=feedbacks.Where(e=>e.Rating == star.Value);
            }
            var feedbackVMs = await feedbacks
                        .OrderByDescending(f => f.FeedbackDate)
                            .Select(f => new FeedbackVM
                            {
                                Id = f.FeedbackID,
                                Content = f.Content,
                                CreatedAt = f.FeedbackDate,
                                Rating = f.Rating,
                                UserName = f.ProviderUser.FullName ?? "Unknown",
                                UserImageUrl = f.ProviderUser.Picture,
                            }).Take(3).ToListAsync();

            return new CourseReviewSectionVM
            {
                ReviewVM = reviewVM,
                FeedbackVM = feedbackVMs
            };
        }
        public async Task EditRatingCourseBasedOnReviews(int courseId)
        {
            var newRating=_unitOfWork.feedbackRepository.Get(e=>e.CourseId == courseId).Average(e=>e.Rating);
            var course = await _unitOfWork.courseRepository.GetOneAsync(r => r.CourseID == courseId,false);
            if (course != null) {
                course.Rate = (int)newRating;
                _unitOfWork.Commit();
            }
            else
            {
                return;
            }

        }

    }

}
