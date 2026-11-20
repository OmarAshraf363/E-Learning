namespace BFCAI.Domain.Entities
{
    public class QuestionChoice
    {
        public int QuestionChoiceID { get; set; }
        public string ChoiceText { get; set; } = string.Empty; 

        public bool IsCorrect { get; set; } 

        public int QuestionID { get; set; } 
        public Question? Question { get; set; }
    }
}
