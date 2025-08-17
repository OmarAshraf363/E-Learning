using Banha_UniverCity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models.ViewModels
{
    public class CurriculumViewModel
    {
        public virtual List<Feedback> Feedbacks { get; set; } = new List<Feedback>();
        public virtual ICollection<CourseCurriculum> CourseCurricula { get; set; } = new List<CourseCurriculum>();
        public virtual List<CourseVideo> CourseVideos { get; set; } = new List<CourseVideo>();
        public virtual List<Assignment> CourseAssignments { get; set; } = new List<Assignment>();
        public virtual Course Course { get; set; } = new Course();
    }
}
