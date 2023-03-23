﻿namespace ECommerce.API.Models
{
    public class InsertPaymentReq
    {
        public int Id { get; set; }
        public int PaymentMethodId { get; set; }
        public int UserId { get; set; }
        public int TotalAmount { get; set; }
        public int ShipingCharges { get; set; }
        public int AmountReduced { get; set; }
        public int AmountPaid { get; set; }
        public string CreatedAt { get; set; } = string.Empty;
    }
}