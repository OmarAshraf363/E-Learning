using Banha_UniverCity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models
{
    public class Attendance
    {
        public int AttendanceID { get; set; }
        public string ApplicationUserID { get; set; } = string.Empty;
        public int CourseCurriculumID { get; set; }
        public DateTime AttendanceDate { get; set; }
        public bool IsPresent { get; set; }

        public ApplicationUser? ApplicationUser { get; set; }
        public CourseCurriculum? CourseCurriculum { get; set; }
    }
}
