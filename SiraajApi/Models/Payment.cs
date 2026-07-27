using System;

namespace SiraajApi.Models
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid? StudentProgramId { get; set; }
        public decimal Amount { get; set; }
        public decimal DiscountApplied { get; set; }
        public decimal FinalAmount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty; // visa, instapay, credit_card, paymob, stripe
        public string PaymentGateway { get; set; } = string.Empty; // stripe, paymob
        public string TransactionId { get; set; } = string.Empty;
        public string Status { get; set; } = "pending"; // pending, completed, failed, refunded
        public DateTime PaymentDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Student Student { get; set; }
        public StudentProgram StudentProgram { get; set; }
    }
}
