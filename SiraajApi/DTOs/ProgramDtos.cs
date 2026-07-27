using System;

namespace SiraajApi.DTOs
{
    public class ProgramDto
    {
        public Guid Id { get; set; }
        public string Icon { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string DescAr { get; set; } = string.Empty;
        public string DescEn { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class CreateProgramRequest
    {
        public string Icon { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string DescAr { get; set; } = string.Empty;
        public string DescEn { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }

    public class UpdateProgramRequest
    {
        public string Icon { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string DescAr { get; set; } = string.Empty;
        public string DescEn { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
