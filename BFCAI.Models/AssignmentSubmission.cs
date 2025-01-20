using Banha_UniverCity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models
{
    public class AssignmentSubmission
    {
        public int AssignmentSubmissionID { get; set; }
        public int AssignmentID { get; set; } 
        public string ApplicationUserID { get; set; }=string.Empty; 
        public DateTime SubmissionDate { get; set; } 
        public string Content { get; set; } = string.Empty; 
        public string Feedback { get; set; } = string.Empty; 
        public int? Grade { get; set; } 

        public Assignment? Assignment { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
