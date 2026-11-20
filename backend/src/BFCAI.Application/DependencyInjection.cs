using BFCAI.Application.Features.AcademicYears;
using BFCAI.Application.Features.Account;
using BFCAI.Application.Features.Cart;
using BFCAI.Application.Features.Catalog;
using BFCAI.Application.Features.Courses;
using BFCAI.Application.Features.Dashboard;
using BFCAI.Application.Features.Departments;
using BFCAI.Application.Features.Enrollments;
using BFCAI.Application.Features.Instructor;
using BFCAI.Application.Features.Lookups;
using BFCAI.Application.Features.Schedules;
using BFCAI.Application.Features.Student;
using BFCAI.Application.Features.Tracks;
using BFCAI.Application.Features.Auth;
using BFCAI.Application.Features.Users;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BFCAI.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Every AbstractValidator<T> in this assembly is registered automatically.
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        // Feature services: one line per feature.
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ILookupService, LookupService>();
        services.AddScoped<IAdminDashboardService, AdminDashboardService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<ITrackService, TrackService>();
        services.AddScoped<IAcademicYearService, AcademicYearService>();
        services.AddScoped<ICourseManagementService, CourseManagementService>();
        services.AddScoped<IScheduleService, ScheduleService>();
        services.AddScoped<IEnrollmentService, EnrollmentService>();
        services.AddScoped<ICatalogService, CatalogService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<ICheckoutService, CheckoutService>();
        services.AddScoped<InstructorAccess>();
        services.AddScoped<IInstructorCourseService, InstructorCourseService>();
        services.AddScoped<ICurriculumService, CurriculumService>();
        services.AddScoped<IExamService, ExamService>();
        services.AddScoped<IAssignmentService, AssignmentService>();
        services.AddScoped<IInstructorStudentService, InstructorStudentService>();
        services.AddScoped<IStudentService, StudentService>();

        return services;
    }
}
