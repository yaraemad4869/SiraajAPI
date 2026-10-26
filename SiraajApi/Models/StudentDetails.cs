using System;
using System.Collections.Generic;

namespace SiraajApi.Models
{
    public class StudentDetails
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        
        // Quran Memorization
        public string SurahsMemorized { get; set; } = "[]"; // JSON array
        public string CurrentSurah { get; set; } = string.Empty;
        public int CurrentJuz { get; set; }
        public int TotalVersesMemorized { get; set; }
        public int TotalSurahsCompleted { get; set; }
        
        // Previous Knowledge
        public string PreviousKnowledge { get; set; } = string.Empty;
        public string PreviousInstitution { get; set; } = string.Empty;
        public string PreviousTeacher { get; set; } = string.Empty;
        public string PreviousLevel { get; set; } = string.Empty;
        
        // Current Level
        public string CurrentLevel { get; set; } = string.Empty;
        public string TajweedLevel { get; set; } = string.Empty;
        
        // Supervision
        public Guid? SupervisorId { get; set; }
        public Guid? TeacherId { get; set; }
        
        // Additional
        public string SpecialNeeds { get; set; } = string.Empty;
        public string Goals { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public Student Student { get; set; }
        public Supervisor Supervisor { get; set; }
        public Teacher Teacher { get; set; }
    }
}
