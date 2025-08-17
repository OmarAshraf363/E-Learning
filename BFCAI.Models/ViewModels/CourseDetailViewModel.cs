using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models.ViewModels
{
    public class CourseDetailViewModel
    {
        public int CourseID { get; set; }
        public string Image { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string InstructorName { get; set; } = string.Empty;
        public double? Rate { get; set; }
        public decimal? Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public int NumOfEnrolments { get; set; }
        public int VideosCount { get; set; }
        public List<string> LearningObjectives { get; set; } = new();
    }
}
