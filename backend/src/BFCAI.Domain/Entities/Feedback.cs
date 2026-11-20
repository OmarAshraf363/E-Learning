namespace BFCAI.Domain.Entities
{
    public class Feedback
    {
        public int FeedbackID { get; set; } 
        public string Content { get; set; } = string.Empty; 
        public DateTime FeedbackDate { get; set; } 

        public int? Rating { get; set; } // Assuming a rating system from 1 to 5
        public string ProviderUserId { get; set; } = string.Empty;
        public ApplicationUser? ProviderUser { get; set; }

        public int? CourseCurriculumId { get; set; }
        public CourseCurriculum? CourseCurriculum { get; set; }
        public string? TargetStudentUserId { get; set; } 
        public ApplicationUser? TargetStudentUser { get; set; } 

        public int? CourseId { get; set; }
        public Course? Course { get; set; } 
    }
}
