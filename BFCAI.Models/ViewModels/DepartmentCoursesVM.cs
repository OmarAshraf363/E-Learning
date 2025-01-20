using Banha_UniverCity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models.ViewModels
{
    public  class DepartmentCoursesVM
    {
        public Department? Department { get; set; }
        public List<Course> Courses { get; set; } = new List<Course>();
        public List<Department> Departments { get; set; }= new List<Department>();
        public List<KeyWord> KeyWords { get; set; }= new List<KeyWord>();
    }
}
