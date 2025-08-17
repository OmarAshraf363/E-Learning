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
        public CourseDetailPageViewModel? Course { get; set; }
        public List<CourseDetailViewModel> RelatedCourses { get; set; }=new List<CourseDetailViewModel>();
        public IReadOnlyList<CourseDetailViewModel> InstructorCourses { get; set; } = new List<CourseDetailViewModel>();
        public CourseReviewSectionVM Feedbacks { get; set; }=new CourseReviewSectionVM();

    }
}
