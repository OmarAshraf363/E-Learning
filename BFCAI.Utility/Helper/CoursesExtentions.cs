using Banha_UniverCity.Models;
using BFCAI.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Utility.Helper
{
    public static class CoursesExtentions
    {
        public static   IQueryable<CourseDetailViewModel>PrepareCoursesToViewDetailAsync(this IQueryable<Course> courses)
        {
            return courses
                .Select(e => new CourseDetailViewModel
                {
                    CourseID = e.CourseID,
                    Image = e.ImgCover,
                    Title = e.CourseName,
                    InstructorName = e.Instructor.FullName,
                    Rate = e.Rate,
                    Price = e.Price,
                    Description = e.Description,
                    NumOfEnrolments = e.Enrollments.Count(),
                    VideosCount = e.CourseVideos.Count(),
                    LearningObjectives = e.LearningObjectives.Select(lo => lo.Objective).ToList()

                });
                
        }
    }
}
