using Banha_UniverCity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models.ViewModels
{
    public class DetailsVM
    {
        public Course? Course { get; set; }
        public List<Course> RelatedCourses { get; set; }=new List<Course>();
        public List<Course> InstructorCourses { get; set; } = new List<Course>();

    }
}
