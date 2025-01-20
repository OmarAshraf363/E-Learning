using Banha_UniverCity.Data;
using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;

namespace Banha_UniverCity.Repository.ModelsRepository
{
    public class CourseCurriculumRepository : GenralRepository<CourseCurriculum>, ICourseCurriculumRepository
    {
        public CourseCurriculumRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
