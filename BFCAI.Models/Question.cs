using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models
{
    public class Question
    {
        public int QuestionID { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public int ExamID { get; set; }
        public Exam? Exam { get; set; }

        public ICollection<QuestionChoice> Choices { get; set; }=new List<QuestionChoice>();

    }
}
