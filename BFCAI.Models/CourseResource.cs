using Banha_UniverCity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models
{
    public class CourseResource
    {
        public int CourseResourceID { get; set; }  
        public string ResourceTitle { get; set; } = string.Empty;  
        public string ResourceURL { get; set; } = string.Empty;  

        public int? CourseCurriculumID { get; set; } 
        public CourseCurriculum? CourseCurriculum { get; set; }  
    }
}
