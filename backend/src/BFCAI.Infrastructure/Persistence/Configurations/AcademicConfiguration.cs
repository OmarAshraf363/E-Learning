using BFCAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BFCAI.Infrastructure.Persistence.Configurations;

public class ClassScheduleConfiguration : IEntityTypeConfiguration<ClassSchedule>
{
    public void Configure(EntityTypeBuilder<ClassSchedule> builder)
    {
        builder.HasOne(s => s.Course).WithMany(c => c.ClassSchedules)
            .HasForeignKey(s => s.CourseId).OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.Instructor).WithMany(i => i.Schedules)
            .HasForeignKey(s => s.InstructorId).OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Room).WithMany(r => r.Schedules)
            .HasForeignKey(s => s.RoomId).OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.AcadmicYear).WithMany(y => y.ClassSchedules)
            .HasForeignKey(s => s.AcadmicYearId).OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Department).WithMany(d => d.ClassSchedules)
            .HasForeignKey(s => s.DepartmentId).OnDelete(DeleteBehavior.Restrict);
    }
}

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasOne(e => e.Department).WithMany(d => d.Events)
            .HasForeignKey(e => e.DepartmentID);

        builder.HasOne(e => e.CreatedBy).WithMany(u => u.Events)
            .HasForeignKey(e => e.CreatedById).OnDelete(DeleteBehavior.Restrict);
    }
}

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.HasMany(e => e.Users).WithOne(e => e.Department)
            .HasForeignKey(e => e.DepartmentId).OnDelete(DeleteBehavior.Restrict);

        // Legacy DB has DEFAULT N'' on these columns (added by old AddColumn migrations).
        builder.Property(d => d.Cover).HasDefaultValue("");
        builder.Property(d => d.DepartmentDescription).HasDefaultValue("");
    }
}
