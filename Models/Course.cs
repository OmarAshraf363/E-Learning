using System.ComponentModel.DataAnnotations.Schema;

namespace Banha_UniverCity.Models
{
    public class Course
    {
        public int CourseID { get; set; } // المعرف الفريد للكورس
        public string CourseName { get; set; } = string.Empty;// اسم الكورس
        public string Description { get; set; } = string.Empty;// وصف الكورس
        public int Credits { get; set; } // عدد الساعات المعتمدة
        public string InstructorId { get; set; } = string.Empty; // معرف المعلم
        public ApplicationUser? Instructor { get; set; }// المعلم المسؤول عن الكورس
        public int DepartmentId {  get; set; }
        [ForeignKey(nameof(DepartmentId))]
        public Department? Department { get; set; }

       

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>(); // الطلاب المسجلين في الكورس
        public ICollection<CourseVideo> CourseVideos { get; set; } = new List<CourseVideo>(); // الفيديوهات المتعلقة بالكورس
        public ICollection<CourseCurriculum> CourseCurricula { get; set; } = new List<CourseCurriculum>();// منهج الكورس
        public ICollection<Feedback> Feedbacks { get; set; }= new List<Feedback>();
        public ICollection<ClassSchedule> ClassSchedules { get; set; } = new List<ClassSchedule>(); // List of schedules for the course

    }
}
