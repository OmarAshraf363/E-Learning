using Banha_UniverCity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models.ViewModels
{
    public class HomeStudentViewModels
    {
        public IEnumerable<Course>? Courses { get; set; }=new List<Course>();
        public List<CourseCurriculum> CourseCurricula { get; set; } =new List<CourseCurriculum>();
        public List<Enrollment> Enrollments { get; set; }= new List<Enrollment>();
        public List<ClassSchedule> ClassSchedule { get; set; } = new List<ClassSchedule>(); 
        public IReadOnlyList<Feedback> Feedbacks { get; set; } = new List<Feedback>();
        public IReadOnlyList<CourseDetailViewModel> SomeAvailableCourse { get; set; } = new List<CourseDetailViewModel>();
        public IReadOnlyList<PostVM> Posts { get; set; }=new List<PostVM>();
        public IReadOnlyList<Community> Communities { get; set; }=new List<Community>();
        
    }
}
