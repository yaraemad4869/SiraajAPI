using System;
using System.Collections.Generic;

namespace SiraajApi.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string ReferralCode { get; set; } = string.Empty;
        public DateTime JoinDate { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual Student? Student { get; set; }
        public virtual Guardian? Guardian { get; set; }
        public virtual Teacher? Teacher { get; set; }
        public virtual ICollection<RefreshToken>? RefreshTokens { get; set; }
        public virtual ICollection<Student>? Students { get; set; } // For Teacher's students
        public virtual ICollection<Student>? Children { get; set; } // For Guardian's children
    }
}
