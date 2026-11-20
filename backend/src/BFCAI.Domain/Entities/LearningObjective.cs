
namespace BFCAI.Domain.Entities
{
    public class LearningObjective
    {
        public int LearningObjectiveID { get; set; }
        public string Objective { get; set; } = string.Empty;

        public int? CourseID { get; set; }//null For Allowed Add Genral Learning Goals
        public Course? Course { get; set; }
    }
}
