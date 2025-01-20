using Banha_UniverCity.Repository.ModelsRepository;

namespace Banha_UniverCity.Repository.IRepository
{
    public interface IUnitOfWork
    {
        public ICourseRepository courseRepository { get; }
        public ICourseCurriculumRepository curriculumRepository { get; }
        public ICourseVideoRepository courseVideoRepository { get; }
        public IDepartmentRepository departmentRepository { get; }
        public IEnrollmentRepository enrollmentRepository { get; }
        public IAcademicYearRepository academicYear {  get; }
        public IClassScheduleRepository classSchedulere { get; }
        public IFeedbackRepository feedbackRepository { get; }
        public IEventRepository eventRepository { get; }
        public IRoomRepository roomRepository { get; }
        public void Commit();
    }
}
