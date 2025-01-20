namespace Banha_UniverCity.Models
{
    public class Feedback
    {
        public int FeedbackID { get; set; } 
        public string Content { get; set; } = string.Empty; 
        public DateTime FeedbackDate { get; set; } 

        
        public string ProviderUserId { get; set; } = string.Empty;
        public ApplicationUser ProviderUser { get; set; } = default!; 

        
        public string? TargetStudentUserId { get; set; } 
        public ApplicationUser? TargetStudentUser { get; set; } 

       
        public int? CourseId { get; set; }
        public Course? Course { get; set; } 
    }
}
