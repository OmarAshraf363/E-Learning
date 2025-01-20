using Banha_UniverCity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models
{
    public class StudentCourseProgress
    {
        public int Id { get; set; }
        public string? StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public ApplicationUser? Student { get; set; }

        public int? CourseId { get; set; }
        public Course? Course { get; set; }

        public decimal ProgressPercentage { get; set; } // Range: 0 to 100
        public DateTime? LastUpdated { get; set; }
    }
}
