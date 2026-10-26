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
    [Route("api/students")]
    [Authorize]
    public class StudentDetailsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/students/{id}/details
        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetStudentDetails(Guid id)
        {
            try
            {
                var details = await _context.StudentDetails
                    .Include(sd => sd.Student)
                    .ThenInclude(s => s.User)
                    .Include(sd => sd.Supervisor)
                    .ThenInclude(s => s.User)
                    .Include(sd => sd.Teacher)
                    .ThenInclude(t => t.User)
                    .FirstOrDefaultAsync(sd => sd.StudentId == id);

                if (details == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        statusCode = 404,
                        message = "Student details not found",
                        timestamp = DateTime.UtcNow,
                        path = $"/api/students/{id}/details"
                    });
                }

                var dto = MapToDto(details);

                return Ok(new { success = true, data = dto });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "An error occurred while fetching student details",
                    timestamp = DateTime.UtcNow,
                    path = $"/api/students/{id}/details"
                });
            }
        }

        // PUT: api/students/{id}/details
        [HttpPut("{id}/details")]
        [Authorize(Roles = "admin,supervisor")]
        public async Task<IActionResult> UpdateStudentDetails(Guid id, [FromBody] UpdateStudentDetailsRequest request)
        {
            try
            {
                var details = await _context.StudentDetails
                    .FirstOrDefaultAsync(sd => sd.StudentId == id);

                if (details == null)
                {
                    // Create new details if not exists
                    details = new StudentDetails
                    {
                        Id = Guid.NewGuid(),
                        StudentId = id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    _context.StudentDetails.Add(details);
                }

                // Update fields
                details.SurahsMemorized = JsonSerializer.Serialize(request.SurahsMemorized ?? new List<QuranProgressDto>());
                details.CurrentSurah = request.CurrentSurah ?? string.Empty;
                details.CurrentJuz = request.CurrentJuz;
                details.TotalVersesMemorized = request.TotalVersesMemorized;
                details.TotalSurahsCompleted = request.TotalSurahsCompleted;
                details.PreviousKnowledge = request.PreviousKnowledge ?? string.Empty;
                details.PreviousInstitution = request.PreviousInstitution ?? string.Empty;
                details.PreviousTeacher = request.PreviousTeacher ?? string.Empty;
                details.PreviousLevel = request.PreviousLevel ?? string.Empty;
                details.CurrentLevel = request.CurrentLevel ?? string.Empty;
                details.TajweedLevel = request.TajweedLevel ?? string.Empty;
                details.SpecialNeeds = request.SpecialNeeds ?? string.Empty;
                details.Goals = request.Goals ?? string.Empty;
                details.Notes = request.Notes ?? string.Empty;
                details.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var dto = await MapToDtoWithIncludes(id);

                return Ok(new
                {
                    success = true,
                    data = dto,
                    message = "Student details updated successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "An error occurred while updating student details",
                    timestamp = DateTime.UtcNow,
                    path = $"/api/students/{id}/details"
                });
            }
        }

        // POST: api/students/{id}/assign
        [HttpPost("{id}/assign")]
        [Authorize(Roles = "admin,supervisor")]
        public async Task<IActionResult> AssignStudent(Guid id, [FromBody] AssignStudentRequest request)
        {
            try
            {
                var details = await _context.StudentDetails
                    .FirstOrDefaultAsync(sd => sd.StudentId == id);

                if (details == null)
                {
                    details = new StudentDetails
                    {
                        Id = Guid.NewGuid(),
                        StudentId = id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    _context.StudentDetails.Add(details);
                }

                details.SupervisorId = request.SupervisorId;
                details.TeacherId = request.TeacherId;
                details.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "Student assigned successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "An error occurred while assigning student",
                    timestamp = DateTime.UtcNow,
                    path = $"/api/students/{id}/assign"
                });
            }
        }

        private StudentDetailsDto MapToDto(StudentDetails details)
        {
            return new StudentDetailsDto
            {
                Id = details.Id,
                StudentId = details.StudentId,
                StudentName = details.Student?.User?.NameAr ?? string.Empty,
                SurahsMemorized = JsonSerializer.Deserialize<List<QuranProgressDto>>(details.SurahsMemorized ?? "[]") ?? new List<QuranProgressDto>(),
                CurrentSurah = details.CurrentSurah,
                CurrentJuz = details.CurrentJuz,
                TotalVersesMemorized = details.TotalVersesMemorized,
                TotalSurahsCompleted = details.TotalSurahsCompleted,
                PreviousKnowledge = details.PreviousKnowledge,
                PreviousInstitution = details.PreviousInstitution,
                PreviousTeacher = details.PreviousTeacher,
                PreviousLevel = details.PreviousLevel,
                CurrentLevel = details.CurrentLevel,
                TajweedLevel = details.TajweedLevel,
                SupervisorId = details.SupervisorId,
                SupervisorName = details.Supervisor?.NameAr ?? string.Empty,
                TeacherId = details.TeacherId,
                TeacherName = details.Teacher?.User?.NameAr ?? string.Empty,
                SpecialNeeds = details.SpecialNeeds,
                Goals = details.Goals,
                Notes = details.Notes
            };
        }

        private async Task<StudentDetailsDto> MapToDtoWithIncludes(Guid studentId)
        {
            var details = await _context.StudentDetails
                .Include(sd => sd.Student)
                .ThenInclude(s => s.User)
                .Include(sd => sd.Supervisor)
                .ThenInclude(s => s.User)
                .Include(sd => sd.Teacher)
                .ThenInclude(t => t.User)
                .FirstOrDefaultAsync(sd => sd.StudentId == studentId);

            return details != null ? MapToDto(details) : new StudentDetailsDto { StudentId = studentId };
        }
    }
}
