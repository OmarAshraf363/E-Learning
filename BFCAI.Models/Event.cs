namespace Banha_UniverCity.Models
{
    public class Event
    {
        public int EventID { get; set; }
        public string Name { get; set; }=string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }

        // يمكن ربط الحدث بقسم معين
        public int? DepartmentID { get; set; }
        public Department? Department { get; set; }

        // يمكن ربط الحدث بمسؤول أو منشئ الحدث (مثلاً مستخدم معين)
        public string? CreatedById { get; set; }
        public ApplicationUser? CreatedBy { get; set; }
    }
}
