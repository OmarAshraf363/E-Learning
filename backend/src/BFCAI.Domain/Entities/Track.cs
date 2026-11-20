namespace BFCAI.Domain.Entities
{
    public class Track
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; }= string.Empty;
        public string? Cover {  get; set; }
        public List<Course> Courses { get; set; }= new List<Course>();
    }
}
