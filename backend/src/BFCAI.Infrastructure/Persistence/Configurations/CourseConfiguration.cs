using BFCAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BFCAI.Infrastructure.Persistence.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasMany(e => e.Keywords).WithOne(e => e.Course)
            .HasForeignKey(e => e.CourseId).OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Instructor).WithMany(u => u.CoursesTaught)
            .HasForeignKey(c => c.InstructorId);

        builder.HasMany(e => e.LearningObjectives).WithOne(e => e.Course)
            .HasForeignKey(e => e.CourseID).OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.TopicsCovered).WithOne(e => e.Course)
            .HasForeignKey(e => e.CourseID).OnDelete(DeleteBehavior.Cascade);
    }
}

public class KeyWordConfiguration : IEntityTypeConfiguration<KeyWord>
{
    public void Configure(EntityTypeBuilder<KeyWord> builder)
    {
        // Legacy DB has DEFAULT N'' on this column (added by an old AddColumn migration).
        builder.Property(k => k.Name).HasDefaultValue("");
    }
}

public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.HasOne(e => e.Student).WithMany(u => u.Enrollments)
            .HasForeignKey(e => e.StudentId).OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Course).WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseID).OnDelete(DeleteBehavior.Cascade);
    }
}

public class CourseVideoConfiguration : IEntityTypeConfiguration<CourseVideo>
{
    public void Configure(EntityTypeBuilder<CourseVideo> builder)
    {
        builder.HasOne(cv => cv.Course).WithMany(c => c.CourseVideos)
            .HasForeignKey(cv => cv.CourseID).OnDelete(DeleteBehavior.Cascade);
    }
}

public class CourseCurriculumConfiguration : IEntityTypeConfiguration<CourseCurriculum>
{
    public void Configure(EntityTypeBuilder<CourseCurriculum> builder)
    {
        builder.HasOne(cc => cc.Course).WithMany(c => c.CourseCurricula)
            .HasForeignKey(cc => cc.CourseID).OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Exams).WithOne(e => e.CourseCurriculum)
            .HasForeignKey(e => e.CurriculumId).OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(e => e.CourseVideos).WithOne(e => e.CourseCurriculum)
            .HasForeignKey(e => e.CourseCurriculumID).OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(e => e.Feedbacks).WithOne(e => e.CourseCurriculum)
            .HasForeignKey(e => e.CourseCurriculumId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class CourseResourceConfiguration : IEntityTypeConfiguration<CourseResource>
{
    public void Configure(EntityTypeBuilder<CourseResource> builder)
    {
        builder.HasOne(e => e.CourseCurriculum).WithMany(e => e.CourseResources)
            .HasForeignKey(e => e.CourseCurriculumID).OnDelete(DeleteBehavior.NoAction);
    }
}

public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
{
    public void Configure(EntityTypeBuilder<Feedback> builder)
    {
        builder.HasOne(f => f.ProviderUser).WithMany(u => u.FeedbacksGiven)
            .HasForeignKey(f => f.ProviderUserId).OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.TargetStudentUser).WithMany(u => u.FeedbacksReceived)
            .HasForeignKey(f => f.TargetStudentUserId).OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.Course).WithMany(c => c.Feedbacks)
            .HasForeignKey(f => f.CourseId);
    }
}
