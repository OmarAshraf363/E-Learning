using Banha_UniverCity.Models;
using BFCAI.Models;
using BFCAI.Models.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Banha_UniverCity.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<CourseVideo> CourseVideos { get; set; }
        public DbSet<CourseCurriculum> CourseCurricula { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<QuestionChoice> QuestionChoices { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<ExamSubmission> ExamSubmissions { get; set; }
        public DbSet<CourseResource> CourseResources { get; set; }

        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<AssignmentSubmission> AssignmentSubmissions { get; set; }
        public DbSet<KeyWord> KeyWords { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItems> CartItems { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Community> Communities { get; set; }



        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.Keywords)
                .WithOne(e => e.Course)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);


            // تعريف العلاقة بين Course وInstructor
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Instructor)
                .WithMany(u => u.CoursesTaught)
                .HasForeignKey(c => c.InstructorId);

            // تعريف العلاقة بين Enrollment وStudent
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(u => u.Enrollments)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // تعريف العلاقة بين Course وEnrollment
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseID).OnDelete(DeleteBehavior.Cascade);



            // تعريف العلاقة بين Course وCourseVideo
            modelBuilder.Entity<CourseVideo>()
                .HasOne(cv => cv.Course)
                .WithMany(c => c.CourseVideos)
                .HasForeignKey(cv => cv.CourseID)
                            .OnDelete(DeleteBehavior.Cascade);


            // تعريف العلاقة بين Course وCourseCurriculum
            modelBuilder.Entity<CourseCurriculum>()
                .HasOne(cc => cc.Course)
                .WithMany(c => c.CourseCurricula)
                .HasForeignKey(cc => cc.CourseID)
                                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<CourseCurriculum>()
                .HasMany(e => e.Exams)
                .WithOne(e => e.CourseCurriculum)
                .HasForeignKey(e => e.CurriculumId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Feedback>()
       .HasOne(f => f.ProviderUser)
       .WithMany(u => u.FeedbacksGiven) // Add a collection for Feedbacks given by the user
       .HasForeignKey(f => f.ProviderUserId)
       .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete to avoid cycles

            // Define the relationship between Feedback and the student receiving the feedback
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.TargetStudentUser)
                .WithMany(u => u.FeedbacksReceived) // Add a collection for Feedbacks received by the student
                .HasForeignKey(f => f.TargetStudentUserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete to avoid cycles

            // Optionally define the relationship between Feedback and Course
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Course)
                .WithMany(c => c.Feedbacks)
                .HasForeignKey(f => f.CourseId);


            modelBuilder.Entity<ClassSchedule>()
             .HasOne(cs => cs.Course)
                .WithMany(c => c.ClassSchedules)
                 .HasForeignKey(cs => cs.CourseId)
                     .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Event>()
           .HasOne(e => e.Department)
           .WithMany(d => d.Events)
           .HasForeignKey(e => e.DepartmentID);

            // تحديد علاقة واحد إلى متعدد بين Event و ApplicationUser (CreatedBy)
            modelBuilder.Entity<Event>()
                .HasOne(e => e.CreatedBy)
                .WithMany(u => u.Events)
                .HasForeignKey(e => e.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Department>()
                .HasMany(e => e.Users)
                .WithOne(e => e.Department)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<ClassSchedule>()
          .HasOne(s => s.Course)
          .WithMany(c => c.ClassSchedules)
          .HasForeignKey(s => s.CourseId)
          .OnDelete(DeleteBehavior.Cascade); // Optional

            // Configure the relationship between Instructor and Schedule (One-to-Many)
            modelBuilder.Entity<ClassSchedule>()
                .HasOne(s => s.Instructor)
                .WithMany(i => i.Schedules)
                .HasForeignKey(s => s.InstructorId)
                .OnDelete(DeleteBehavior.Restrict); // Optional

            // Configure the relationship between Room and Schedule (One-to-Many)
            modelBuilder.Entity<ClassSchedule>()
                .HasOne(s => s.Room)
                .WithMany(r => r.Schedules)
                .HasForeignKey(s => s.RoomId)
                .OnDelete(DeleteBehavior.Restrict); // Optional

            modelBuilder.Entity<AcademicYear>()
                .HasMany(e => e.ClassSchedules)
                .WithOne(e => e.AcadmicYear)
                .HasForeignKey(e => e.AcadmicYearId).OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Department>()
                .HasMany(e => e.ClassSchedules)
                .WithOne(e => e.Department)
                .HasForeignKey(e => e.DepartmentId).OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<Exam>()
         .HasOne(e => e.Instructor)
         .WithMany(u => u.Exams)
         .HasForeignKey(e => e.InstructorId)
         .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Exam>()
           .HasMany(e => e.Questions)
           .WithOne(q => q.Exam)
           .HasForeignKey(q => q.ExamID)
           .OnDelete(DeleteBehavior.Cascade);



            modelBuilder.Entity<Question>()
          .HasMany(q => q.Choices)
          .WithOne(o => o.Question)
          .HasForeignKey(o => o.QuestionID)
          .OnDelete(DeleteBehavior.Cascade);




            // علاقة ExamSubmission مع AppUser (Student)
            modelBuilder.Entity<ExamSubmission>()
                .HasOne(es => es.Student)
                .WithMany(u => u.ExamSubmissions)
                .HasForeignKey(es => es.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // علاقة ExamSubmission مع Exam
            modelBuilder.Entity<ExamSubmission>()
                .HasOne(es => es.Exam)
                .WithMany(e => e.ExamSubmissions)
                .HasForeignKey(es => es.ExamID)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<CourseCurriculum>()
                .HasMany(e => e.CourseVideos)
                .WithOne(e => e.CourseCurriculum)
                .HasForeignKey(e => e.CourseCurriculumID)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CourseResource>()
                .HasOne(e => e.CourseCurriculum)
                .WithMany(e => e.CourseResources)
                .HasForeignKey(e => e.CourseCurriculumID)
                 .OnDelete(DeleteBehavior.NoAction);



            modelBuilder.Entity<Assignment>()
          .HasOne(a => a.CourseCurriculum)
          .WithMany(c => c.Assignments)
          .HasForeignKey(a => a.CourseCurriculumID).OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<AssignmentSubmission>()
                .HasOne(a => a.Assignment)
                .WithMany(a => a.Submissions)
                .HasForeignKey(a => a.AssignmentID).OnDelete(DeleteBehavior.Cascade);



            modelBuilder.Entity<AssignmentSubmission>()
                .HasOne(a => a.ApplicationUser)
                .WithMany(u => u.AssignmentSubmissions)
                .HasForeignKey(a => a.ApplicationUserID)
                                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Attendance>()
            .HasOne(a => a.ApplicationUser)
            .WithMany(u => u.Attendances)
            .HasForeignKey(a => a.ApplicationUserID)
                            .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.CourseCurriculum)
                .WithMany(c => c.Attendances)
                .HasForeignKey(a => a.CourseCurriculumID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.LearningObjectives)
                .WithOne(e => e.Course)
                .HasForeignKey(e => e.CourseID)
          .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Course>()
               .HasMany(e => e.TopicsCovered)
               .WithOne(e => e.Course)
               .HasForeignKey(e => e.CourseID)
          .OnDelete(DeleteBehavior.Cascade);


            // العلاقة بين Cart و ApplicationUser (سلة التسوق والمستخدم)
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.ApplicationUser)
                .WithMany(u => u.Carts)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict); // لمنع الحذف التتابعي

            // العلاقة بين Cart و CartItem (عناصر سلة التسوق)
            modelBuilder.Entity<Cart>()
                .HasMany(c => c.Items)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade); // عند حذف سلة التسوق، يتم حذف العناصر

            // العلاقة بين CartItem و Course (العنصر في السلة والمنتج)
            modelBuilder.Entity<CartItems>()
                .HasOne(ci => ci.Course)
                .WithMany()
                .HasForeignKey(ci => ci.CourseId)
                .OnDelete(DeleteBehavior.Restrict); // لمنع الحذف التتابعي للكورس

            // العلاقة بين Order و ApplicationUser (الطلب والمستخدم)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.ApplicationUser)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.AppUserId)
                .OnDelete(DeleteBehavior.Restrict); // لمنع الحذف التتابعي

            // العلاقة بين Order و OrderItem (الطلب وعناصر الطلب)
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // عند حذف الطلب، يتم حذف عناصر الطلب

            // العلاقة بين OrderItem و Course (العنصر في الطلب والكورس)
            modelBuilder.Entity<OrderItems>()
                .HasOne(oi => oi.Course)
                .WithMany()
                .HasForeignKey(oi => oi.CourseId)
                .OnDelete(DeleteBehavior.Restrict); // لمنع الحذف التتابعي للكورس


            modelBuilder.Entity<Post>()
           .HasMany(p => p.Comments)
           .WithOne(c => c.Post)
           .HasForeignKey(c => c.PostId)
           .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Post>()
                .HasMany(p=>p.KeyWords)
                .WithOne(c => c.Post) .HasForeignKey(c => c.PostId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Post>()
                .HasMany(p => p.Reactions)
                .WithOne(e=>e.Post)
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.NoAction);
              

            // Comment Configuration
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId);

            modelBuilder.Entity<Comment>()
                .HasMany(c => c.Reactions)
                .WithOne(e=>e.Comment)
                .HasForeignKey(r => r.CommentId)
                .OnDelete(DeleteBehavior.NoAction);
    
            // Reaction Configuration
            modelBuilder.Entity<Reaction>()
                .Property(r => r.Type)
                .IsRequired();

      











            //seeding

            modelBuilder.Entity<Department>().HasData(
                   new Department { DepartmentID = 1, DepartmentName = "Computer Science" },
                   new Department { DepartmentID = 2, DepartmentName = "Electrical Engineering" }
               );

            modelBuilder.Entity<ApplicationUser>().HasData(
                  new ApplicationUser { Id = "1", UserName = "student1", NormalizedUserName = "STUDENT1", Email = "student1@example.com", NormalizedEmail = "STUDENT1@EXAMPLE.COM", FullName = "Student One", UserType = "Student" },
                new ApplicationUser { Id = "2", UserName = "instructor1", NormalizedUserName = "INSTRUCTOR1", Email = "instructor1@example.com", NormalizedEmail = "INSTRUCTOR1@EXAMPLE.COM", FullName = "Instructor One", UserType = "Instructor" },
 new ApplicationUser
 {
     Id = "123548458", // User ID should be a string
     UserName = "Admin@gmail.com",
     Email = "Admin@gmail.com",
     NormalizedUserName = "ADMIN@GMAIL.COM",
     NormalizedEmail = "ADMIN@GMAIL.COM",
     EmailConfirmed = false,
     PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "Admin@123"),
     SecurityStamp = Guid.NewGuid().ToString(), // Ensure this is unique
     FullName = "Admin",
     UserType = StaticData.role_Admin,
 }

);

            modelBuilder.Entity<Course>().HasData(
                new Course { CourseID = 1, CourseName = "Introduction to Programming", DepartmentId = 1, InstructorId = "2" },
                new Course { CourseID = 2, CourseName = "Digital Circuits", DepartmentId = 2, InstructorId = "2" }
            );

            modelBuilder.Entity<Enrollment>().HasData(
                new Enrollment { EnrollmentID = 1, StudentId = "1", CourseID = 1 },
                new Enrollment { EnrollmentID = 2, StudentId = "1", CourseID = 2 }
            );

            modelBuilder.Entity<CourseVideo>().HasData(
                new CourseVideo { CourseVideoID = 1, CourseID = 1, VideoTitle = "Course Overview", VideoURL = "http://example.com/video1" },
                new CourseVideo { CourseVideoID = 2, CourseID = 1, VideoTitle = "Getting Started", VideoURL = "http://example.com/video2" }
            );

            modelBuilder.Entity<CourseCurriculum>().HasData(
                new CourseCurriculum { CourseCurriculumID = 1, CourseID = 1, Title = "Introduction", Content = "Intro Of Cs" },
                new CourseCurriculum { CourseCurriculumID = 2, CourseID = 2, Title = "Basic Programming Concepts", Content = "Intro Of C++" }
            );



            modelBuilder.Entity<Feedback>().HasData(
                new Feedback { FeedbackID = 1, ProviderUserId = "1", TargetStudentUserId = "1", CourseId = 1, Content = "Great course!", FeedbackDate = DateTime.Now, },
                new Feedback { FeedbackID = 2, ProviderUserId = "2", TargetStudentUserId = "1", CourseId = 2, Content = "Need improvement on some topics.", FeedbackDate = DateTime.Now }
            );

            modelBuilder.Entity<Room>().HasData(
      new Room { RoomID = 1, Name = "Room 101", Capacity = 30 },
      new Room { RoomID = 2, Name = "Lab 202", Capacity = 20 }
  );
                        // Seed Data for Schedule
            modelBuilder.Entity<ClassSchedule>().HasData(
                new ClassSchedule { ClassScheduleId = 1, CourseId = 1, InstructorId = "1", RoomId = 1, DayOfWeek = "Monday", StartTime = new DateTime(2024, 10, 1, 10, 0, 0), EndTime = new DateTime(2024, 10, 1, 10, 0, 0) },
                new ClassSchedule { ClassScheduleId = 2, CourseId = 2, InstructorId = "2", RoomId = 2, DayOfWeek = "Tuesday", StartTime = new DateTime(2024, 10, 1, 10, 2, 0), EndTime = new DateTime(2024, 10, 1, 10, 2, 0) }
            );
            modelBuilder.Entity<AcademicYear>().HasData(
                new AcademicYear() { AcademicYearID = 1, YearName = "2020-2021" },
                new AcademicYear() { AcademicYearID = 2, YearName = "2021-2022" },
                new AcademicYear() { AcademicYearID = 3, YearName = "2022-2023" },
                new AcademicYear() { AcademicYearID = 4, YearName = "2023-2024" }

                );

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "1", // Role ID should be a string
                    Name = StaticData.role_Admin,
                    NormalizedName = StaticData.role_Admin.ToUpper()
                },
                new IdentityRole
                {
                    Id = "2", // Role ID should be a string
                    Name = StaticData.role_Instructor,
                    NormalizedName = StaticData.role_Instructor.ToUpper()
                },
                 new IdentityRole
                 {
                     Id = "3", // Role ID should be a string
                     Name = StaticData.role_Student,
                     NormalizedName = StaticData.role_Student.ToUpper()
                 }
            );
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
              new IdentityUserRole<string>
              {
                  RoleId = "1", // Role ID as string
                  UserId = "123548458" // User ID as string
              }
          );




            // بيانات مبدئية لـ Exams
            modelBuilder.Entity<Exam>().HasData(
                new Exam { ExamID = 1, Title = "Math Final Exam", ExamDate = new DateTime(2024, 12, 20), CourseID = 1, InstructorId = "1" },
                new Exam { ExamID = 2, Title = "Physics Final Exam", ExamDate = new DateTime(2024, 12, 21), CourseID = 2, InstructorId = "2" }
            );

            // بيانات مبدئية لـ Questions
            modelBuilder.Entity<Question>().HasData(
                new Question { QuestionID = 1, QuestionText = "What is 2 + 2?", ExamID = 1 },
                new Question { QuestionID = 2, QuestionText = "What is the speed of light?", ExamID = 2 }
            );

            // بيانات مبدئية لـ QuestionChoices
            modelBuilder.Entity<QuestionChoice>().HasData(
                new QuestionChoice { QuestionChoiceID = 1, ChoiceText = "4", IsCorrect = true, QuestionID = 1 },
                new QuestionChoice { QuestionChoiceID = 2, ChoiceText = "3", IsCorrect = false, QuestionID = 1 },
                new QuestionChoice { QuestionChoiceID = 3, ChoiceText = "299,792 km/s", IsCorrect = true, QuestionID = 2 },
                new QuestionChoice { QuestionChoiceID = 4, ChoiceText = "150,000 km/s", IsCorrect = false, QuestionID = 2 }
            );

            modelBuilder.Entity<Post>().HasData(
        new Post { Id = 1, Content = "Welcome to the B FCAI Platform Community!", UserId = "123548458", CreatedAt = DateTime.UtcNow },
        new Post { Id = 2, Content = "Here's a guide to using the platform effectively.", UserId = "123548458", CreatedAt = DateTime.UtcNow }
    );

            modelBuilder.Entity<Comment>().HasData(
                new Comment { Id = 1, PostId = 1, Content = "Thank you for this platform!", UserId = "123548458", CreatedAt = DateTime.UtcNow },
                new Comment { Id = 2, PostId = 1, Content = "Excited to be part of this community.", UserId = "123548458", CreatedAt = DateTime.UtcNow }
            );

            modelBuilder.Entity<Reaction>().HasData(
                new Reaction { Id = 1, Type = ReactionType.Like.ToString(), UserId = "123548458", PostId = 1 },
                new Reaction { Id = 2, Type = ReactionType.Love.ToString(), UserId = "123548458", PostId = 1,  }
            );
            modelBuilder.Entity<Community>().HasData(
                new Community { Id=1,Name="Web Development Community" , Description="All you need to know about Web Tech"}
                
                
                );

        }
    }
}


