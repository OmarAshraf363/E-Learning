using Banha_UniverCity.Data;
using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;

namespace Banha_UniverCity.Repository.ModelsRepository
{
    public class CourseRepository : GenralRepository<Course>, ICourseRepository
    {
        public CourseRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
