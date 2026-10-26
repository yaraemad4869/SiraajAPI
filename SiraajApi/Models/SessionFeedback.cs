using System;

namespace SiraajApi.Models
{
    public class SessionFeedback
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid TeacherId { get; set; }
        public DateTime SessionDate { get; set; }
        public int SessionDuration { get; set; } // in minutes
        
        // Quran Recitation (JSON)
        public string QuranRecitation { get; set; } = "{}";
        
        // Tajweed Assessment (JSON)
        public string Tajweed { get; set; } = "{}";
        
        // New Memorization (JSON)
        public string NewMemorization { get; set; } = "{}";
        
        // Review (JSON)
        public string Review { get; set; } = "{}";
        
        public int OverallRating { get; set; } // 1-5
        public string TeacherNotes { get; set; } = string.Empty;
        public string Homework { get; set; } = string.Empty;
        public string NextSessionFocus { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public Student Student { get; set; }
        public Teacher Teacher { get; set; }
    }
}
