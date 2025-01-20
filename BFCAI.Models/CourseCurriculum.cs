using BFCAI.Models;

namespace Banha_UniverCity.Models
{
    public class CourseCurriculum
    {
        public int CourseCurriculumID { get; set; } 
        public string Title { get; set; } = string.Empty; 
        public string Content { get; set; } = string.Empty; 
        public int CourseID { get; set; } 
        public Course? Course { get; set; }
        
        public ICollection<CourseResource> CourseResources { get; set; } = new List<CourseResource>();

        public ICollection<CourseVideo> CourseVideos { get; set; }=new List<CourseVideo>();
        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
        public ICollection<Exam> Exams { get; set; }= new List<Exam>();


    }
}

