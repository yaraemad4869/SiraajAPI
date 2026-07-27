using System;

namespace SiraajApi.Models
{
    public class Package
    {
        public Guid Id { get; set; }
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public int Price { get; set; }
        public int? OriginalPrice { get; set; }
        public int Sessions { get; set; }
        public int SessionsPerWeek { get; set; }
        public string Color { get; set; } = "#C9A84C";
        public bool IsHighlighted { get; set; }
        public string BadgeAr { get; set; } = string.Empty;
        public string BadgeEn { get; set; } = string.Empty;
        public string FeaturesAr { get; set; } = "[]"; // JSON array
        public string FeaturesEn { get; set; } = "[]"; // JSON array
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
