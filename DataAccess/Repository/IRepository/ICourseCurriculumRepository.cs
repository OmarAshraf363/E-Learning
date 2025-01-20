using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;

namespace DataAccess.Repository.IRepository
{
    public interface ICourseCurriculumRepository : IGenralRepository<CourseCurriculum>
    {
        CourseCurriculum GetCurriculumWithIncluded(int? id);

    }
}
