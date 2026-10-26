using System;

namespace SiraajApi.Models
{
    public class Package
    {
        public Guid Id { get; set; }
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        
        // SAR Pricing
        public int Price { get; set; }
        public int? OriginalPrice { get; set; }
        
        // USD Pricing
        public int? PriceUsd { get; set; }
        public int? OriginalPriceUsd { get; set; }
        
        // Default Currency for this package (SAR or USD)
        public string DefaultCurrency { get; set; } = "SAR";
        
        public int Sessions { get; set; }
        public int SessionsPerWeek { get; set; }
        public string Color { get; set; } = "#44C3C6";
        public bool IsHighlighted { get; set; }
        public string BadgeAr { get; set; } = string.Empty;
        public string BadgeEn { get; set; } = string.Empty;
        public string FeaturesAr { get; set; } = "[]"; 
        public string FeaturesEn { get; set; } = "[]"; 
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
