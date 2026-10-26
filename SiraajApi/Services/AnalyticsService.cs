using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SiraajApi.Data;
using SiraajApi.DTOs;
using SiraajApi.Models;
using SiraajApi.Services.Interfaces;

namespace SiraajApi.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly ApplicationDbContext _context;

        public AnalyticsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OwnerDashboardStats> GetOwnerDashboardStats(Guid? userId, PeriodFilterRequest request)
        {
            var period = await GetPeriodInfo(request);
            var startDate = period.StartDate;
            var endDate = period.EndDate;

            // Get total students
            var totalStudents = await _context.Students.CountAsync();
            var activeStudents = await _context.Students
                .CountAsync(s => s.Status == "active");

            // Get teachers
            var totalTeachers = await _context.Users
                .CountAsync(u => u.Role == "teacher");

            // Get supervisors
            var totalSupervisors = await _context.Supervisors.CountAsync();

            // Get revenue from completed payments in period
            var payments = await _context.Payments
                .Where(p => p.Status == "completed" && p.PaymentDate >= startDate && p.PaymentDate <= endDate)
                .ToListAsync();

            var totalRevenue = payments.Sum(p => p.FinalAmount);
            var revenueGrowth = CalculateRevenueGrowth(startDate, endDate);

            // Get sessions in period
            var sessions = await _context.SessionFeedbacks
                .Where(sf => sf.SessionDate >= startDate && sf.SessionDate <= endDate)
                .ToListAsync();

            // Calculate retention rate
            var retentionRate = await CalculateRetentionRate(startDate, endDate);

            // Get teacher performance
            var teacherPerformance = await GetTeacherPerformanceData(startDate, endDate);

            // Revenue data for charts
            var revenueData = await GetRevenueData(startDate, endDate);

            // Student growth data - FIXED: Use User.JoinDate instead of Student.CreatedAt
            var studentGrowthData = await GetStudentGrowthData(startDate, endDate);

            return new OwnerDashboardStats
            {
                TotalStudents = totalStudents,
                ActiveStudents = activeStudents,
                TotalTeachers = totalTeachers,
                TotalSupervisors = totalSupervisors,
                TotalRevenue = totalRevenue,
                RevenueGrowth = revenueGrowth,
                RetentionRate = retentionRate,
                AverageSessionsPerStudent = totalStudents > 0 ? Math.Round((double)sessions.Count / totalStudents, 2) : 0,
                AverageStudentRating = sessions.Count > 0 ? Math.Round(sessions.Average(s => s.OverallRating), 2) : 0,
                CompletionRate = await CalculateCompletionRate(startDate, endDate),
                RevenueData = revenueData,
                StudentGrowthData = studentGrowthData,
                TeacherPerformance = teacherPerformance
            };
        }

        public async Task<SupervisorDashboardStats> GetSupervisorDashboardStats(Guid supervisorId, PeriodFilterRequest request)
        {
            var period = await GetPeriodInfo(request);
            var startDate = period.StartDate;
            var endDate = period.EndDate;

            // Get supervised students
            var studentDetails = await _context.StudentDetails
                .Where(sd => sd.SupervisorId == supervisorId)
                .ToListAsync();

            var studentIds = studentDetails.Select(sd => sd.StudentId).ToList();

            // Get supervised teachers
            var teachers = await _context.Teachers
                .Where(t => studentDetails.Any(sd => sd.TeacherId == t.UserId))
                .ToListAsync();

            var supervisedStudents = studentIds.Count;
            var supervisedTeachers = teachers.Count;

            // Get sessions for supervised students
            var sessions = await _context.SessionFeedbacks
                .Where(sf => studentIds.Contains(sf.StudentId) && sf.SessionDate >= startDate && sf.SessionDate <= endDate)
                .ToListAsync();

            // Calculate completion rate
            var completionRate = await CalculateCompletionRate(startDate, endDate, studentIds);

            // Teacher performance
            var teacherPerformance = await GetTeacherPerformanceData(startDate, endDate, studentIds);

            // Students with progress
            var studentsWithProgress = studentDetails.Count(sd => sd.TotalSurahsCompleted > 0);

            // Students needing attention (no progress in last 30 days)
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
            var studentsNeedingAttention = studentDetails
                .Count(sd => !_context.SessionFeedbacks
                    .Any(sf => sf.StudentId == sd.StudentId && sf.SessionDate >= thirtyDaysAgo));

            return new SupervisorDashboardStats
            {
                SupervisedTeachers = supervisedTeachers,
                SupervisedStudents = supervisedStudents,
                CompletionRate = completionRate,
                AverageTeacherRating = sessions.Count > 0 ? Math.Round(sessions.Average(s => s.OverallRating), 2) : 0,
                TeacherPerformance = teacherPerformance,
                StudentsWithProgress = studentsWithProgress,
                StudentsNeedingAttention = studentsNeedingAttention
            };
        }

        public async Task<TeacherDashboardStats> GetTeacherDashboardStats(Guid teacherId, PeriodFilterRequest request)
        {
            var period = await GetPeriodInfo(request);
            var startDate = period.StartDate;
            var endDate = period.EndDate;

            // Get assigned students
            var studentDetails = await _context.StudentDetails
                .Where(sd => sd.TeacherId == teacherId)
                .ToListAsync();

            var studentIds = studentDetails.Select(sd => sd.StudentId).ToList();
            var assignedStudents = studentIds.Count;

            // Get sessions
            var sessions = await _context.SessionFeedbacks
                .Where(sf => sf.TeacherId == teacherId && sf.SessionDate >= startDate && sf.SessionDate <= endDate)
                .ToListAsync();

            // Get completed surahs
            var completedSurahs = await _context.SurahCompletionReports
                .Where(sc => sc.TeacherId == teacherId && sc.CompletedAt >= startDate && sc.CompletedAt <= endDate)
                .ToListAsync();

            // Student progress
            var studentProgress = await GetStudentProgressData(teacherId, studentIds, startDate, endDate);

            // Recent sessions (last 5)
            var recentSessions = sessions
                .OrderByDescending(s => s.SessionDate)
                .Take(5)
                .Select(s => MapToFeedbackResponse(s))
                .ToList();

            return new TeacherDashboardStats
            {
                AssignedStudents = assignedStudents,
                TotalSessions = sessions.Count,
                AverageRating = sessions.Count > 0 ? Math.Round(sessions.Average(s => s.OverallRating), 2) : 0,
                CompletedSurahs = completedSurahs.Count,
                TotalVersesMemorized = studentDetails.Sum(sd => sd.TotalVersesMemorized),
                StudentProgress = studentProgress,
                RecentSessions = recentSessions
            };
        }

        public async Task<StudentProgressData> GetStudentProgress(Guid studentId, PeriodFilterRequest request)
        {
            var period = await GetPeriodInfo(request);
            var startDate = period.StartDate;
            var endDate = period.EndDate;

            var sessions = await _context.SessionFeedbacks
                .Where(sf => sf.StudentId == studentId && sf.SessionDate >= startDate && sf.SessionDate <= endDate)
                .ToListAsync();

            var completedSurahs = await _context.SurahCompletionReports
                .Where(sc => sc.StudentId == studentId && sc.CompletedAt >= startDate && sc.CompletedAt <= endDate)
                .ToListAsync();

            var studentDetails = await _context.StudentDetails
                .FirstOrDefaultAsync(sd => sd.StudentId == studentId);

            return new StudentProgressData
            {
                StudentId = studentId,
                StudentName = await GetStudentName(studentId),
                TotalSessions = sessions.Count,
                CompletedSurahs = completedSurahs.Count,
                MemorizedVerses = studentDetails?.TotalVersesMemorized ?? 0,
                AverageScore = sessions.Count > 0 ? Math.Round(sessions.Average(s => s.OverallRating), 2) : 0
            };
        }

        public async Task<PeriodFilterResponse> GetPeriodInfo(PeriodFilterRequest request)
        {
            var now = DateTime.UtcNow;
            DateTime startDate;
            DateTime endDate = now;

            switch (request.PeriodType)
            {
                case "this_month":
                    startDate = new DateTime(now.Year, now.Month, 1);
                    endDate = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));
                    break;
                case "last_3_months":
                    startDate = new DateTime(now.Year, now.Month, 1).AddMonths(-3);
                    endDate = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));
                    break;
                case "last_6_months":
                    startDate = new DateTime(now.Year, now.Month, 1).AddMonths(-6);
                    endDate = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));
                    break;
                case "this_year":
                    startDate = new DateTime(now.Year, 1, 1);
                    endDate = new DateTime(now.Year, 12, 31);
                    break;
                case "custom":
                    startDate = request.StartDate ?? now.AddMonths(-1);
                    endDate = request.EndDate ?? now;
                    break;
                default:
                    startDate = new DateTime(now.Year, now.Month, 1);
                    break;
            }

            return new PeriodFilterResponse
            {
                PeriodType = request.PeriodType,
                LabelAr = GetPeriodLabelAr(request.PeriodType),
                LabelEn = GetPeriodLabelEn(request.PeriodType),
                StartDate = startDate,
                EndDate = endDate
            };
        }

        // Helper Methods
        private async Task<double> CalculateCompletionRate(DateTime startDate, DateTime endDate, List<Guid>? studentIds = null)
        {
            var query = _context.SurahCompletionReports.AsQueryable();
            if (studentIds != null && studentIds.Any())
            {
                query = query.Where(sc => studentIds.Contains(sc.StudentId));
            }

            var completed = await query
                .Where(sc => sc.CompletedAt >= startDate && sc.CompletedAt <= endDate)
                .CountAsync();

            var total = await _context.Students.CountAsync();
            if (studentIds != null && studentIds.Any())
            {
                total = studentIds.Count;
            }

            return total > 0 ? Math.Round((double)completed / total * 100, 2) : 0;
        }

        private async Task<double> CalculateRetentionRate(DateTime startDate, DateTime endDate)
        {
            var previousPeriodStart = startDate.AddMonths(-1);
            var previousPeriodEnd = startDate.AddDays(-1);

            // Get students who joined before the previous period
            var previousActive = await _context.Users
                .Where(u => u.Role == "student" && u.JoinDate <= previousPeriodEnd)
                .CountAsync();

            // Get students who are active in the current period (have sessions or are active)
            var currentActive = await _context.Users
                .Where(u => u.Role == "student" && u.JoinDate >= startDate && u.JoinDate <= endDate)
                .CountAsync();

            return previousActive > 0 ? Math.Round((double)currentActive / previousActive * 100, 2) : 0;
        }

        private decimal CalculateRevenueGrowth(DateTime startDate, DateTime endDate)
        {
            var previousPeriodStart = startDate.AddMonths(-1);
            var previousPeriodEnd = startDate.AddDays(-1);

            var currentRevenue = _context.Payments
                .Where(p => p.Status == "completed" && p.PaymentDate >= startDate && p.PaymentDate <= endDate)
                .Sum(p => p.FinalAmount);

            var previousRevenue = _context.Payments
                .Where(p => p.Status == "completed" && p.PaymentDate >= previousPeriodStart && p.PaymentDate <= previousPeriodEnd)
                .Sum(p => p.FinalAmount);

            return previousRevenue > 0 ? Math.Round((currentRevenue - previousRevenue) / previousRevenue * 100, 2) : 0;
        }

        private async Task<List<TeacherPerformanceData>> GetTeacherPerformanceData(DateTime startDate, DateTime endDate, List<Guid>? studentIds = null)
        {
            var teachers = await _context.Teachers
                .Include(t => t.User)
                .ToListAsync();

            var result = new List<TeacherPerformanceData>();

            foreach (var teacher in teachers)
            {
                // Get teacher's students
                var teacherStudents = await _context.StudentDetails
                    .Where(sd => sd.TeacherId == teacher.UserId)
                    .Select(sd => sd.StudentId)
                    .ToListAsync();

                if (studentIds != null && studentIds.Any())
                {
                    teacherStudents = teacherStudents.Where(id => studentIds.Contains(id)).ToList();
                }

                if (!teacherStudents.Any()) continue;

                // Get sessions
                var sessions = await _context.SessionFeedbacks
                    .Where(sf => sf.TeacherId == teacher.UserId && sf.SessionDate >= startDate && sf.SessionDate <= endDate)
                    .ToListAsync();

                // Get completed surahs
                var completedSurahs = await _context.SurahCompletionReports
                    .Where(sc => sc.TeacherId == teacher.UserId && sc.CompletedAt >= startDate && sc.CompletedAt <= endDate)
                    .CountAsync();

                var completionRate = teacherStudents.Count > 0 ? Math.Round((double)completedSurahs / teacherStudents.Count * 100, 2) : 0;

                result.Add(new TeacherPerformanceData
                {
                    TeacherId = teacher.UserId,
                    TeacherName = teacher.User?.NameAr ?? string.Empty,
                    StudentsCount = teacherStudents.Count,
                    SessionsCount = sessions.Count,
                    AverageRating = sessions.Count > 0 ? Math.Round(sessions.Average(s => s.OverallRating), 2) : 0,
                    CompletionRate = completionRate
                });
            }

            return result.OrderByDescending(t => t.AverageRating).ToList();
        }

        private async Task<List<RevenueDataPoint>> GetRevenueData(DateTime startDate, DateTime endDate)
        {
            var payments = await _context.Payments
                .Where(p => p.Status == "completed" && p.PaymentDate >= startDate && p.PaymentDate <= endDate)
                .ToListAsync();

            var grouped = payments
                .GroupBy(p => new { p.PaymentDate.Year, p.PaymentDate.Month })
                .Select(g => new RevenueDataPoint
                {
                    Period = $"{g.Key.Year}-{g.Key.Month:D2}",
                    Amount = g.Sum(p => p.FinalAmount),
                    Count = g.Count()
                })
                .OrderBy(r => r.Period)
                .ToList();

            return grouped;
        }

        private async Task<List<StudentGrowthDataPoint>> GetStudentGrowthData(DateTime startDate, DateTime endDate)
        {
            // Use User.JoinDate since Student doesn't have CreatedAt
            var students = await _context.Students
                .Include(s => s.User)
                .Where(s => s.User.JoinDate >= startDate && s.User.JoinDate <= endDate)
                .ToListAsync();

            var grouped = students
                .GroupBy(s => new { s.User.JoinDate.Year, s.User.JoinDate.Month })
                .Select(g => new StudentGrowthDataPoint
                {
                    Period = $"{g.Key.Year}-{g.Key.Month:D2}",
                    Total = g.Count(),
                    New = g.Count()
                })
                .OrderBy(r => r.Period)
                .ToList();

            return grouped;
        }

        private async Task<List<StudentProgressData>> GetStudentProgressData(Guid teacherId, List<Guid> studentIds, DateTime startDate, DateTime endDate)
        {
            var result = new List<StudentProgressData>();

            foreach (var studentId in studentIds)
            {
                var sessions = await _context.SessionFeedbacks
                    .Where(sf => sf.StudentId == studentId && sf.SessionDate >= startDate && sf.SessionDate <= endDate)
                    .ToListAsync();

                var completedSurahs = await _context.SurahCompletionReports
                    .Where(sc => sc.StudentId == studentId && sc.CompletedAt >= startDate && sc.CompletedAt <= endDate)
                    .CountAsync();

                var details = await _context.StudentDetails
                    .FirstOrDefaultAsync(sd => sd.StudentId == studentId);

                result.Add(new StudentProgressData
                {
                    StudentId = studentId,
                    StudentName = await GetStudentName(studentId),
                    TotalSessions = sessions.Count,
                    CompletedSurahs = completedSurahs,
                    MemorizedVerses = details?.TotalVersesMemorized ?? 0,
                    AverageScore = sessions.Count > 0 ? Math.Round(sessions.Average(s => s.OverallRating), 2) : 0
                });
            }

            return result;
        }

        private async Task<string> GetStudentName(Guid studentId)
        {
            var student = await _context.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.UserId == studentId);

            return student?.User?.NameAr ?? string.Empty;
        }

        private SessionFeedbackResponse MapToFeedbackResponse(SessionFeedback feedback)
        {
            return new SessionFeedbackResponse
            {
                Id = feedback.Id,
                StudentId = feedback.StudentId,
                SessionDate = feedback.SessionDate,
                SessionDuration = feedback.SessionDuration,
                OverallRating = feedback.OverallRating,
                TeacherNotes = feedback.TeacherNotes,
                Homework = feedback.Homework,
                NextSessionFocus = feedback.NextSessionFocus,
                CreatedAt = feedback.CreatedAt
            };
        }

        private string GetPeriodLabelAr(string periodType)
        {
            return periodType switch
            {
                "this_month" => "هذا الشهر",
                "last_3_months" => "آخر ٣ أشهر",
                "last_6_months" => "آخر ٦ أشهر",
                "this_year" => "هذا العام",
                "custom" => "مخصص",
                _ => "هذا الشهر"
            };
        }

        private string GetPeriodLabelEn(string periodType)
        {
            return periodType switch
            {
                "this_month" => "This Month",
                "last_3_months" => "Last 3 Months",
                "last_6_months" => "Last 6 Months",
                "this_year" => "This Year",
                "custom" => "Custom",
                _ => "This Month"
            };
        }
    }
}
