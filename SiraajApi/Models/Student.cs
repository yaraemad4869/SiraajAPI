using System;

namespace SiraajApi.Models
{
    public class Student
    {
        public Guid UserId { get; set; }
        public string PackageId { get; set; } = string.Empty;
        public Guid? TeacherId { get; set; }
        public Guid? GuardianId { get; set; }
        public string Schedule { get; set; } = "[]";
        public string Status { get; set; } = "pending";
        public int? Age { get; set; }
        public string Level { get; set; } = string.Empty;
        
        public virtual User? User { get; set; }
        public virtual User? Teacher { get; set; } // This is a User who is a Teacher
        public virtual User? Guardian { get; set; } // This is a User who is a Guardian
    }
}
