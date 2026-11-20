using BFCAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BFCAI.Infrastructure.Persistence.Configurations;

public class ExamConfiguration : IEntityTypeConfiguration<Exam>
{
    public void Configure(EntityTypeBuilder<Exam> builder)
    {
        builder.HasOne(e => e.Instructor).WithMany(u => u.Exams)
            .HasForeignKey(e => e.InstructorId).OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Questions).WithOne(q => q.Exam)
            .HasForeignKey(q => q.ExamID).OnDelete(DeleteBehavior.Cascade);
    }
}

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.HasMany(q => q.Choices).WithOne(o => o.Question)
            .HasForeignKey(o => o.QuestionID).OnDelete(DeleteBehavior.Cascade);
    }
}

public class ExamSubmissionConfiguration : IEntityTypeConfiguration<ExamSubmission>
{
    public void Configure(EntityTypeBuilder<ExamSubmission> builder)
    {
        builder.HasOne(es => es.Student).WithMany(u => u.ExamSubmissions)
            .HasForeignKey(es => es.StudentId).OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(es => es.Exam).WithMany(e => e.ExamSubmissions)
            .HasForeignKey(es => es.ExamID).OnDelete(DeleteBehavior.Cascade);
    }
}

public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
{
    public void Configure(EntityTypeBuilder<Assignment> builder)
    {
        builder.HasOne(a => a.CourseCurriculum).WithMany(c => c.Assignments)
            .HasForeignKey(a => a.CourseCurriculumID).OnDelete(DeleteBehavior.NoAction);
    }
}

public class AssignmentSubmissionConfiguration : IEntityTypeConfiguration<AssignmentSubmission>
{
    public void Configure(EntityTypeBuilder<AssignmentSubmission> builder)
    {
        builder.HasOne(a => a.Assignment).WithMany(a => a.Submissions)
            .HasForeignKey(a => a.AssignmentID).OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.ApplicationUser).WithMany(u => u.AssignmentSubmissions)
            .HasForeignKey(a => a.ApplicationUserID).OnDelete(DeleteBehavior.NoAction);
    }
}

public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
{
    public void Configure(EntityTypeBuilder<Attendance> builder)
    {
        builder.HasOne(a => a.ApplicationUser).WithMany(u => u.Attendances)
            .HasForeignKey(a => a.ApplicationUserID).OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(a => a.CourseCurriculum).WithMany(c => c.Attendances)
            .HasForeignKey(a => a.CourseCurriculumID).OnDelete(DeleteBehavior.NoAction);
    }
}
