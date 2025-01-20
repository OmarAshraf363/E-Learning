using BFCAI.Models;
using Microsoft.AspNetCore.Identity;

namespace Banha_UniverCity.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; } = string.Empty; // الاسم الكامل للمستخدم
        public string? Picture {  get; set; }=string.Empty;
        public string UserType { get; set; } = string.Empty; // نوع المستخدم: "Student" أو "Instructor"

        public string JopDescription {  get; set; } = string.Empty;
        public int? AvailableCreditHours { get; set; } = 0;  // الساعات المتاحة للتسجيل

        public decimal? Gpa {  get; set; }

        public int? AcademicYearID { get; set; }  // FK to AcademicYear
        public AcademicYear? AcademicYear { get; set; }  // الربط مع كيان AcademicYear


        public int? DepartmentId {  get; set; }
        public Department? Department { get; set; }


        public ICollection<StudentCourseProgress> StudentCourseProgresses { get; set; }=new List<StudentCourseProgress>();





        public ICollection<Cart> Carts { get; set; } = new List<Cart>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();  // الارتباطات بالكورسات (للطلاب)
        public ICollection<Course> CoursesTaught { get; set; } = new List<Course>();// الكورسات التي يقوم بتدريسها (للمعلمين)


        // Feedbacks given by the user (if they are an instructor or admin)
        public ICollection<Feedback> FeedbacksGiven { get; set; } = new List<Feedback>();

        // Feedbacks received by the user (if they are a student)
        public ICollection<Feedback> FeedbacksReceived { get; set; } = new List<Feedback>();

        public ICollection<Event> Events { get; set; } = new List<Event>();


        public ICollection<ClassSchedule> Schedules { get; set; } = new List<ClassSchedule>();
        public ICollection<Exam> Exams { get; set; } = new List<Exam>();

        public ICollection<ExamSubmission> ExamSubmissions { get; set; } = new List<ExamSubmission>();
        public ICollection<AssignmentSubmission> AssignmentSubmissions { get; set; } = new List<AssignmentSubmission>();
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();






        
    }

}
