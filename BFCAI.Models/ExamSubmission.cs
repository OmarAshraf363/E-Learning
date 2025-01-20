using Banha_UniverCity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models
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
