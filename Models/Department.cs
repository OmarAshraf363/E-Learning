namespace Banha_UniverCity.Models
{
    public class Department
    {
        public int DepartmentID { get; set; } // المعرف الفريد للقسم
        public string DepartmentName { get; set; } = string.Empty; // اسم القسم
        public ICollection<Course> Courses { get; set; } = new List<Course>(); // الكورسات التي يتضمنها القسم
        public ICollection<Event> Events { get; set; } = new List<Event>();
        public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        public ICollection<ClassSchedule> ClassSchedules { get; set; }= new List<ClassSchedule>();

    }
}
