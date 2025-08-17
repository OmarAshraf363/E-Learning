using Banha_UniverCity.Data;
using Banha_UniverCity.Models;
using DataAccess.Repository.IRepository;
using DataAccess.Repository.ModelsRepository;

namespace Banha_UniverCity.Repository.ModelsRepository
{
    public class EnrollmentRepository : GenralRepository<Enrollment>, IEnrollmentRepository
    {
        private readonly ApplicationDbContext _context;
        public EnrollmentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Course>> GetStudentCourses(string studentId)
        {
            var userEnrollments = await GetAllAsync(e => e.StudentId == studentId, includes: e => e.Course);
            var courses = userEnrollments.Select(e => e.Course).ToList();

            return courses;
        }
        public async Task EnrollUserInCourses(string studentId, List<int?> courseIds)
        {
            foreach (var courseId in courseIds)
            {
                var enrollment = new Enrollment
                {
                    StudentId = studentId,
                    CourseID = courseId
                };
                await AddAsync(enrollment);
            }
        }
    }
}
