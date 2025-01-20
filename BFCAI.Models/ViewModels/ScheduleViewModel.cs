using Microsoft.AspNetCore.Mvc.Rendering;

namespace Banha_UniverCity.ViewModels
{
    public class ScheduleViewModel
    {
        public int ClassScheduleId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string DayOfWeek { get; set; } = string.Empty; // The day of the week for the class (e.g., "Monday")


        public int CourseID { get; set; }
        public int InstructorID { get; set; }
        public int RoomID { get; set; }
        public int? DepartmentId { get; set; }
        public int? AcadmicYearId { get; set; }


        // Dropdown lists data

        public IEnumerable<SelectListItem> Courses { get; set; }= Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> Instructors { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> Rooms { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> Departments { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> Years { get; set; } = Enumerable.Empty<SelectListItem>();



    }
}
