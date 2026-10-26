using System;
using System.Collections.Generic;

namespace SiraajApi.DTOs
{
    public class SessionFeedbackRequest
    {
        public Guid StudentId { get; set; }
        public DateTime SessionDate { get; set; }
        public int SessionDuration { get; set; }
        
        // Quran Recitation
        public QuranRecitationDto QuranRecitation { get; set; } = new();
        
        // Tajweed Assessment
        public TajweedAssessmentDto Tajweed { get; set; } = new();
        
        // New Memorization
        public NewMemorizationDto NewMemorization { get; set; } = new();
        
        // Review
        public ReviewDto Review { get; set; } = new();
        
        public int OverallRating { get; set; }
        public string TeacherNotes { get; set; } = string.Empty;
        public string Homework { get; set; } = string.Empty;
        public string NextSessionFocus { get; set; } = string.Empty;
    }

    public class QuranRecitationDto
    {
        public int Accuracy { get; set; } // 1-5
        public int Fluency { get; set; } // 1-5
        public int TajweedApplication { get; set; } // 1-5
        public int MemorizationQuality { get; set; } // 1-5
        public string Comments { get; set; } = string.Empty;
    }

    public class TajweedAssessmentDto
    {
        public int Makharij { get; set; } // 1-5
        public int Sifat { get; set; } // 1-5
        public int Ghunna { get; set; } // 1-5
        public int Idgham { get; set; } // 1-5
        public int Iqlab { get; set; } // 1-5
        public int Ikhfa { get; set; } // 1-5
        public int StoppingRules { get; set; } // 1-5
        public string Comments { get; set; } = string.Empty;
    }

    public class NewMemorizationDto
    {
        public string SurahName { get; set; } = string.Empty;
        public int VersesCount { get; set; }
        public int VersesStart { get; set; }
        public int VersesEnd { get; set; }
        public int Quality { get; set; } // 1-5
        public string Comments { get; set; } = string.Empty;
    }

    public class ReviewDto
    {
        public string SurahName { get; set; } = string.Empty;
        public int VersesReviewed { get; set; }
        public int Quality { get; set; } // 1-5
        public string Comments { get; set; } = string.Empty;
    }

    public class SessionFeedbackResponse
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public Guid TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public DateTime SessionDate { get; set; }
        public int SessionDuration { get; set; }
        public QuranRecitationDto QuranRecitation { get; set; } = new();
        public TajweedAssessmentDto Tajweed { get; set; } = new();
        public NewMemorizationDto NewMemorization { get; set; } = new();
        public ReviewDto Review { get; set; } = new();
        public int OverallRating { get; set; }
        public string TeacherNotes { get; set; } = string.Empty;
        public string Homework { get; set; } = string.Empty;
        public string NextSessionFocus { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
