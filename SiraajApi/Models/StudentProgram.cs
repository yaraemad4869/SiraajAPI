using System;

namespace SiraajApi.Models
{
    public class StudentProgram
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid ProgramId { get; set; }
        public Guid PackageId { get; set; }
        public DateTime EnrolledDate { get; set; } = DateTime.UtcNow;
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public Student Student { get; set; }
        public QuranProgram Program { get; set; }
        public Package Package { get; set; }
    }
}
