namespace Banha_UniverCity.Models
{
    public class TopicCovered
    {
        public int TopicCoveredID { get; set; }
        public string Topic { get; set; } = string.Empty;

        public int CourseID { get; set; }
        public Course? Course { get; set; }
    }
}