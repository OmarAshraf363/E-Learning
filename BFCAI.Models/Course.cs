using BFCAI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banha_UniverCity.Models
{
    public class Course
    {
        public int CourseID { get; set; } 
        public string CourseName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Credits { get; set; }

        public string? DemoVideoUrl { get; set; } = string.Empty; // رابط الفيديو التوضيحي للكورس
        public string? ImgCover {  get; set; }
        public int? Rate { get; set; }
        public decimal? Price {  get; set; }
       
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? NumOfEnrollments { get; set; }
        public string InstructorId { get; set; } = string.Empty; 
        public ApplicationUser? Instructor { get; set; }
        public int? DepartmentId {  get; set; }
        [ForeignKey(nameof(DepartmentId))]
        public Department? Department { get; set; }


        public int? TrackID { get; set; }
        [ForeignKey(nameof(TrackID))]
        public Track? Track {  get; set; }

        public ICollection<StudentCourseProgress> StudentCourseProgresses { get; set; } = new List<StudentCourseProgress>();

        public ICollection<KeyWord> Keywords { get; set; }=new List<KeyWord>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>(); // الطلاب المسجلين في الكورس
        public ICollection<CourseVideo> CourseVideos { get; set; } = new List<CourseVideo>(); // الفيديوهات المتعلقة بالكورس
        public ICollection<CourseCurriculum> CourseCurricula { get; set; } = new List<CourseCurriculum>();// منهج الكورس
        public ICollection<Feedback> Feedbacks { get; set; }= new List<Feedback>();
        public ICollection<ClassSchedule> ClassSchedules { get; set; } = new List<ClassSchedule>(); // List of schedules for the course
       public ICollection<Exam> Exams { get; set; } = new List<Exam>();

        public List<LearningObjective>? LearningObjectives { get; set; }=new List<LearningObjective>();
        public List<TopicCovered>? TopicsCovered { get; set; }=new List<TopicCovered>();

    }

}
