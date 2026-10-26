using System;
using System.Collections.Generic;

namespace SiraajApi.DTOs
{
    public class PeriodFilterRequest
    {
        public string PeriodType { get; set; } = "this_month"; // this_month, last_3_months, last_6_months, this_year, custom
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class PeriodFilterResponse
    {
        public string PeriodType { get; set; } = string.Empty;
        public string LabelAr { get; set; } = string.Empty;
        public string LabelEn { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class OwnerDashboardStats
    {
        // Overview
        public int TotalStudents { get; set; }
        public int ActiveStudents { get; set; }
        public int TotalTeachers { get; set; }
        public int TotalSupervisors { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal RevenueGrowth { get; set; }
        
        // Retention
        public double RetentionRate { get; set; }
        public double AverageSessionsPerStudent { get; set; }
        
        // Performance
        public double AverageStudentRating { get; set; }
        public double CompletionRate { get; set; }
        
        // Charts Data
        public List<RevenueDataPoint> RevenueData { get; set; } = new();
        public List<StudentGrowthDataPoint> StudentGrowthData { get; set; } = new();
        public List<TeacherPerformanceData> TeacherPerformance { get; set; } = new();
    }

    public class SupervisorDashboardStats
    {
        // Overview
        public int SupervisedTeachers { get; set; }
        public int SupervisedStudents { get; set; }
        public double CompletionRate { get; set; }
        public double AverageTeacherRating { get; set; }
        
        // Teacher Performance
        public List<TeacherPerformanceData> TeacherPerformance { get; set; } = new();
        
        // Student Stats
        public int StudentsWithProgress { get; set; }
        public int StudentsNeedingAttention { get; set; }
    }

    public class TeacherDashboardStats
    {
        // Overview
        public int AssignedStudents { get; set; }
        public int TotalSessions { get; set; }
        public double AverageRating { get; set; }
        public int CompletedSurahs { get; set; }
        public int TotalVersesMemorized { get; set; }
        
        // Student Progress
        public List<StudentProgressData> StudentProgress { get; set; } = new();
        
        // Recent Activity
        public List<SessionFeedbackResponse> RecentSessions { get; set; } = new();
    }

    public class RevenueDataPoint
    {
        public string Period { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public int Count { get; set; }
    }

    public class StudentGrowthDataPoint
    {
        public string Period { get; set; } = string.Empty;
        public int Total { get; set; }
        public int New { get; set; }
    }

    public class TeacherPerformanceData
    {
        public Guid TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public int StudentsCount { get; set; }
        public int SessionsCount { get; set; }
        public double AverageRating { get; set; }
        public double CompletionRate { get; set; }
    }

    public class StudentProgressData
    {
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public int TotalSessions { get; set; }
        public int CompletedSurahs { get; set; }
        public int MemorizedVerses { get; set; }
        public double AverageScore { get; set; }
    }
}
