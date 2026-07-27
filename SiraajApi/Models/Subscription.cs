using System;

namespace SiraajApi.Models
{
    public class Subscription
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid PackageId { get; set; }
        public Guid ProgramId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;
        public bool AutoRenew { get; set; }
        public string Status { get; set; } = "active"; // active, expired, cancelled
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Student Student { get; set; }
        public Package Package { get; set; }
        public QuranProgram Program { get; set; }
    }
}
