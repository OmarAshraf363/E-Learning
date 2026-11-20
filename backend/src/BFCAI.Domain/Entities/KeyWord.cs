
namespace BFCAI.Domain.Entities
{
    public class KeyWord
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        //navigation property 
        public int? PostId { get; set; }
        public Post? Post { get; set; }

        public int? CourseId { get; set; }
        public Course? Course { get; set; }
    }
}
