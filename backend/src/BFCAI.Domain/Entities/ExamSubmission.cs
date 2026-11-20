namespace BFCAI.Domain.Entities
{
    public class ExamSubmission
    {
        public int ExamSubmissionID { get; set; }
        public int ExamID { get; set; }
        public Exam? Exam { get; set; }

        public string? StudentId { get; set; }
        public ApplicationUser? Student { get; set; }

        public DateTime SubmissionDate { get; set; }
        public float Score { get; set; }
    }
}
