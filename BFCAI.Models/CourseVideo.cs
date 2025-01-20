namespace Banha_UniverCity.Models
{
    public class CourseVideo
    {
        public int CourseVideoID { get; set; } 
        public string VideoTitle { get; set; } = string.Empty;
        public string VideoURL { get; set; } = string.Empty; 
        public int CourseID { get; set; } 
        public Course? Course { get; set; }
        public int? CourseCurriculumID { get; set; }  
        public CourseCurriculum? CourseCurriculum { get; set; }


    }
}
