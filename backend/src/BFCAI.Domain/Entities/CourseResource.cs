namespace BFCAI.Domain.Entities
{
    public class CourseResource
    {
        public int CourseResourceID { get; set; }  
        public string ResourceTitle { get; set; } = string.Empty;  
        public string ResourceURL { get; set; } = string.Empty;  

        public int? CourseCurriculumID { get; set; } 
        public CourseCurriculum? CourseCurriculum { get; set; }  
    }
}
