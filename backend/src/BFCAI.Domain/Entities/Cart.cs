namespace BFCAI.Domain.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public string? UserId { get; set; } = string.Empty;
        public ApplicationUser? ApplicationUser {  get; set; }
        
        public ICollection<CartItems> Items { get; set; }=new List<CartItems>();
    }
}
