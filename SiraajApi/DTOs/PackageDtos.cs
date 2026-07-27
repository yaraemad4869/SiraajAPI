using System;
using System.Collections.Generic;

namespace SiraajApi.DTOs
{
    public class PackageDto
    {
        public Guid Id { get; set; }
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public int Price { get; set; }
        public int? OriginalPrice { get; set; }
        public int Sessions { get; set; }
        public int SessionsPerWeek { get; set; }
        public string Color { get; set; } = string.Empty;
        public bool IsHighlighted { get; set; }
        public string BadgeAr { get; set; } = string.Empty;
        public string BadgeEn { get; set; } = string.Empty;
        public List<string> FeaturesAr { get; set; } = new();
        public List<string> FeaturesEn { get; set; } = new();
        public bool IsActive { get; set; }
    }

    public class CreatePackageRequest
    {
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
        public List<string> FeaturesAr { get; set; } = new();
        public List<string> FeaturesEn { get; set; } = new();
    }

    public class UpdatePackageRequest
    {
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public int Price { get; set; }
        public int? OriginalPrice { get; set; }
        public int Sessions { get; set; }
        public int SessionsPerWeek { get; set; }
        public string Color { get; set; } = string.Empty;
        public bool IsHighlighted { get; set; }
        public string BadgeAr { get; set; } = string.Empty;
        public string BadgeEn { get; set; } = string.Empty;
        public List<string> FeaturesAr { get; set; } = new();
        public List<string> FeaturesEn { get; set; } = new();
        public bool IsActive { get; set; }
    }
}
