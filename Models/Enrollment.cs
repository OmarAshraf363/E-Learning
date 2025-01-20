namespace Banha_UniverCity.Models
{
    public class Enrollment
    {
        public int EnrollmentID { get; set; } // المعرف الفريد للتسجيل
        public string StudentId { get; set; } = string.Empty; // معرف الطالب
        public ApplicationUser? Student { get; set; }// الطالب المسجل في الكورس
        
        public int CourseID { get; set; } // معرف الكورس
        public Course? Course { get; set; } = default!; // الكورس المسجل
        public decimal? Grade { get; set; } // الدرجة التي حصل عليها الطالب في الكورس
    }
}
