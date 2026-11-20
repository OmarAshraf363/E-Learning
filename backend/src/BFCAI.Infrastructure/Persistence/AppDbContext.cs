using BFCAI.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BFCAI.Infrastructure.Persistence;

/// <summary>
/// EF Core context. The model maps to EXACTLY the same schema as the legacy MVC
/// ApplicationDbContext (same tables, names, keys and delete behaviors), plus RefreshTokens.
/// Note: the context is IdentityDbContext&lt;IdentityUser&gt; with ApplicationUser as a derived type,
/// which keeps the legacy "Discriminator" column on AspNetUsers.
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<IdentityUser>(options)
{
    // DbSet names are kept identical to the legacy context because they define table names.
    public DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<CourseVideo> CourseVideos => Set<CourseVideo>();
    public DbSet<CourseCurriculum> CourseCurricula => Set<CourseCurriculum>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Exam> Exams => Set<Exam>();
    public DbSet<QuestionChoice> QuestionChoices => Set<QuestionChoice>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<ExamSubmission> ExamSubmissions => Set<ExamSubmission>();
    public DbSet<CourseResource> CourseResources => Set<CourseResource>();
    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<Assignment> Assignments => Set<Assignment>();
    public DbSet<AssignmentSubmission> AssignmentSubmissions => Set<AssignmentSubmission>();
    public DbSet<KeyWord> KeyWords => Set<KeyWord>();
    public DbSet<OrderItems> OrderItems => Set<OrderItems>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItems> CartItems => Set<CartItems>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Reaction> Reactions => Set<Reaction>();
    public DbSet<Community> Communities => Set<Community>();
    public DbSet<Track> Track => Set<Track>();
    public DbSet<Feedback> Feedback => Set<Feedback>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        SeedData.Apply(builder);
    }
}
