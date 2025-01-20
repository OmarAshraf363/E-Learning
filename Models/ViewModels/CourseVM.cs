using Banha_UniverCity.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banha_UniverCity.ViewModels
{
    public class CourseVM
    {
        public int CourseID { get; set; } // المعرف الفريد للكورس
        public string CourseName { get; set; } = string.Empty;// اسم الكورس
        public string Description { get; set; } = string.Empty;// وصف الكورس
        public int Credits { get; set; } // عدد الساعات المعتمدة
        public string InstructorId { get; set; } = string.Empty; // معرف المعلم
        public ApplicationUser? Instructor { get; set; }// المعلم المسؤول عن الكورس
        public int DepartmentId { get; set; }
        [ForeignKey(nameof(DepartmentId))]
        public Department? Department { get; set; }


        public Course? Course { get; set; }


        [ValidateNever]
        public ICollection<Department> Departments { get; set; } = new List<Department>();
        [ValidateNever]

        public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();


    }
}
