using DataAccess.Repository.IRepository;

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
        public IExamRepository examRepository {  get; }
        public IQusetionRepository qusetionRepository { get; }
        public IChoicesRepository choicesRepository {  get; }
        public IExamSubmitionsRepository examSubmitionsRepository { get; }
        public ICourseResourceRepository courseResourceRepository { get; }
        public IAttendanceRepository attendanceRepository {  get; }
        public IAssinmentRepository assinmentRepository {  get; }
        public IAssinmentSubmitionRepository assinmentSubmitionRepository {  get; }
        public ILearningObjectiveRepository learningObjectiveRepository { get; }
        public ITopicCoveresRepository topicCoveresRepository { get; }
        public IKeyWordRepository keyWordRepository { get; }
        public IOrderRepository orderRepository { get; }
        public IOrderItemRepository orderItemRepository { get; }
        public ICartRepository cartRepository { get; }
        public ICartItemRepository cartItemRepository { get; }
        public ITrackRepository trackRepository { get; }
        public IProgressRepository progressRepository { get; }
        public IPostRepository postRepository { get; }
        public ICommentRepository commentRepository { get; }
        public ICommunityRepository communityRepository { get; }
        public IReactionRepository reactionRepository { get; }
        public void Commit();
    }
}
