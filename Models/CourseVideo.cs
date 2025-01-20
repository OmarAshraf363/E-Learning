namespace Banha_UniverCity.Models
{
    public class CourseVideo
    {
        public int CourseVideoID { get; set; } // المعرف الفريد للفيديو
        public string VideoTitle { get; set; } = string.Empty; // عنوان الفيديو
        public string VideoURL { get; set; } = string.Empty; // رابط الفيديو
        public int CourseID { get; set; } // معرف الكورس
        public Course? Course { get; set; }  // الكورس الذي ينتمي إليه الفيديو
    }
}
