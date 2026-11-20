namespace BFCAI.Domain.Entities
{
    public class OrderItems
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? CourseId { get; set; }
        public int? Quantity { get; set; }
        public decimal? ListPrice { get; set; }

        public decimal? Discount { get; set; }
        public decimal? TotalPrice { get; set; }

        //navigation
        public Order? Order { get; set; }
        public Course? Course { get; set; }

    }
}
