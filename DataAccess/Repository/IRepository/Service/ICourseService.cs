using Banha_UniverCity.Models;
using Banha_UniverCity.ViewModels;
using BFCAI.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository.Service
{
    public interface ICourseService
    {
        Task<List<CourseDetailViewModel>> GetRelatedCourses(int courseId, string? studentId);
        Task<CourseVM> PrepareCourseViewModelAsync(int? id, int? deptId, int? trackId, string instructorId);
        Task<IReadOnlyList<CourseDetailViewModel>> GetRecommendationCoursesforSpacifcStudent(string studentId);
        Task<IReadOnlyList<CourseDetailViewModel>> GetCoursesWIthMaxEnrollments();
        Task<(bool Success, string Message)> SaveCourseAsync(CourseVM model, IFormFile ImgCover);
        Task<CourseReviewSectionVM> GetCourseFeedbackAndReviews(int courseId, int? star = null);
        Task EditRatingCourseBasedOnReviews(int courseId);
    }
}
