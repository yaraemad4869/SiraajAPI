using System;
using System.Collections.Generic;

namespace SiraajApi.DTOs
{
    // ============================================
    // SURAH COMPLETION REQUEST
    // ============================================
    public class SurahCompletionRequest
    {
        public Guid StudentId { get; set; }
        public string SurahName { get; set; } = string.Empty;
        public int SurahNumber { get; set; }
        public string CompletionType { get; set; } = string.Empty; // quarter, half, complete
        public int VersesCount { get; set; }
        public int VersesStart { get; set; }
        public int VersesEnd { get; set; }
        public int MemorizationAccuracy { get; set; } // 1-5
        public int TajweedAccuracy { get; set; } // 1-5
        public int Fluency { get; set; } // 1-5
        public List<string> Strengths { get; set; } = new();
        public List<string> Weaknesses { get; set; } = new();
        public List<string> Recommendations { get; set; } = new();
        public string TeacherNotes { get; set; } = string.Empty;
        public DateTime StartedAt { get; set; }
        public DateTime CompletedAt { get; set; }
        public string NextSurahRecommended { get; set; } = string.Empty;
        public string ReviewFrequency { get; set; } = string.Empty; // daily, weekly, biweekly
    }

    // ============================================
    // SURAH COMPLETION RESPONSE
    // ============================================
    public class SurahCompletionResponse
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public Guid TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public string SurahName { get; set; } = string.Empty;
        public int SurahNumber { get; set; }
        public string CompletionType { get; set; } = string.Empty;
        public int VersesCount { get; set; }
        public int VersesStart { get; set; }
        public int VersesEnd { get; set; }
        public int MemorizationAccuracy { get; set; }
        public int TajweedAccuracy { get; set; }
        public int Fluency { get; set; }
        public List<string> Strengths { get; set; } = new();
        public List<string> Weaknesses { get; set; } = new();
        public List<string> Recommendations { get; set; } = new();
        public string TeacherNotes { get; set; } = string.Empty;
        public DateTime StartedAt { get; set; }
        public DateTime CompletedAt { get; set; }
        public DateTime LastReviewDate { get; set; }
        public string NextSurahRecommended { get; set; } = string.Empty;
        public string ReviewFrequency { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    // ============================================
    // STUDENT REPORT SUMMARY
    // ============================================
    public class StudentReportSummary
    {
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public int TotalSessions { get; set; }
        public int TotalSurahsCompleted { get; set; }
        public int TotalVersesMemorized { get; set; }
        public double AverageRating { get; set; }
        public double MemorizationAccuracy { get; set; }
        public double TajweedAccuracy { get; set; }
        public double Fluency { get; set; }
        public string CurrentLevel { get; set; } = string.Empty;
        public string TajweedLevel { get; set; } = string.Empty;
        public List<SurahCompletionResponse> SurahsCompleted { get; set; } = new();
        public List<SessionFeedbackResponse> RecentFeedback { get; set; } = new();
        public List<QuranProgressDto> SurahProgress { get; set; } = new(); // Uses QuranProgressDto from StudentDetailsDtos.cs
    }
}
