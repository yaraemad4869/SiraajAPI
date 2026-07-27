using System;

namespace SiraajApi.Models
{
    public class FamilyDiscount
    {
        public Guid Id { get; set; }
        public int Order { get; set; } // 1, 2, 3, 4+
        public int DiscountPercentage { get; set; }
        public string LabelAr { get; set; } = string.Empty;
        public string LabelEn { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
