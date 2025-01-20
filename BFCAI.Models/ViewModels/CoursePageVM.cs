using Banha_UniverCity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models.ViewModels
{
    public  class CoursePageVM
    {
        public List<Course> CourseList { get; set; }=new List<Course>();
        public List<Department> DepartmentList { get; set; } =new List<Department>();
        public List<LearningObjective> LearningObjectiveList { get; set;} =new List<LearningObjective>();
        public List<TopicCovered> TopicCoveredList { get; set; } = new List<TopicCovered>();
    }
}
