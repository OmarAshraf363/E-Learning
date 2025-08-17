using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models.ViewModels
{
    public class TrackDetailsVIewModel
    {
        public int? TrackId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Cover { get; set; } = string.Empty;
        public int NumOfCourses { get; set; }
        public int NUMOfEnrollments { get; set; }
        public IReadOnlyList<CourseDetailViewModel> CoursesThatLoginStudentNotHave { get; set; } = new List<CourseDetailViewModel>();
        public int NumOfCoursesThatLoginStudentNotHave => CoursesThatLoginStudentNotHave.Count;
        public int NumOfCoursesThatLoginStudentHave => Courses.Count - NumOfCoursesThatLoginStudentNotHave;
        public IReadOnlyList<CourseDetailViewModel> Courses { get; set; } = new List<CourseDetailViewModel>();




    }
}
