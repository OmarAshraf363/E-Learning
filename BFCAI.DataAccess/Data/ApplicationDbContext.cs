using Banha_UniverCity.Models;
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
            // تعريف العلاقة بين Course وInstructor
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Instructor)
                .WithMany(u => u.CoursesTaught)
                .HasForeignKey(c => c.InstructorId);

            // تعريف العلاقة بين Enrollment وStudent
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(u => u.Enrollments)
                .HasForeignKey(e => e.StudentId);

            // تعريف العلاقة بين Course وEnrollment
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseID).OnDelete(DeleteBehavior.NoAction);
            ;

            // تعريف العلاقة بين Course وCourseVideo
            modelBuilder.Entity<CourseVideo>()
                .HasOne(cv => cv.Course)
                .WithMany(c => c.CourseVideos)
                .HasForeignKey(cv => cv.CourseID);

            // تعريف العلاقة بين Course وCourseCurriculum
            modelBuilder.Entity<CourseCurriculum>()
                .HasOne(cc => cc.Course)
                .WithMany(c => c.CourseCurricula)
                .HasForeignKey(cc => cc.CourseID);



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
                .HasMany(e=>e.ClassSchedules)
                .WithOne(e => e.Department)
                .HasForeignKey(e => e.DepartmentId).OnDelete(DeleteBehavior.Restrict);


            //seeding

            modelBuilder.Entity<Department>().HasData(
                   new Department { DepartmentID = 1, DepartmentName = "Computer Science" },
                   new Department { DepartmentID = 2, DepartmentName = "Electrical Engineering" }
               );

            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser { Id = "1", UserName = "student1", NormalizedUserName = "STUDENT1", Email = "student1@example.com", NormalizedEmail = "STUDENT1@EXAMPLE.COM", FullName = "Student One", UserType = "Student" },
                new ApplicationUser { Id = "2", UserName = "instructor1", NormalizedUserName = "INSTRUCTOR1", Email = "instructor1@example.com", NormalizedEmail = "INSTRUCTOR1@EXAMPLE.COM", FullName = "Instructor One", UserType = "Instructor" }
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
        }
    }
}

