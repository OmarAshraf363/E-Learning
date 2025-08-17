using Banha_UniverCity.Models;
using Banha_UniverCity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models.ViewModels
{
    public class CoursesByDepartmentVM
    {
        public string DepartmentName { get; set; } = string.Empty;
        public string DepartmentDescription { get; set; } = string.Empty;
        public PagedListViewModel<CourseDetailViewModel> Courses { get; set; } = new PagedListViewModel<CourseDetailViewModel>();
    }
}
