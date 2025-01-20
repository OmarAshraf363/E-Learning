using Banha_UniverCity.Data;
using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;

namespace Banha_UniverCity.Repository.ModelsRepository
{
    public class CourseVideoRepository : GenralRepository<CourseVideo>, ICourseVideoRepository
    {
        public CourseVideoRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
