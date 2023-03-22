namespace ECommerce.API.Models
{
    public class InsertOrderReq
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CartId { get; set; }
        public int PaymentId { get; set; }
        public string CreatedAt { get; set; } = string.Empty;
    }
}
