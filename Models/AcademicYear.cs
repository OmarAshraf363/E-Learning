namespace Banha_UniverCity.Models
{
    public class AcademicYear
    {
        public int AcademicYearID { get; set; } // المعرف الفريد للسنة الدراسية
        public string YearName { get; set; } = string.Empty; // اسم السنة الدراسية مثل "First Year" أو "Second Year"

        public ICollection<ClassSchedule> ClassSchedules { get; set; }=new List<ClassSchedule>();
        public ICollection<ApplicationUser> Students { get; set; } = new List<ApplicationUser>(); // الطلاب المرتبطين بهذه السنة الدراسية
    }
}
