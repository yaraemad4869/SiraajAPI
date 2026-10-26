using System;

namespace SiraajApi.Models
{
    public class QuranProgram
    {
        public Guid Id { get; set; }
        public string Icon { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string DescAr { get; set; } = string.Empty;
        public string DescEn { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class Feature
    {
        public Guid Id { get; set; }
        public string Icon { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string TitleAr { get; set; } = string.Empty;
        public string TitleEn { get; set; } = string.Empty;
        public string DescAr { get; set; } = string.Empty;
        public string DescEn { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class HeroSettings
    {
        public Guid Id { get; set; }
        public string TitleAr { get; set; } = string.Empty;
        public string TitleEn { get; set; } = string.Empty;
        public string SubtitleAr { get; set; } = string.Empty;
        public string SubtitleEn { get; set; } = string.Empty;
        public string TaglineAr { get; set; } = string.Empty;
        public string TaglineEn { get; set; } = string.Empty;
        
        // Payment Information
        public string InstapayNumber { get; set; } = string.Empty;
        public string InstapayName { get; set; } = string.Empty;
        public string InstapayLink { get; set; } = string.Empty;
        public string VodafoneCashNumber { get; set; } = string.Empty;
        public string BankName { get; set; } = string.Empty;
        public string BankAccountName { get; set; } = string.Empty;
        public string BankAccountNumber { get; set; } = string.Empty;
        public string BankIban { get; set; } = string.Empty;
        public string BankSwiftCode { get; set; } = string.Empty;
        
        // Contact Information
        public string ContactPhone { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string WhatsAppNumber { get; set; } = string.Empty;
        
        public int ReferralDiscount { get; set; } = 50;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
