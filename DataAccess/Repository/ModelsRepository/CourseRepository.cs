using Banha_UniverCity.Data;
using Banha_UniverCity.Migrations;
using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;
using Banha_UniverCity.ViewModels;
using BFCAI.Models.ViewModels;
using BFCAI.Utility.Helper;
using BFCAI.Utility.Shared;
using DataAccess.Repository.IRepository;
using DataAccess.Repository.ModelsRepository;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Banha_UniverCity.Repository.ModelsRepository
{
    public class CourseRepository : GenralRepository<Course>, ICourseRepository
    {
        ApplicationDbContext _context;
        public CourseRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }


        public CourseDetailPageViewModel GetCourseCurriculum(int? id)
        {
            var course = _context.Courses.Where(e => e.CourseID == id).Select( e => new CourseDetailPageViewModel
            {
                DemoVideoUrl= _context.CourseCurricula.Where(c => c.CourseID == e.CourseID).Select(c => c.CourseVideos.FirstOrDefault().VideoURL).FirstOrDefault(),
                DemoTitle = _context.CourseCurricula.Where(c => c.CourseID == e.CourseID).Select(c => c.CourseVideos.FirstOrDefault().VideoTitle).FirstOrDefault(),
                CourseName = e.CourseName,
                 CourseId = e.CourseID,
                    DepartmentId = e.DepartmentId,
                    InstructorId = e.InstructorId,
                    InstructorBio = e.Instructor.JopDescription,
                    InstructorImage = e.Instructor.Picture,
                TrackId = e.TrackID,
                    CourseDescription= e.Description,
                    Rate= e.Rate,
                    NumOfFeedbacks= e.Feedbacks.Count(),
                    NumOfVideos= e.CourseVideos.Count(),
                    NumOfEnrollments= e.Enrollments.Count(),
                    InstructorName = e.Instructor.UserName,
                    Price = e.Price,
                    LearningObjectives= e.LearningObjectives.Select(c => c.Objective).ToList(),
                    TopicsCovered= e.TopicsCovered.Select(c => c.Topic).ToList()
            }).FirstOrDefault();

            return course;


        }
        public async Task<object> GetProcess(int id)
        {

            var course = await _context.Courses.Where(e => e.CourseID == id).AsNoTracking().Select(e => new CurriculumViewModel
            {
                Course = _context.Courses.Find(id),
                CourseCurricula = e.CourseCurricula,
                Feedbacks =_context.Feedback.Where(f=>f.CourseId==id).Include(e=>e.ProviderUser).ToList(),
            }).FirstOrDefaultAsync();
            if (course == null)
                return null;


            return course;
        }

        public IQueryable<CourseDetailViewModel> ProjectCourses(IQueryable<Course> source, Expression<Func<Course, bool>>? filter = null)
        {
            return filter == null ? source.PrepareCoursesToViewDetailAsync() : source.Where(filter).PrepareCoursesToViewDetailAsync();
        }
        public async Task<PagedListViewModel<CourseDetailViewModel>> GetListOfCoursesDetailsWithPagination(
             Expression<Func<Course, bool>>? filter = null,
             PaginationParam? param = null)
        {
            var courses = _context.Set<Course>().AsNoTracking();

            if (filter != null)
                courses = courses.Where(filter);
            if (param == null)
                param = new PaginationParam();

            var projected = ProjectCourses(courses);


            return await projected.ToPagedListAsync(param.Page, param.PageSize);
        }



        public async Task<CourseCurriculumViewModel> QuickAccessToSpacifcCourse(int id)
        {
            var course = await _context.Courses.Where(e => e.CourseID == id).
                AsNoTracking().Select(e => new CourseCurriculumViewModel
                {
                    CourseName = e.CourseName,
                    CourseId = e.CourseID,
                    DepartmentId = e.DepartmentId,
                    InstructorId = e.InstructorId,
                    TrackId = e.TrackID,
                    sUbCurrculas = e.CourseCurricula.Select(c => new SUbCurrcula
                    {
                        Title = c.Title,
                        Content = c.Content,
                        Assignments = c.Assignments.Select(a => new ASSVM
                        {
                            Id = a.AssignmentID,
                            Title = a.Title,
                            Url = a.Content
                        }),
                        Resources = c.CourseResources.Select(a => new RefVM
                        {
                            Title = a.ResourceTitle,
                            Url = a.ResourceURL
                        }),
                        Videos = c.CourseVideos.Select(a => new VideoVM
                        {
                            Title = a.VideoTitle,
                            Url = a.VideoURL
                        }),
                    }),

                }).FirstOrDefaultAsync();

            return course??new CourseCurriculumViewModel();


        }



        public IQueryable<Course> GetFilteredCourses(CourseParam param)
        {
            var query = _context.Courses.AsQueryable();

            if (!string.IsNullOrWhiteSpace(param.Search))
            {
                var search = param.Search.ToLower();
                query = query.Where(c => c.CourseName.ToLower().Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(param.InstructorId))
            {
                query = query.Where(c => c.InstructorId == param.InstructorId);
            }

            if (param.CategoryId > 0)
            {
                query = query.Where(c => c.DepartmentId == param.CategoryId);
            }

            if (param.Price > 0)
            {
                query = query.Where(c => c.Price <= param.Price);
            }

            // Sorting
            switch (param.SortBy?.ToLower())
            {
                case "price":
                    query = param.SortOrder == "desc"
                        ? query.OrderByDescending(c => c.Price)
                        : query.OrderBy(c => c.Price);
                    break;

                case "rate":
                    query = param.SortOrder == "desc"
                        ? query.OrderByDescending(c => c.Rate)
                        : query.OrderBy(c => c.Rate);
                    break;

                case "popularity":
                    query = param.SortOrder == "desc"
                        ? query.OrderByDescending(c => c.Enrollments.Count)
                        : query.OrderBy(c => c.Enrollments.Count);
                    break;

                default: // relevance or fallback
                    query = query.OrderByDescending(c => c.CourseID);
                    break;
            }

            return query;
        }


    }
}