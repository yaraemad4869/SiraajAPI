using System;

namespace SiraajApi.Models
{
    public class SurahCompletionReport
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid TeacherId { get; set; }
        
        public string SurahName { get; set; } = string.Empty;
        public int SurahNumber { get; set; }
        
        public string CompletionType { get; set; } = string.Empty; // quarter, half, complete
        public int VersesCount { get; set; }
        public int VersesStart { get; set; }
        public int VersesEnd { get; set; }
        
        public int MemorizationAccuracy { get; set; } // 1-5
        public int TajweedAccuracy { get; set; } // 1-5
        public int Fluency { get; set; } // 1-5
        
        public string Strengths { get; set; } = "[]"; // JSON array
        public string Weaknesses { get; set; } = "[]"; // JSON array
        public string Recommendations { get; set; } = "[]"; // JSON array
        
        public string TeacherNotes { get; set; } = string.Empty;
        public DateTime StartedAt { get; set; }
        public DateTime CompletedAt { get; set; }
        public DateTime LastReviewDate { get; set; }
        
        public string NextSurahRecommended { get; set; } = string.Empty;
        public string ReviewFrequency { get; set; } = string.Empty; // daily, weekly, biweekly
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public Student Student { get; set; }
        public Teacher Teacher { get; set; }
    }
}
