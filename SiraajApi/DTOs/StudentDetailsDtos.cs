using System;
using System.Collections.Generic;

namespace SiraajApi.DTOs
{
    // ============================================
    // STUDENT DETAILS DTOs
    // ============================================
    public class StudentDetailsDto
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        
        // Quran Memorization
        public List<QuranProgressDto> SurahsMemorized { get; set; } = new();
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
        public string SupervisorName { get; set; } = string.Empty;
        public Guid? TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        
        // Additional
        public string SpecialNeeds { get; set; } = string.Empty;
        public string Goals { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }

    public class QuranProgressDto
    {
        public string SurahName { get; set; } = string.Empty;
        public int SurahNumber { get; set; }
        public int VersesMemorized { get; set; }
        public int TotalVerses { get; set; }
        public string Status { get; set; } = string.Empty; // not_started, in_progress, completed, reviewing
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string MemorizationLevel { get; set; } = string.Empty; // weak, medium, strong, mastered
        public string Notes { get; set; } = string.Empty;
    }

    public class UpdateStudentDetailsRequest
    {
        // Quran Memorization
        public List<QuranProgressDto> SurahsMemorized { get; set; } = new();
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
        
        // Additional
        public string SpecialNeeds { get; set; } = string.Empty;
        public string Goals { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }

    public class AssignStudentRequest
    {
        public Guid? SupervisorId { get; set; }
        public Guid? TeacherId { get; set; }
    }

    // ============================================
    // SUPERVISOR DTOs
    // ============================================
    public class CreateSupervisorRequest
    {
        public Guid UserId { get; set; }
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }

    public class UpdateSupervisorRequest
    {
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }

    // ============================================
    // STUDENT UPDATE DTO
    // ============================================
    public class UpdateStudentRequest
    {
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string? Phone { get; set; }
        public string? PackageId { get; set; }
        public string? Status { get; set; } // active, pending, inactive
    }
}
