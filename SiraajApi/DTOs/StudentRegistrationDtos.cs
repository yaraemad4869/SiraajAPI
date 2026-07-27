using System;
using System.Collections.Generic;

namespace SiraajApi.DTOs
{
    public class StudentRegistrationRequest
    {
        public Guid? StudentId { get; set; } // For existing students
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public int? Age { get; set; }
        public string Level { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string GuardianPhone { get; set; } = string.Empty;
        public Guid PackageId { get; set; }
        public Guid ProgramId { get; set; }
        public List<string> Schedule { get; set; } = new(); // ["sat-09:00", "mon-09:00"]
        public string DiscountCode { get; set; } = string.Empty;
    }

    public class StudentRegistrationResponse
    {
        public Guid StudentId { get; set; }
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string PackageName { get; set; } = string.Empty;
        public string ProgramName { get; set; } = string.Empty;
        public int MonthlyPrice { get; set; }
        public int DiscountApplied { get; set; }
        public int FinalPrice { get; set; }
        public List<string> Schedule { get; set; } = new();
        public string Status { get; set; } = string.Empty;
    }

    public class FamilyDiscountDto
    {
        public int Order { get; set; }
        public int DiscountPercentage { get; set; }
        public string LabelAr { get; set; } = string.Empty;
        public string LabelEn { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
    }

    public class CalculateDiscountRequest
    {
        public int ChildCount { get; set; }
        public string ReferralCode { get; set; } = string.Empty;
        public int PackagePrice { get; set; }
    }

    public class DiscountCalculationResponse
    {
        public int FamilyDiscount { get; set; }
        public int ReferralDiscount { get; set; }
        public int TotalDiscount { get; set; }
        public int FinalPrice { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
