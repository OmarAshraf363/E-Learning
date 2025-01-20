using Banha_UniverCity.Models;


namespace BFCAI.Models
{
    public class Assignment
    {
        public int AssignmentID { get; set; }
        public int CourseCurriculumID { get; set; } 
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        public DateTime DeadLine { get; set; } 

        
        public CourseCurriculum? CourseCurriculum { get; set; }
        public ICollection<AssignmentSubmission> Submissions { get; set; }=new List<AssignmentSubmission>();
    }
}
