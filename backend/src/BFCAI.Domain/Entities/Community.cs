namespace BFCAI.Domain.Entities
{
    public class Community
    {
        public int Id { get; set; }
        public string Name { get; set; }=string.Empty;

        public string Description { get; set; } = string.Empty;

        public ICollection<Post> Posts { get; set; }=new List<Post>();

    }
}
