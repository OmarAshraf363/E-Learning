using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models.ViewModels
{
   public class QuestionVM
    {
        public int QuestionID { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public int ExamID { get; set; }

        [ValidateNever]
        public List<QuestionChoice> Choices { get; set; } = new List<QuestionChoice>();
    }
}
