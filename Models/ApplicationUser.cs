using Microsoft.AspNetCore.Identity;

namespace Banha_UniverCity.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; } = string.Empty; // الاسم الكامل للمستخدم
        public string UserType { get; set; } = string.Empty; // نوع المستخدم: "Student" أو "Instructor"

        public int? AvailableCreditHours { get; set; } = 0;  // الساعات المتاحة للتسجيل

        public int? AcademicYearID { get; set; }  // FK to AcademicYear
        public AcademicYear? AcademicYear { get; set; }  // الربط مع كيان AcademicYear


        public int? DepartmentId {  get; set; }
        public Department? Department { get; set; }



        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();  // الارتباطات بالكورسات (للطلاب)
        public ICollection<Course> CoursesTaught { get; set; } = new List<Course>();// الكورسات التي يقوم بتدريسها (للمعلمين)


        // Feedbacks given by the user (if they are an instructor or admin)
        public ICollection<Feedback> FeedbacksGiven { get; set; } = new List<Feedback>();

        // Feedbacks received by the user (if they are a student)
        public ICollection<Feedback> FeedbacksReceived { get; set; } = new List<Feedback>();

        public ICollection<Event> Events { get; set; } = new List<Event>();


        public ICollection<ClassSchedule> Schedules { get; set; } = new List<ClassSchedule>();

    }

}
