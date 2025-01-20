using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models
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
