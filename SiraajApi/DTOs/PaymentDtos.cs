using System;

namespace SiraajApi.DTOs
{
    public class InitiatePaymentRequest
    {
        public Guid StudentId { get; set; }
        public Guid StudentProgramId { get; set; }
        public string PaymentMethod { get; set; } = string.Empty; // visa, instapay, credit_card, paymob, stripe
        public string PaymentMethodId { get; set; } = string.Empty; // For Stripe/Paymob token
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "SAR"; // SAR or USD
        
        // Card Details
        public string CardNumber { get; set; } = string.Empty;
        public string CardExpiry { get; set; } = string.Empty;
        public string CardCvv { get; set; } = string.Empty;
        public string CardHolderName { get; set; } = string.Empty;
    }

    public class PaymentResponse
    {
        public Guid PaymentId { get; set; }
        public string TransactionId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "SAR";
        public string PaymentUrl { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    public class PaymentHistoryDto
    {
        public Guid Id { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "SAR";
        public string PaymentMethod { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
        public string PackageName { get; set; } = string.Empty;
    }

    public class PaymentCallbackRequest
    {
        public string TransactionId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string PaymentId { get; set; } = string.Empty;
        public string Signature { get; set; } = string.Empty;
    }

    public class InvoiceDto
    {
        public Guid Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; } = "SAR";
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsPaid { get; set; }
        public string PdfUrl { get; set; } = string.Empty;
    }

    public class SubscriptionDto
    {
        public Guid Id { get; set; }
        public string PackageName { get; set; } = string.Empty;
        public string ProgramName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool AutoRenew { get; set; }
    }
}
