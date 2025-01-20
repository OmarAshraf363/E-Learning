using Banha_UniverCity.Data;
using Banha_UniverCity.Models;
using DataAccess.Repository.IRepository;
using DataAccess.Repository.ModelsRepository;
using Microsoft.EntityFrameworkCore;

namespace Banha_UniverCity.Repository.ModelsRepository
{
    public class CourseCurriculumRepository : GenralRepository<CourseCurriculum>, ICourseCurriculumRepository
    {
        ApplicationDbContext _context;
        public CourseCurriculumRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public CourseCurriculum GetCurriculumWithIncluded(int? id)
        {
            var curriculum=_context.CourseCurricula.Where(e=>e.CourseCurriculumID == id)
                .Include(e=>e.Assignments).ThenInclude(e=>e.Submissions)
                .Include(e=>e.CourseVideos).Include(e=>e.CourseResources).FirstOrDefault();
            return curriculum;
        }
    }
}
