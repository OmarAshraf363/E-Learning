using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models.ViewModels
{
    public class QuestionsVM
    {

        public List<QuestionVM> Questions { get; set; } = new List<QuestionVM>();

      

    }
}
