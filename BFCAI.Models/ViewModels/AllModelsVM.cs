using Banha_UniverCity.Models;
using BFCAI.Models;
using BFCAI.Models.ViewModels;

namespace Banha_UniverCity.ViewModels
{
    public class AllModelsVM
    {
        public IReadOnlyList<CourseDetailViewModel> MaxEnrollmentCourses { get; set; }=new List<CourseDetailViewModel>();
        public PagedListViewModel<CourseDetailViewModel> PagedCourses { get; set; } = new PagedListViewModel<CourseDetailViewModel>();


        public IReadOnlyList<CourseDetailViewModel> RecommendationCourses { get; set; } = new List<CourseDetailViewModel>();


        public IReadOnlyList<Department> Departments { get; set; } =new List<Department>();
        public List<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();

        public IReadOnlyList<Enrollment> Enrollments { get; set; }= new List<Enrollment>();
        public IReadOnlyList<Feedback> Feedbacks { get; set; } = new List<Feedback>();
        public IReadOnlyList<Event> Events { get; set; }=new List<Event>();
        public IReadOnlyList<TrackDetailsVIewModel> Tracks { get; set; }=new List<TrackDetailsVIewModel>();

        public List<int> CoursesIdesOfTrack { get; set; } = new();
        

        

    }
}
