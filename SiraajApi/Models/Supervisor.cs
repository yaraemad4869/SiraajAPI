using System;

namespace SiraajApi.Models
{
    public class Supervisor
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public User User { get; set; }
        public ICollection<StudentDetails> Students { get; set; }
    }
}
