using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;

namespace DataAccess.Repository.IRepository
{
    public interface IEnrollmentRepository : IGenralRepository<Enrollment>
    {
        Task<IReadOnlyList<Course>> GetStudentCourses(string studentId);
        Task EnrollUserInCourses(string studentId, List<int?> courseIds);
    }
}
