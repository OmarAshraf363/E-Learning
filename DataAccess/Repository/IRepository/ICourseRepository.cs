using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;

namespace DataAccess.Repository.IRepository
{
    public interface ICourseRepository : IGenralRepository<Course>
    {
        Course GetCourseCurriculum(int? id);
    }
}
