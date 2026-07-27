using System;

namespace SiraajApi.Models
{
    public class Schedule
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public string Day { get; set; } = string.Empty; // sat, sun, mon, tue, wed, thu
        public string Time { get; set; } = string.Empty; // 09:00, 10:00, etc.
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation property
        public Student Student { get; set; }
    }
}
