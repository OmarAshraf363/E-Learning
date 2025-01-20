using Banha_UniverCity.Models;
using BFCAI.Models;

namespace Banha_UniverCity.ViewModels
{
    public class AllModelsVM
    {
        public List<Course> Courses { get; set; }=new List<Course>();


        public List<Course> RecommendationCourses { get; set; } = new List<Course>();


        public List<Department> Departments { get; set; } =new List<Department>();
        public List<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();

        public List<Enrollment> Enrollments { get; set; }= new List<Enrollment>();
        public List<Feedback> Feedbacks { get; set; } = new();
        public List<Event> Events { get; set; }=new List<Event>();
        public List<Track> Tracks { get; set; }=new();

        public List<int> CoursesIdesOfTrack { get; set; } = new();
        

        

    }
}
