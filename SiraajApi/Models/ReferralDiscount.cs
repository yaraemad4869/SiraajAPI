using System;

namespace SiraajApi.Models
{
    public class ReferralDiscount
    {
        public Guid Id { get; set; }
        public string ReferralCode { get; set; } = string.Empty;
        public Guid ReferrerId { get; set; } // The person who shared the code
        public Guid? ReferredUserId { get; set; } // The person who used the code
        public int DiscountAmount { get; set; } = 50;
        public bool IsUsed { get; set; }
        public DateTime? UsedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public User Referrer { get; set; }
        public User ReferredUser { get; set; }
    }
}
