using Banha_UniverCity.Data;
using Banha_UniverCity.Models;
using BFCAI.Models.ViewModels;
using DataAccess.Repository.IRepository;
using DataAccess.Repository.ModelsRepository;
using Microsoft.EntityFrameworkCore;

namespace Banha_UniverCity.Repository.ModelsRepository
{
    public class CourseCurriculumRepository : GenralRepository<CourseCurriculum>, ICourseCurriculumRepository
    {
       private readonly ApplicationDbContext _context;
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
        public async Task<SUbCurrcula> GetCurriculumContentAsync(int id) 
        {
            var result = await _context.CourseCurricula.Where(e => e.CourseCurriculumID == id)
                .Select(  e => new SUbCurrcula
                {
                    Id=e.CourseCurriculumID,
                    Title=e.Title,
                    Content=e.Content,
                    Videos=  _context.CourseVideos.Where(cv=>cv.CourseCurriculumID==id).Select(v=>new VideoVM
                    {
                        Id=v.CourseVideoID,
                        Url=v.VideoURL,
                        Title=v.VideoTitle
                    }).ToList(),
                    Assignments=_context.Assignments.Where(a=>a.CourseCurriculumID==id).Select(ass=>new ASSVM
                    {
                        Id=ass.AssignmentID,
                        Title=ass.Title,
                        Url=ass.Content,
                        DeadLine=ass.DeadLine,
                        Submissions=_context.AssignmentSubmissions.Where(s=>s.AssignmentID==ass.AssignmentID).ToList()
                    }).ToList(),
                    Resources=_context.CourseResources.Where(r=>r.CourseCurriculumID==id).Select(re=>new RefVM
                    {
                        Id=re.CourseResourceID,
                        Title=re.ResourceTitle,
                        Url=re.ResourceURL,
                    }).ToList(),
                    Feedbacks=_context.Feedback.Where(f=>f.CourseCurriculumId==id).Select(fe=>new FeedbackVM
                    {
                        Content=fe.Content,
                        CreatedAt=fe.FeedbackDate,
                        UserImageUrl=fe.ProviderUser.Picture,
                        UserName=fe.ProviderUser.UserName,
                        Id=fe.FeedbackID,
                        Rating=fe.Rating,
                    }).ToList()

                }).FirstOrDefaultAsync();
            return result;
        }
    }
}
