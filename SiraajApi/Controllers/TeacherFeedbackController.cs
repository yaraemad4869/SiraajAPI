using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiraajApi.Data;
using SiraajApi.DTOs;
using SiraajApi.Models;

namespace SiraajApi.Controllers
{
    [ApiController]
    [Route("api/teachers")]
    [Authorize]
    public class TeacherFeedbackController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TeacherFeedbackController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/teachers/feedback
        [HttpPost("feedback")]
        [Authorize(Roles = "admin,supervisor,teacher")]
        public async Task<IActionResult> SubmitFeedback([FromBody] SessionFeedbackRequest request)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst("userId")?.Value ?? Guid.Empty.ToString());
                var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.UserId == userId);

                if (teacher == null && !User.IsInRole("admin") && !User.IsInRole("supervisor"))
                {
                    return Unauthorized(new
                    {
                        success = false,
                        statusCode = 401,
                        message = "Only teachers can submit feedback",
                        timestamp = DateTime.UtcNow,
                        path = "/api/teachers/feedback"
                    });
                }

                var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == request.StudentId);

                if (student == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        statusCode = 404,
                        message = "Student not found",
                        timestamp = DateTime.UtcNow,
                        path = "/api/teachers/feedback"
                    });
                }

                var feedback = new SessionFeedback
                {
                    Id = Guid.NewGuid(),
                    StudentId = request.StudentId,
                    TeacherId = teacher?.UserId ?? userId,
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

                // Get the feedback with navigation properties
                var result = await _context.SessionFeedbacks
                    .Include(sf => sf.Student)
                    .ThenInclude(s => s.User)
                    .Include(sf => sf.Teacher)
                    .ThenInclude(t => t.User)
                    .FirstOrDefaultAsync(sf => sf.Id == feedback.Id);

                return Ok(new
                {
                    success = true,
                    data = result,
                    message = "Feedback submitted successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "An error occurred while submitting feedback",
                    timestamp = DateTime.UtcNow,
                    path = "/api/teachers/feedback"
                });
            }
        }

        // GET: api/teachers/feedback/student/{id}
        [HttpGet("feedback/student/{id}")]
        [Authorize]
        public async Task<IActionResult> GetStudentFeedback(Guid id)
        {
            try
            {
                var feedbacks = await _context.SessionFeedbacks
                    .Include(sf => sf.Student)
                    .ThenInclude(s => s.User)
                    .Include(sf => sf.Teacher)
                    .ThenInclude(t => t.User)
                    .Where(sf => sf.StudentId == id)
                    .OrderByDescending(sf => sf.SessionDate)
                    .ToListAsync();

                var result = feedbacks.Select(sf => new
                {
                    sf.Id,
                    sf.StudentId,
                    StudentName = sf.Student?.User?.NameAr ?? string.Empty,
                    sf.TeacherId,
                    TeacherName = sf.Teacher?.User?.NameAr ?? string.Empty,
                    sf.SessionDate,
                    sf.SessionDuration,
                    QuranRecitation = JsonSerializer.Deserialize<QuranRecitationDto>(sf.QuranRecitation ?? "{}"),
                    Tajweed = JsonSerializer.Deserialize<TajweedAssessmentDto>(sf.Tajweed ?? "{}"),
                    NewMemorization = JsonSerializer.Deserialize<NewMemorizationDto>(sf.NewMemorization ?? "{}"),
                    Review = JsonSerializer.Deserialize<ReviewDto>(sf.Review ?? "{}"),
                    sf.OverallRating,
                    sf.TeacherNotes,
                    sf.Homework,
                    sf.NextSessionFocus,
                    sf.CreatedAt
                }).ToList();

                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "An error occurred while fetching student feedback",
                    timestamp = DateTime.UtcNow,
                    path = $"/api/teachers/feedback/student/{id}"
                });
            }
        }

        // GET: api/teachers/feedback/student/{id}/latest
        [HttpGet("feedback/student/{id}/latest")]
        [Authorize]
        public async Task<IActionResult> GetLatestFeedback(Guid id)
        {
            try
            {
                var feedback = await _context.SessionFeedbacks
                    .Include(sf => sf.Student)
                    .ThenInclude(s => s.User)
                    .Include(sf => sf.Teacher)
                    .ThenInclude(t => t.User)
                    .Where(sf => sf.StudentId == id)
                    .OrderByDescending(sf => sf.SessionDate)
                    .FirstOrDefaultAsync();

                if (feedback == null)
                {
                    return Ok(new { success = true, data = (object?)null, message = "No feedback found" });
                }

                return Ok(new
                {
                    success = true,
                    data = new
                    {
                        feedback.Id,
                        feedback.StudentId,
                        StudentName = feedback.Student?.User?.NameAr ?? string.Empty,
                        feedback.TeacherId,
                        TeacherName = feedback.Teacher?.User?.NameAr ?? string.Empty,
                        feedback.SessionDate,
                        feedback.SessionDuration,
                        QuranRecitation = JsonSerializer.Deserialize<QuranRecitationDto>(feedback.QuranRecitation ?? "{}"),
                        Tajweed = JsonSerializer.Deserialize<TajweedAssessmentDto>(feedback.Tajweed ?? "{}"),
                        NewMemorization = JsonSerializer.Deserialize<NewMemorizationDto>(feedback.NewMemorization ?? "{}"),
                        Review = JsonSerializer.Deserialize<ReviewDto>(feedback.Review ?? "{}"),
                        feedback.OverallRating,
                        feedback.TeacherNotes,
                        feedback.Homework,
                        feedback.NextSessionFocus,
                        feedback.CreatedAt
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "An error occurred while fetching latest feedback",
                    timestamp = DateTime.UtcNow,
                    path = $"/api/teachers/feedback/student/{id}/latest"
                });
            }
        }

        // GET: api/teachers/feedback/{id}
        [HttpGet("feedback/{id}")]
        [Authorize]
        public async Task<IActionResult> GetFeedbackById(Guid id)
        {
            try
            {
                var feedback = await _context.SessionFeedbacks
                    .Include(sf => sf.Student)
                    .ThenInclude(s => s.User)
                    .Include(sf => sf.Teacher)
                    .ThenInclude(t => t.User)
                    .FirstOrDefaultAsync(sf => sf.Id == id);

                if (feedback == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        statusCode = 404,
                        message = "Feedback not found",
                        timestamp = DateTime.UtcNow,
                        path = $"/api/teachers/feedback/{id}"
                    });
                }

                return Ok(new
                {
                    success = true,
                    data = new
                    {
                        feedback.Id,
                        feedback.StudentId,
                        StudentName = feedback.Student?.User?.NameAr ?? string.Empty,
                        feedback.TeacherId,
                        TeacherName = feedback.Teacher?.User?.NameAr ?? string.Empty,
                        feedback.SessionDate,
                        feedback.SessionDuration,
                        QuranRecitation = JsonSerializer.Deserialize<QuranRecitationDto>(feedback.QuranRecitation ?? "{}"),
                        Tajweed = JsonSerializer.Deserialize<TajweedAssessmentDto>(feedback.Tajweed ?? "{}"),
                        NewMemorization = JsonSerializer.Deserialize<NewMemorizationDto>(feedback.NewMemorization ?? "{}"),
                        Review = JsonSerializer.Deserialize<ReviewDto>(feedback.Review ?? "{}"),
                        feedback.OverallRating,
                        feedback.TeacherNotes,
                        feedback.Homework,
                        feedback.NextSessionFocus,
                        feedback.CreatedAt
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "An error occurred while fetching feedback",
                    timestamp = DateTime.UtcNow,
                    path = $"/api/teachers/feedback/{id}"
                });
            }
        }

        // POST: api/teachers/reports/surah-completion
        [HttpPost("reports/surah-completion")]
        [Authorize(Roles = "admin,supervisor,teacher")]
        public async Task<IActionResult> SubmitSurahReport([FromBody] SurahCompletionRequest request)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst("userId")?.Value ?? Guid.Empty.ToString());
                var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.UserId == userId);

                if (teacher == null && !User.IsInRole("admin") && !User.IsInRole("supervisor"))
                {
                    return Unauthorized(new
                    {
                        success = false,
                        statusCode = 401,
                        message = "Only teachers can submit surah reports",
                        timestamp = DateTime.UtcNow,
                        path = "/api/teachers/reports/surah-completion"
                    });
                }

                var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == request.StudentId);

                if (student == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        statusCode = 404,
                        message = "Student not found",
                        timestamp = DateTime.UtcNow,
                        path = "/api/teachers/reports/surah-completion"
                    });
                }

                var report = new SurahCompletionReport
                {
                    Id = Guid.NewGuid(),
                    StudentId = request.StudentId,
                    TeacherId = teacher?.UserId ?? userId,
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

                return Ok(new
                {
                    success = true,
                    data = report,
                    message = "Surah completion report submitted successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "An error occurred while submitting surah report",
                    timestamp = DateTime.UtcNow,
                    path = "/api/teachers/reports/surah-completion"
                });
            }
        }

        // GET: api/teachers/reports/student/{id}
        [HttpGet("reports/student/{id}")]
        [Authorize]
        public async Task<IActionResult> GetStudentSurahReports(Guid id)
        {
            try
            {
                var reports = await _context.SurahCompletionReports
                    .Include(sc => sc.Student)
                    .ThenInclude(s => s.User)
                    .Include(sc => sc.Teacher)
                    .ThenInclude(t => t.User)
                    .Where(sc => sc.StudentId == id)
                    .OrderByDescending(sc => sc.CompletedAt)
                    .ToListAsync();

                var result = reports.Select(sc => new
                {
                    sc.Id,
                    sc.StudentId,
                    StudentName = sc.Student?.User?.NameAr ?? string.Empty,
                    sc.TeacherId,
                    TeacherName = sc.Teacher?.User?.NameAr ?? string.Empty,
                    sc.SurahName,
                    sc.SurahNumber,
                    sc.CompletionType,
                    sc.VersesCount,
                    sc.VersesStart,
                    sc.VersesEnd,
                    sc.MemorizationAccuracy,
                    sc.TajweedAccuracy,
                    sc.Fluency,
                    Strengths = JsonSerializer.Deserialize<List<string>>(sc.Strengths ?? "[]"),
                    Weaknesses = JsonSerializer.Deserialize<List<string>>(sc.Weaknesses ?? "[]"),
                    Recommendations = JsonSerializer.Deserialize<List<string>>(sc.Recommendations ?? "[]"),
                    sc.TeacherNotes,
                    sc.StartedAt,
                    sc.CompletedAt,
                    sc.LastReviewDate,
                    sc.NextSurahRecommended,
                    sc.ReviewFrequency,
                    sc.CreatedAt
                }).ToList();

                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "An error occurred while fetching surah reports",
                    timestamp = DateTime.UtcNow,
                    path = $"/api/teachers/reports/student/{id}"
                });
            }
        }

        // GET: api/teachers/reports/student/{id}/summary
        [HttpGet("reports/student/{id}/summary")]
        [Authorize]
        public async Task<IActionResult> GetStudentReportSummary(Guid id)
        {
            try
            {
                var studentDetails = await _context.StudentDetails
                    .Include(sd => sd.Student)
                    .ThenInclude(s => s.User)
                    .FirstOrDefaultAsync(sd => sd.StudentId == id);

                if (studentDetails == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        statusCode = 404,
                        message = "Student not found",
                        timestamp = DateTime.UtcNow,
                        path = $"/api/teachers/reports/student/{id}/summary"
                    });
                }

                var feedback = await _context.SessionFeedbacks
                    .Where(sf => sf.StudentId == id)
                    .ToListAsync();

                var surahs = await _context.SurahCompletionReports
                    .Where(sc => sc.StudentId == id)
                    .OrderByDescending(sc => sc.CompletedAt)
                    .ToListAsync();

                var surahProgress = JsonSerializer.Deserialize<List<QuranProgressDto>>(
                    studentDetails.SurahsMemorized ?? "[]") ?? new List<QuranProgressDto>();

                var summary = new
                {
                    StudentId = id,
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
                    SurahsCompleted = surahs.Select(s => new
                    {
                        s.SurahName,
                        s.SurahNumber,
                        s.CompletionType,
                        s.CompletedAt,
                        s.MemorizationAccuracy,
                        s.TajweedAccuracy,
                        s.Fluency
                    }).ToList(),
                    RecentFeedback = feedback
                        .OrderByDescending(f => f.SessionDate)
                        .Take(5)
                        .Select(f => new
                        {
                            f.SessionDate,
                            f.OverallRating,
                            f.TeacherNotes
                        })
                        .ToList(),
                    SurahProgress = surahProgress
                };

                return Ok(new { success = true, data = summary });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "An error occurred while fetching student summary",
                    timestamp = DateTime.UtcNow,
                    path = $"/api/teachers/reports/student/{id}/summary"
                });
            }
        }

        // GET: api/teachers/reports/surah/{surah_id}
        [HttpGet("reports/surah/{surah_id}")]
        [Authorize]
        public async Task<IActionResult> GetSurahReference(int surah_id)
        {
            try
            {
                var surah = await _context.QuranSurahs
                    .FirstOrDefaultAsync(s => s.Number == surah_id);

                if (surah == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        statusCode = 404,
                        message = "Surah not found",
                        timestamp = DateTime.UtcNow,
                        path = $"/api/teachers/reports/surah/{surah_id}"
                    });
                }

                return Ok(new
                {
                    success = true,
                    data = new
                    {
                        surah.Id,
                        surah.Number,
                        surah.NameAr,
                        surah.NameEn,
                        surah.VersesCount,
                        surah.JuzNumber
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "An error occurred while fetching surah reference",
                    timestamp = DateTime.UtcNow,
                    path = $"/api/teachers/reports/surah/{surah_id}"
                });
            }
        }
    }
}
