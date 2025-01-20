using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models.ViewModels
{
    public class StudentCourseViewModel
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public List<CourseCurriculumViewModel> Curricula { get; set; } = new();
    }
}
