using System;

namespace SiraajApi.Models
{
    public class DiscountCode
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public int DiscountAmount { get; set; } = 50;
        public Guid? UsedBy { get; set; }
        public DateTime? UsedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public virtual User? User { get; set; }
    }
}
