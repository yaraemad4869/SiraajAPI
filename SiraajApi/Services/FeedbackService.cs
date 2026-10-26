using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SiraajApi.Data;
using SiraajApi.DTOs;
using SiraajApi.Models;
using SiraajApi.Services.Interfaces;

namespace SiraajApi.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly ApplicationDbContext _context;

        public FeedbackService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SessionFeedbackResponse> SubmitFeedback(Guid teacherId, SessionFeedbackRequest request)
        {
            var feedback = new SessionFeedback
            {
                Id = Guid.NewGuid(),
                StudentId = request.StudentId,
                TeacherId = teacherId,
                SessionDate = request.SessionDate,
                SessionDuration = request.SessionDuration,
                QuranRecitation = JsonSerializer.Serialize(request.QuranRecitation),
                Tajweed = JsonSerializer.Serialize(request.Tajweed),
                NewMemorization = JsonSerializer.Serialize(request.NewMemorization),
                Review = JsonSerializer.Serialize(request.Review),
                OverallRating = request.OverallRating,
                TeacherNotes = request.TeacherNotes,
                Homework = request.Homework,
                NextSessionFocus = request.NextSessionFocus,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.SessionFeedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            return await GetFeedbackById(feedback.Id);
        }

        public async Task<SessionFeedbackResponse> GetFeedbackById(Guid id)
        {
            var feedback = await _context.SessionFeedbacks
                .Include(sf => sf.Student)
                .ThenInclude(s => s.User)
                .Include(sf => sf.Teacher)
                .ThenInclude(t => t.User)
                .FirstOrDefaultAsync(sf => sf.Id == id);

            if (feedback == null) return null;

            return new SessionFeedbackResponse
            {
                Id = feedback.Id,
                StudentId = feedback.StudentId,
                StudentName = feedback.Student?.User?.NameAr ?? string.Empty,
                TeacherId = feedback.TeacherId,
                TeacherName = feedback.Teacher?.User?.NameAr ?? string.Empty,
                SessionDate = feedback.SessionDate,
                SessionDuration = feedback.SessionDuration,
                QuranRecitation = JsonSerializer.Deserialize<QuranRecitationDto>(feedback.QuranRecitation ?? "{}"),
                Tajweed = JsonSerializer.Deserialize<TajweedAssessmentDto>(feedback.Tajweed ?? "{}"),
                NewMemorization = JsonSerializer.Deserialize<NewMemorizationDto>(feedback.NewMemorization ?? "{}"),
                Review = JsonSerializer.Deserialize<ReviewDto>(feedback.Review ?? "{}"),
                OverallRating = feedback.OverallRating,
                TeacherNotes = feedback.TeacherNotes,
                Homework = feedback.Homework,
                NextSessionFocus = feedback.NextSessionFocus,
                CreatedAt = feedback.CreatedAt
            };
        }

        public async Task<SurahCompletionResponse> SubmitSurahReport(Guid teacherId, SurahCompletionRequest request)
        {
            var report = new SurahCompletionReport
            {
                Id = Guid.NewGuid(),
                StudentId = request.StudentId,
                TeacherId = teacherId,
                SurahName = request.SurahName,
                SurahNumber = request.SurahNumber,
                CompletionType = request.CompletionType,
                VersesCount = request.VersesCount,
                VersesStart = request.VersesStart,
                VersesEnd = request.VersesEnd,
                MemorizationAccuracy = request.MemorizationAccuracy,
                TajweedAccuracy = request.TajweedAccuracy,
                Fluency = request.Fluency,
                Strengths = JsonSerializer.Serialize(request.Strengths ?? new List<string>()),
                Weaknesses = JsonSerializer.Serialize(request.Weaknesses ?? new List<string>()),
                Recommendations = JsonSerializer.Serialize(request.Recommendations ?? new List<string>()),
                TeacherNotes = request.TeacherNotes,
                StartedAt = request.StartedAt,
                CompletedAt = request.CompletedAt,
                LastReviewDate = DateTime.UtcNow,
                NextSurahRecommended = request.NextSurahRecommended,
                ReviewFrequency = request.ReviewFrequency,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Update student details
            var studentDetails = await _context.StudentDetails
                .FirstOrDefaultAsync(sd => sd.StudentId == request.StudentId);

            if (studentDetails != null)
            {
                var surahs = JsonSerializer.Deserialize<List<QuranProgressDto>>(
                    studentDetails.SurahsMemorized ?? "[]") ?? new List<QuranProgressDto>();

                var existing = surahs.FirstOrDefault(s => s.SurahName == request.SurahName);
                if (existing != null)
                {
                    existing.Status = "completed";
                    existing.CompletedAt = request.CompletedAt;
                    existing.VersesMemorized = request.VersesCount;
                }
                else
                {
                    surahs.Add(new QuranProgressDto
                    {
                        SurahName = request.SurahName,
                        SurahNumber = request.SurahNumber,
                        VersesMemorized = request.VersesCount,
                        TotalVerses = request.VersesCount,
                        Status = "completed",
                        CompletedAt = request.CompletedAt
                    });
                }

                studentDetails.SurahsMemorized = JsonSerializer.Serialize(surahs);
                studentDetails.TotalSurahsCompleted = surahs.Count(s => s.Status == "completed");
                studentDetails.TotalVersesMemorized = surahs.Sum(s => s.VersesMemorized);
                studentDetails.UpdatedAt = DateTime.UtcNow;
            }

            _context.SurahCompletionReports.Add(report);
            await _context.SaveChangesAsync();

            return await GetSurahReportById(report.Id);
        }

        public async Task<SurahCompletionResponse> GetSurahReportById(Guid id)
        {
            var report = await _context.SurahCompletionReports
                .Include(sc => sc.Student)
                .ThenInclude(s => s.User)
                .Include(sc => sc.Teacher)
                .ThenInclude(t => t.User)
                .FirstOrDefaultAsync(sc => sc.Id == id);

            if (report == null) return null;

            return new SurahCompletionResponse
            {
                Id = report.Id,
                StudentId = report.StudentId,
                StudentName = report.Student?.User?.NameAr ?? string.Empty,
                TeacherId = report.TeacherId,
                TeacherName = report.Teacher?.User?.NameAr ?? string.Empty,
                SurahName = report.SurahName,
                SurahNumber = report.SurahNumber,
                CompletionType = report.CompletionType,
                VersesCount = report.VersesCount,
                VersesStart = report.VersesStart,
                VersesEnd = report.VersesEnd,
                MemorizationAccuracy = report.MemorizationAccuracy,
                TajweedAccuracy = report.TajweedAccuracy,
                Fluency = report.Fluency,
                Strengths = JsonSerializer.Deserialize<List<string>>(report.Strengths ?? "[]"),
                Weaknesses = JsonSerializer.Deserialize<List<string>>(report.Weaknesses ?? "[]"),
                Recommendations = JsonSerializer.Deserialize<List<string>>(report.Recommendations ?? "[]"),
                TeacherNotes = report.TeacherNotes,
                StartedAt = report.StartedAt,
                CompletedAt = report.CompletedAt,
                LastReviewDate = report.LastReviewDate,
                NextSurahRecommended = report.NextSurahRecommended,
                ReviewFrequency = report.ReviewFrequency,
                CreatedAt = report.CreatedAt
            };
        }

        public async Task<StudentReportSummary> GetStudentReportSummary(Guid studentId)
        {
            var studentDetails = await _context.StudentDetails
                .Include(sd => sd.Student)
                .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(sd => sd.StudentId == studentId);

            if (studentDetails == null) return null;

            var feedback = await _context.SessionFeedbacks
                .Where(sf => sf.StudentId == studentId)
                .ToListAsync();

            var surahs = await _context.SurahCompletionReports
                .Where(sc => sc.StudentId == studentId)
                .OrderByDescending(sc => sc.CompletedAt)
                .ToListAsync();

            var surahProgress = JsonSerializer.Deserialize<List<QuranProgressDto>>(
                studentDetails.SurahsMemorized ?? "[]") ?? new List<QuranProgressDto>();

            return new StudentReportSummary
            {
                StudentId = studentId,
                StudentName = studentDetails.Student?.User?.NameAr ?? string.Empty,
                TotalSessions = feedback.Count,
                TotalSurahsCompleted = surahs.Count,
                TotalVersesMemorized = studentDetails.TotalVersesMemorized,
                AverageRating = feedback.Count > 0 ? Math.Round(feedback.Average(f => f.OverallRating), 2) : 0,
                MemorizationAccuracy = surahs.Count > 0 ? Math.Round(surahs.Average(s => s.MemorizationAccuracy), 2) : 0,
                TajweedAccuracy = surahs.Count > 0 ? Math.Round(surahs.Average(s => s.TajweedAccuracy), 2) : 0,
                Fluency = surahs.Count > 0 ? Math.Round(surahs.Average(s => s.Fluency), 2) : 0,
                CurrentLevel = studentDetails.CurrentLevel,
                TajweedLevel = studentDetails.TajweedLevel,
                SurahsCompleted = surahs.Select(s => new SurahCompletionResponse
                {
                    Id = s.Id,
                    SurahName = s.SurahName,
                    SurahNumber = s.SurahNumber,
                    CompletionType = s.CompletionType,
                    VersesCount = s.VersesCount,
                    MemorizationAccuracy = s.MemorizationAccuracy,
                    TajweedAccuracy = s.TajweedAccuracy,
                    Fluency = s.Fluency,
                    CompletedAt = s.CompletedAt
                }).ToList(),
                RecentFeedback = feedback
                    .OrderByDescending(f => f.SessionDate)
                    .Take(5)
                    .Select(f => new SessionFeedbackResponse
                    {
                        Id = f.Id,
                        SessionDate = f.SessionDate,
                        OverallRating = f.OverallRating,
                        TeacherNotes = f.TeacherNotes,
                        Homework = f.Homework
                    }).ToList(),
                SurahProgress = surahProgress
            };
        }

        public async Task<byte[]> GenerateStudentReportPdf(Guid studentId)
        {
            // This is a placeholder for PDF generation
            // In production, use a library like iTextSharp or QuestPDF
            await Task.CompletedTask;
            return Array.Empty<byte>();
        }
    }
}
