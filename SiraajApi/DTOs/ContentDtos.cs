using System;

namespace SiraajApi.DTOs
{
    public class QuranProgramDto
    {
        public Guid Id { get; set; }
        public string Icon { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string DescAr { get; set; } = string.Empty;
        public string DescEn { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class CreateQuranProgramRequest
    {
        public string Icon { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string DescAr { get; set; } = string.Empty;
        public string DescEn { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }

    public class FeatureDto
    {
        public Guid Id { get; set; }
        public string Icon { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string TitleAr { get; set; } = string.Empty;
        public string TitleEn { get; set; } = string.Empty;
        public string DescAr { get; set; } = string.Empty;
        public string DescEn { get; set; } = string.Empty;
    }

    public class CreateFeatureRequest
    {
        public string Icon { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string TitleAr { get; set; } = string.Empty;
        public string TitleEn { get; set; } = string.Empty;
        public string DescAr { get; set; } = string.Empty;
        public string DescEn { get; set; } = string.Empty;
    }

    public class HeroSettingsDto
    {
        public string TitleAr { get; set; } = string.Empty;
        public string TitleEn { get; set; } = string.Empty;
        public string SubtitleAr { get; set; } = string.Empty;
        public string SubtitleEn { get; set; } = string.Empty;
        public string TaglineAr { get; set; } = string.Empty;
        public string TaglineEn { get; set; } = string.Empty;
        public string InstapayNumber { get; set; } = string.Empty;
        public string InstapayName { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public int ReferralDiscount { get; set; }
    }
}
