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
        
        public IReadOnlyList<CourseDetailViewModel> CourseList { get; set; }=new List<CourseDetailViewModel>();
        public PagedListViewModel<CourseDetailViewModel> PagedCourseList { get; set; } = new PagedListViewModel<CourseDetailViewModel>();
        public List<Department> DepartmentList { get; set; } =new List<Department>();
        public List<LearningObjective> LearningObjectiveList { get; set;} =new List<LearningObjective>();
        public List<TopicCovered> TopicCoveredList { get; set; } = new List<TopicCovered>();

    }
}
