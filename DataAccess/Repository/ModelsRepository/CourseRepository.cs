using Banha_UniverCity.Data;
using Banha_UniverCity.Models;
using DataAccess.Repository.IRepository;
using DataAccess.Repository.ModelsRepository;
using Microsoft.EntityFrameworkCore;

namespace Banha_UniverCity.Repository.ModelsRepository
{
    public class CourseRepository : GenralRepository<Course>, ICourseRepository
    {
        ApplicationDbContext _context;
        public CourseRepository(ApplicationDbContext context) : base(context)
        {
            this._context=context;
        }

       public Course GetCourseCurriculum(int?id)
        {
            var course = _context.Courses.Where(e => e.CourseID == id)
                .Include(e=>e.TopicsCovered).Include(e=>e.LearningObjectives)
                .Include(e=>e.Enrollments).Include(e=>e.Instructor)
                .Include(e => e.CourseCurricula).ThenInclude(e => e.CourseResources).Include(e=>e.Feedbacks)
                .ThenInclude(e=>e.ProviderUser)
                 .Include(e => e.CourseCurricula).ThenInclude(e => e.Exams)
                .Include(e => e.CourseCurricula).ThenInclude(e => e.CourseVideos)
                .Include(e=>e.CourseCurricula).ThenInclude(e=>e.Assignments).ThenInclude(e=>e.Submissions)
                .FirstOrDefault();
            return course;
        }
    }
}
