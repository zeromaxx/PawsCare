namespace PawsCare.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string? CustomerName { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
        public string? Notes { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
