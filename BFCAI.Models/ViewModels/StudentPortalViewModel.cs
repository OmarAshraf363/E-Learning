using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models.ViewModels
{
    public class StudentPortalViewModel
    {
        public string StudentName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<StudentCourseViewModel> Courses { get; set; } = new();
    }
}
