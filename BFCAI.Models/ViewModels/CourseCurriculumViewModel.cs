using Banha_UniverCity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models.ViewModels
{
    public class CourseCurriculumViewModel
    {
        public string CourseName { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public int? DepartmentId { get; set; }
        public int? TrackId { get; set; }
        public string InstructorId { get; set; } = string.Empty;
        public string CurriculumName { get; set; }=string.Empty;
        public IEnumerable<SUbCurrcula> sUbCurrculas { get; set; }= new List<SUbCurrcula>();      
   
        public List<Exam> Exams { get; set; } = new();
    }
    public class CourseDetailPageViewModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int? DepartmentId { get; set; }
        public int? TrackId { get; set; }
        public string? InstructorId { get; set; }
        public string CourseDescription { get; set; }= string.Empty;
        public string InstructorImage { get; set; } = string.Empty;
        public string InstructorBio { get; set; } = string.Empty;

        public string InstructorName { get; set; } = string.Empty;
        public string DemoVideoUrl { get; set; } = string.Empty;
        public string DemoTitle { get; set; } = string.Empty;
        public int? Rate { get; set; }
        public decimal? Price { get; set; }
        public int NumOfFeedbacks { get; set; }
        public int NumOfVideos { get; set; }
        public int NumOfEnrollments { get; set; }
        public List<string> LearningObjectives { get; set; } = new();
        public List<string> TopicsCovered { get; set; } = new();
    }
    public class SUbCurrcula
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; } = string.Empty;
        public IEnumerable<RefVM> Resources { get; set; } = new List<RefVM>();
        public IEnumerable<VideoVM> Videos { get; set; } = new List<VideoVM>();
        public IEnumerable<ASSVM> Assignments { get; set; } = new List<ASSVM>();
        public IEnumerable<FeedbackVM> Feedbacks { get; set; }= new List<FeedbackVM>();


    }
    public class FeedbackVM
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserImageUrl { get; set; }
        public string Content { get; set; }
        public int? Rating { get; set; } // 1 to 5
        public DateTime CreatedAt { get; set; }
    }
    public class ASSVM
    {
        public int Id { get; set; }
        public string Title{ get; set; }
        public string Url { get; set; }
        public DateTime? DeadLine { get; set; }
        public List<AssignmentSubmission> Submissions { get; set; }=new List<AssignmentSubmission>();
        
    }
    public class RefVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
    public class VideoVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}
