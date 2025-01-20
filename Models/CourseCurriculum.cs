namespace Banha_UniverCity.Models
{
    public class CourseCurriculum
    {
        public int CourseCurriculumID { get; set; } // المعرف الفريد للمنهج
        public string Title { get; set; } = string.Empty; // عنوان المنهج أو الوحدة
        public string Content { get; set; } = string.Empty; // محتوى المنهج (نص أو HTML)
        public int CourseID { get; set; } // معرف الكورس
        public Course? Course { get; set; }  // الكورس الذي ينتمي إليه المنهج
    }
}
