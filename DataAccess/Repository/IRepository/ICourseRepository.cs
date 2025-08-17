using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;
using BFCAI.Models.ViewModels;
using BFCAI.Utility.Shared;
using System.Linq.Expressions;

namespace DataAccess.Repository.IRepository
{
    public interface ICourseRepository : IGenralRepository<Course>
    {
        CourseDetailPageViewModel GetCourseCurriculum(int? id);
        public  Task<object> GetProcess(int id);
        Task<CourseCurriculumViewModel> QuickAccessToSpacifcCourse(int id);
        IQueryable<Course> GetFilteredCourses(CourseParam param);
        IQueryable<CourseDetailViewModel> ProjectCourses(IQueryable<Course> source, Expression<Func<Course, bool>>? filter = null);
        Task<PagedListViewModel<CourseDetailViewModel>> GetListOfCoursesDetailsWithPagination(
       Expression<Func<Course, bool>>? filter = null,
       PaginationParam? param = null);
    }
}
