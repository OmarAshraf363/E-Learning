using Banha_UniverCity.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models.ViewModels
{
    public class EnrollmentViewModel
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;
        public string Email { get; set; }=string.Empty;
        public int TotalEnrollments {  get; set; }
        public List<SelectListItem>? Courses { get; set; } // لعرض قائمة الكورسات
        public List<Enrollment> Enrollments { get; set; }=new List<Enrollment>();
    }
}
