using Banha_UniverCity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models
{
    public class Exam
    {
        public int ExamID { get; set; }
        public string Title { get; set; }=string.Empty;
        public DateTime ExamDate { get; set; }
        public int? CurriculumId { get; set; }
        public CourseCurriculum? CourseCurriculum { get; set; }
        public int CourseID { get; set; } 
        public Course? Course { get; set; }

        // Instructor who created the exam
        public string? InstructorId { get; set; }
        public ApplicationUser? Instructor { get; set; }

        // الطلاب الذين قدموا الامتحان
        public ICollection<ExamSubmission> ExamSubmissions { get; set; } = new List<ExamSubmission>();

        public ICollection<Question> Questions { get; set; } = new List<Question>();

    }
}
