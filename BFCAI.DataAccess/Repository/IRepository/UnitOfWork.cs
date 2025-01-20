using Banha_UniverCity.Data;
using Banha_UniverCity.Repository.ModelsRepository;
using System;

namespace Banha_UniverCity.Repository.IRepository
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ApplicationDbContext context;

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
            courseRepository=new CourseRepository(context);
            curriculumRepository=new CourseCurriculumRepository(context);
            courseVideoRepository=new CourseVideoRepository(context);
            departmentRepository=new DepartmentRepository(context);
            enrollmentRepository=new EnrollmentRepository(context);
            classSchedulere=new ClassScheduleRepository(context);
            academicYear=new AcademicYearRepository(context);
            feedbackRepository=new FeedbackRepository(context);
            eventRepository=new EventRepository(context);
            roomRepository=new RoomRepository(context);
            
            
        }
        public ICourseRepository courseRepository { get; set; }
        public ICourseCurriculumRepository curriculumRepository { get; set; }
        public ICourseVideoRepository courseVideoRepository { get; set; }
        public IDepartmentRepository departmentRepository { get; set; }
        public IEnrollmentRepository enrollmentRepository { get; set; }
        public IAcademicYearRepository academicYear { get; set; }
       
        public IFeedbackRepository feedbackRepository { get; set; }
        public IEventRepository eventRepository { get; set; }
        public IRoomRepository roomRepository { get; set; }

        public IClassScheduleRepository classSchedulere { get; set; }

        public void Commit() { context.SaveChanges(); }
    }
}
