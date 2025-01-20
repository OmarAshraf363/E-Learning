using Banha_UniverCity.Data;
using Banha_UniverCity.Repository.IRepository;
using Banha_UniverCity.Repository.ModelsRepository;
using DataAccess.Repository.IRepository;
using System;

namespace DataAccess.Repository.ModelsRepository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
            courseRepository = new CourseRepository(context);
            curriculumRepository = new CourseCurriculumRepository(context);
            courseVideoRepository = new CourseVideoRepository(context);
            departmentRepository = new DepartmentRepository(context);
            enrollmentRepository = new EnrollmentRepository(context);
            classSchedulere = new ClassScheduleRepository(context);
            academicYear = new AcademicYearRepository(context);
            feedbackRepository = new FeedbackRepository(context);
            eventRepository = new EventRepository(context);
            roomRepository = new RoomRepository(context);
            examRepository = new ExamRepository(context);
            qusetionRepository = new QusetionRepository(context);
            choicesRepository = new ChoicesRepository(context);
            examSubmitionsRepository = new ExamSubmitionsRepository(context);
            courseResourceRepository = new CourseResourceRepository(context);
            attendanceRepository = new AttendanceRepository(context);
            assinmentRepository = new AssinmentRepository(context);
            assinmentSubmitionRepository=new AssinmentSubmitionRepository(context);
            topicCoveresRepository = new TopicCoveresRepository(context);
            learningObjectiveRepository = new LearningObjectiveRepository(context);
            keyWordRepository = new KeyWordRepository(context);
            orderItemRepository = new OrderItemRepository(context);
            cartItemRepository = new CartItemRepository(context);
            cartRepository = new CartRepository(context);
            orderRepository = new OrderRepository(context);
            trackRepository = new TrackRepository(context);
            progressRepository = new ProgressRepository(context);
            postRepository = new PostRepository(context);
            commentRepository = new CommentRepository(context);
            communityRepository = new CommunityRepository(context);
            reactionRepository = new ReactionRepository(context);
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

        public IExamRepository examRepository { get; set; }
        public IQusetionRepository qusetionRepository { get; set; }
        public IChoicesRepository choicesRepository { get; set; }
        public IExamSubmitionsRepository examSubmitionsRepository { get; set; }
        public ICourseResourceRepository courseResourceRepository { get; set; }
        public IAttendanceRepository attendanceRepository { get; set; }
        public IAssinmentRepository assinmentRepository { get; set; }
        public IAssinmentSubmitionRepository assinmentSubmitionRepository { get; set; }
        public ILearningObjectiveRepository learningObjectiveRepository { get; set; }
        public ITopicCoveresRepository topicCoveresRepository { get; set; }
        public IKeyWordRepository keyWordRepository { get; set; }
        public IOrderRepository orderRepository { get; set; }
        public IOrderItemRepository orderItemRepository { get; set; }
        public ICartRepository cartRepository { get; set; }
        public ICartItemRepository cartItemRepository { get; set; }
        public ITrackRepository trackRepository { get; set; }

        public IProgressRepository progressRepository { get; set; }

        public IPostRepository postRepository { get; set; }
        public ICommentRepository commentRepository { get; set; }
        public ICommunityRepository communityRepository { get; set; }
        public IReactionRepository reactionRepository { get; set; }
        public void Commit() { context.SaveChanges(); }
    }
}
