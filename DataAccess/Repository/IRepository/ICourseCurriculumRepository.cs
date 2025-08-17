using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;
using BFCAI.Models.ViewModels;

namespace DataAccess.Repository.IRepository
{
    public interface ICourseCurriculumRepository : IGenralRepository<CourseCurriculum>
    {
        CourseCurriculum GetCurriculumWithIncluded(int? id);
        Task<SUbCurrcula> GetCurriculumContentAsync(int id);

    }
}
