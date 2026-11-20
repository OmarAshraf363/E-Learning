using BFCAI.Domain.Constants;
using BFCAI.Domain.Entities;
using BFCAI.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BFCAI.Infrastructure.Persistence;

/// <summary>
/// Same seed data as the legacy ApplicationDbContext.
/// Technical fix: values that changed on every build (DateTime.Now, a freshly generated
/// password hash and security stamp) are now constants, so migrations stay stable.
/// Admin login: Admin@gmail.com / Admin@123 (unchanged).
/// </summary>
internal static class SeedData
{
    public const string AdminUserId = "123548458";

    // PBKDF2 (Identity V3) hash of "Admin@123" with a fixed salt.
    private const string AdminPasswordHash =
        "AQAAAAIAAYagAAAAEG8cKp5LfTxajg8bLD1OX2AmHNh542ZmSj2SnzSHO5OXdEm53LZGuzdhxu28ewcK0A==";

    private static readonly DateTime SeedDate = new(2024, 9, 1, 0, 0, 0, DateTimeKind.Utc);

    public static void Apply(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>().HasData(
            new Department { DepartmentID = 1, DepartmentName = "Computer Science" },
            new Department { DepartmentID = 2, DepartmentName = "Electrical Engineering" });

        modelBuilder.Entity<ApplicationUser>().HasData(
            new ApplicationUser
            {
                Id = "1", UserName = "student1", NormalizedUserName = "STUDENT1",
                Email = "student1@example.com", NormalizedEmail = "STUDENT1@EXAMPLE.COM",
                FullName = "Student One", UserType = "Student",
                SecurityStamp = "8b1f5d6e-0c4a-4f3e-9a51-2d7c6b3e4f01",
                ConcurrencyStamp = "c1a6f3e2-5b7d-4c8e-9f01-1a2b3c4d5e01",
            },
            new ApplicationUser
            {
                Id = "2", UserName = "instructor1", NormalizedUserName = "INSTRUCTOR1",
                Email = "instructor1@example.com", NormalizedEmail = "INSTRUCTOR1@EXAMPLE.COM",
                FullName = "Instructor One", UserType = "Instructor",
                SecurityStamp = "8b1f5d6e-0c4a-4f3e-9a51-2d7c6b3e4f02",
                ConcurrencyStamp = "c1a6f3e2-5b7d-4c8e-9f01-1a2b3c4d5e02",
            },
            new ApplicationUser
            {
                Id = AdminUserId,
                UserName = "Admin@gmail.com", NormalizedUserName = "ADMIN@GMAIL.COM",
                Email = "Admin@gmail.com", NormalizedEmail = "ADMIN@GMAIL.COM",
                EmailConfirmed = false,
                PasswordHash = AdminPasswordHash,
                SecurityStamp = "8b1f5d6e-0c4a-4f3e-9a51-2d7c6b3e4f03",
                ConcurrencyStamp = "c1a6f3e2-5b7d-4c8e-9f01-1a2b3c4d5e03",
                FullName = "Admin",
                UserType = Roles.Admin,
            });

        modelBuilder.Entity<Course>().HasData(
            new Course { CourseID = 1, CourseName = "Introduction to Programming", DepartmentId = 1, InstructorId = "2" },
            new Course { CourseID = 2, CourseName = "Digital Circuits", DepartmentId = 2, InstructorId = "2" });

        modelBuilder.Entity<Enrollment>().HasData(
            new Enrollment { EnrollmentID = 1, StudentId = "1", CourseID = 1 },
            new Enrollment { EnrollmentID = 2, StudentId = "1", CourseID = 2 });

        modelBuilder.Entity<CourseVideo>().HasData(
            new CourseVideo { CourseVideoID = 1, CourseID = 1, VideoTitle = "Course Overview", VideoURL = "http://example.com/video1" },
            new CourseVideo { CourseVideoID = 2, CourseID = 1, VideoTitle = "Getting Started", VideoURL = "http://example.com/video2" });

        modelBuilder.Entity<CourseCurriculum>().HasData(
            new CourseCurriculum { CourseCurriculumID = 1, CourseID = 1, Title = "Introduction", Content = "Intro Of Cs" },
            new CourseCurriculum { CourseCurriculumID = 2, CourseID = 2, Title = "Basic Programming Concepts", Content = "Intro Of C++" });

        modelBuilder.Entity<Feedback>().HasData(
            new Feedback { FeedbackID = 1, ProviderUserId = "1", TargetStudentUserId = "1", CourseId = 1, Content = "Great course!", FeedbackDate = SeedDate },
            new Feedback { FeedbackID = 2, ProviderUserId = "2", TargetStudentUserId = "1", CourseId = 2, Content = "Need improvement on some topics.", FeedbackDate = SeedDate });

        modelBuilder.Entity<Room>().HasData(
            new Room { RoomID = 1, Name = "Room 101", Capacity = 30 },
            new Room { RoomID = 2, Name = "Lab 202", Capacity = 20 });

        modelBuilder.Entity<ClassSchedule>().HasData(
            new ClassSchedule { ClassScheduleId = 1, CourseId = 1, InstructorId = "1", RoomId = 1, DayOfWeek = "Monday", StartTime = new DateTime(2024, 10, 1, 10, 0, 0), EndTime = new DateTime(2024, 10, 1, 10, 0, 0) },
            new ClassSchedule { ClassScheduleId = 2, CourseId = 2, InstructorId = "2", RoomId = 2, DayOfWeek = "Tuesday", StartTime = new DateTime(2024, 10, 1, 10, 2, 0), EndTime = new DateTime(2024, 10, 1, 10, 2, 0) });

        modelBuilder.Entity<AcademicYear>().HasData(
            new AcademicYear { AcademicYearID = 1, YearName = "2020-2021" },
            new AcademicYear { AcademicYearID = 2, YearName = "2021-2022" },
            new AcademicYear { AcademicYearID = 3, YearName = "2022-2023" },
            new AcademicYear { AcademicYearID = 4, YearName = "2023-2024" });

        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = "1", Name = Roles.Admin, NormalizedName = Roles.Admin.ToUpperInvariant(), ConcurrencyStamp = "role-admin" },
            new IdentityRole { Id = "2", Name = Roles.Instructor, NormalizedName = Roles.Instructor.ToUpperInvariant(), ConcurrencyStamp = "role-instructor" },
            new IdentityRole { Id = "3", Name = Roles.Student, NormalizedName = Roles.Student.ToUpperInvariant(), ConcurrencyStamp = "role-student" });

        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string> { RoleId = "1", UserId = AdminUserId });

        modelBuilder.Entity<Exam>().HasData(
            new Exam { ExamID = 1, Title = "Math Final Exam", ExamDate = new DateTime(2024, 12, 20), CourseID = 1, InstructorId = "1" },
            new Exam { ExamID = 2, Title = "Physics Final Exam", ExamDate = new DateTime(2024, 12, 21), CourseID = 2, InstructorId = "2" });

        modelBuilder.Entity<Question>().HasData(
            new Question { QuestionID = 1, QuestionText = "What is 2 + 2?", ExamID = 1 },
            new Question { QuestionID = 2, QuestionText = "What is the speed of light?", ExamID = 2 });

        modelBuilder.Entity<QuestionChoice>().HasData(
            new QuestionChoice { QuestionChoiceID = 1, ChoiceText = "4", IsCorrect = true, QuestionID = 1 },
            new QuestionChoice { QuestionChoiceID = 2, ChoiceText = "3", IsCorrect = false, QuestionID = 1 },
            new QuestionChoice { QuestionChoiceID = 3, ChoiceText = "299,792 km/s", IsCorrect = true, QuestionID = 2 },
            new QuestionChoice { QuestionChoiceID = 4, ChoiceText = "150,000 km/s", IsCorrect = false, QuestionID = 2 });

        modelBuilder.Entity<Post>().HasData(
            new Post { Id = 1, Content = "Welcome to the B FCAI Platform Community!", UserId = AdminUserId, CreatedAt = SeedDate },
            new Post { Id = 2, Content = "Here's a guide to using the platform effectively.", UserId = AdminUserId, CreatedAt = SeedDate });

        modelBuilder.Entity<Comment>().HasData(
            new Comment { Id = 1, PostId = 1, Content = "Thank you for this platform!", UserId = AdminUserId, CreatedAt = SeedDate },
            new Comment { Id = 2, PostId = 1, Content = "Excited to be part of this community.", UserId = AdminUserId, CreatedAt = SeedDate });

        modelBuilder.Entity<Reaction>().HasData(
            new Reaction { Id = 1, Type = ReactionType.Like.ToString(), UserId = AdminUserId, PostId = 1 },
            new Reaction { Id = 2, Type = ReactionType.Love.ToString(), UserId = AdminUserId, PostId = 1 });

        modelBuilder.Entity<Community>().HasData(
            new Community { Id = 1, Name = "Web Development Community", Description = "All you need to know about Web Tech" });
    }
}
