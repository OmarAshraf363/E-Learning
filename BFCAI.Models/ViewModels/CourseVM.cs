using Banha_UniverCity.Models;
using BFCAI.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banha_UniverCity.ViewModels
{
    public class CourseVM
    {
        public int CourseID { get; set; } // المعرف الفريد للكورس

        [Required(ErrorMessage = "Course name is required.")]
        [StringLength(100, ErrorMessage = "Course name cannot be longer than 100 characters.")]
        public string CourseName { get; set; } = string.Empty; // اسم الكورس

        [Required(ErrorMessage = "Course description is required.")]
        [StringLength(1000, ErrorMessage = "Description cannot be longer than 1000 characters.")]
        public string Description { get; set; } = string.Empty; // وصف الكورس

        [Required(ErrorMessage = "Credits are required.")]
        [Range(1, 50, ErrorMessage = "Credits must be between 1 and 50.")]
        public int Credits { get; set; } // عدد الساعات المعتمدة

        [Required(ErrorMessage = "Instructor is required.")]
        public string InstructorId { get; set; } = string.Empty; // معرف المعلم
        public string? ImgCover {  get; set; }
        public int? Rate {  get; set; }   

        public ApplicationUser? Instructor { get; set; } // المعلم المسؤول عن الكورس

        [Required(ErrorMessage = "Department is required.")]
        public int? DepartmentId { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public Department? Department { get; set; }

        [Required]
        [Url(ErrorMessage = "Invalid URL format.")]
        public string? DemoVideoUrl { get; set; } = string.Empty; // رابط الفيديو التوضيحي للكورس

        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]

        public DateTime? EndDate { get; set; }

        public int? NumOfEnrollments { get; set; }

        public Course? Course { get; set; }

        [ValidateNever]
        public IEnumerable<Department> Departments { get; set; } = new List<Department>();

        [ValidateNever]
        public IEnumerable<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();

        [Required(ErrorMessage = "At least one topic must be added.")]
        public List<TopicCovered> TopicCovereds { get; set; } = new List<TopicCovered>();

        [Required(ErrorMessage = "At least one learning objective must be added.")]
        public List<LearningObjective> LearningObjectives { get; set; } = new List<LearningObjective>();
        [Required(ErrorMessage = "At least one key word must be added.")]

        public List<KeyWord> KeyWords { get; set; } = new List<KeyWord>();

        public int? TrackId { get; set; }
        [ValidateNever]
        public IEnumerable<Track> Tracks { get; set; }= new List<Track>();
    }

}