using System;
using System.Linq;
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
    [Route("api/supervisors")]
    [Authorize]
    public class SupervisorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SupervisorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/supervisors/students
        [HttpGet("students")]
        [Authorize(Roles = "admin,supervisor")]
        public async Task<IActionResult> GetSupervisedStudents()
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst("userId")?.Value ?? Guid.Empty.ToString());
                var supervisor = await _context.Supervisors.FirstOrDefaultAsync(s => s.UserId == userId);

                IQueryable<StudentDetails> query = _context.StudentDetails
                    .Include(sd => sd.Student)
                    .ThenInclude(s => s.User)
                    .Include(sd => sd.Teacher)
                    .ThenInclude(t => t.User);

                if (!User.IsInRole("admin") && supervisor != null)
                {
                    query = query.Where(sd => sd.SupervisorId == supervisor.Id);
                }

                var students = await query
                    .Select(sd => new
                    {
                        sd.Id,
                        sd.StudentId,
                        StudentName = sd.Student.User.NameAr,
                        sd.CurrentLevel,
                        sd.TajweedLevel,
                        sd.TotalSurahsCompleted,
                        sd.TotalVersesMemorized,
                        TeacherName = sd.Teacher.User.NameAr,
                        sd.CreatedAt,
                        sd.UpdatedAt
                    })
                    .ToListAsync();

                return Ok(new { success = true, data = students });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "An error occurred while fetching supervised students",
                    timestamp = DateTime.UtcNow,
                    path = "/api/supervisors/students"
                });
            }
        }

        // GET: api/supervisors/students/{id}
        [HttpGet("students/{id}")]
        [Authorize(Roles = "admin,supervisor")]
        public async Task<IActionResult> GetStudentDetails(Guid id)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst("userId")?.Value ?? Guid.Empty.ToString());
                var supervisor = await _context.Supervisors.FirstOrDefaultAsync(s => s.UserId == userId);

                var studentDetails = await _context.StudentDetails
                    .Include(sd => sd.Student)
                    .ThenInclude(s => s.User)
                    .Include(sd => sd.Teacher)
                    .ThenInclude(t => t.User)
                    .Include(sd => sd.Supervisor)
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
                        path = $"/api/supervisors/students/{id}"
                    });
                }

                // Check authorization
                if (!User.IsInRole("admin") && studentDetails.SupervisorId != supervisor?.Id)
                {
                    return Unauthorized(new
                    {
                        success = false,
                        statusCode = 401,
                        message = "You are not authorized to view this student",
                        timestamp = DateTime.UtcNow,
                        path = $"/api/supervisors/students/{id}"
                    });
                }

                return Ok(new
                {
                    success = true,
                    data = new
                    {
                        studentDetails.Id,
                        studentDetails.StudentId,
                        StudentName = studentDetails.Student.User.NameAr,
                        StudentPhone = studentDetails.Student.User.Phone,
                        studentDetails.CurrentLevel,
                        studentDetails.TajweedLevel,
                        studentDetails.TotalSurahsCompleted,
                        studentDetails.TotalVersesMemorized,
                        studentDetails.CurrentSurah,
                        studentDetails.CurrentJuz,
                        studentDetails.PreviousKnowledge,
                        studentDetails.PreviousInstitution,
                        studentDetails.PreviousTeacher,
                        studentDetails.PreviousLevel,
                        studentDetails.SpecialNeeds,
                        studentDetails.Goals,
                        studentDetails.Notes,
                        SupervisorName = studentDetails.Supervisor?.NameAr ?? string.Empty,
                        TeacherName = studentDetails.Teacher?.User?.NameAr ?? string.Empty,
                        studentDetails.CreatedAt,
                        studentDetails.UpdatedAt
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "An error occurred while fetching student details",
                    timestamp = DateTime.UtcNow,
                    path = $"/api/supervisors/students/{id}"
                });
            }
        }

        // POST: api/supervisors/students/{id}/assign
        [HttpPost("students/{id}/assign")]
        [Authorize(Roles = "admin,supervisor")]
        public async Task<IActionResult> AssignStudent(Guid id, [FromBody] AssignStudentRequest request)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst("userId")?.Value ?? Guid.Empty.ToString());
                var supervisor = await _context.Supervisors.FirstOrDefaultAsync(s => s.UserId == userId);

                var studentDetails = await _context.StudentDetails
                    .FirstOrDefaultAsync(sd => sd.StudentId == id);

                if (studentDetails == null)
                {
                    studentDetails = new StudentDetails
                    {
                        Id = Guid.NewGuid(),
                        StudentId = id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    _context.StudentDetails.Add(studentDetails);
                }

                // Check authorization
                if (!User.IsInRole("admin"))
                {
                    if (request.SupervisorId.HasValue && request.SupervisorId.Value != supervisor?.Id)
                    {
                        return Unauthorized(new
                        {
                            success = false,
                            statusCode = 401,
                            message = "You can only assign students to yourself",
                            timestamp = DateTime.UtcNow,
                            path = $"/api/supervisors/students/{id}/assign"
                        });
                    }

                    if (studentDetails.SupervisorId != supervisor?.Id && studentDetails.SupervisorId.HasValue)
                    {
                        return Unauthorized(new
                        {
                            success = false,
                            statusCode = 401,
                            message = "You are not authorized to assign this student",
                            timestamp = DateTime.UtcNow,
                            path = $"/api/supervisors/students/{id}/assign"
                        });
                    }
                }

                studentDetails.SupervisorId = request.SupervisorId;
                studentDetails.TeacherId = request.TeacherId;
                studentDetails.UpdatedAt = DateTime.UtcNow;

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
                    path = $"/api/supervisors/students/{id}/assign"
                });
            }
        }

        // PUT: api/supervisors/students/{id}/profile
        [HttpPut("students/{id}/profile")]
        [Authorize(Roles = "admin,supervisor")]
        public async Task<IActionResult> UpdateStudentProfile(Guid id, [FromBody] UpdateStudentDetailsRequest request)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst("userId")?.Value ?? Guid.Empty.ToString());
                var supervisor = await _context.Supervisors.FirstOrDefaultAsync(s => s.UserId == userId);

                var studentDetails = await _context.StudentDetails
                    .FirstOrDefaultAsync(sd => sd.StudentId == id);

                if (studentDetails == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        statusCode = 404,
                        message = "Student not found",
                        timestamp = DateTime.UtcNow,
                        path = $"/api/supervisors/students/{id}/profile"
                    });
                }

                // Check authorization
                if (!User.IsInRole("admin") && studentDetails.SupervisorId != supervisor?.Id)
                {
                    return Unauthorized(new
                    {
                        success = false,
                        statusCode = 401,
                        message = "You are not authorized to update this student",
                        timestamp = DateTime.UtcNow,
                        path = $"/api/supervisors/students/{id}/profile"
                    });
                }

                // Update fields
                studentDetails.CurrentLevel = request.CurrentLevel ?? studentDetails.CurrentLevel;
                studentDetails.TajweedLevel = request.TajweedLevel ?? studentDetails.TajweedLevel;
                studentDetails.CurrentSurah = request.CurrentSurah ?? studentDetails.CurrentSurah;
                studentDetails.CurrentJuz = request.CurrentJuz;
                studentDetails.TotalVersesMemorized = request.TotalVersesMemorized;
                studentDetails.TotalSurahsCompleted = request.TotalSurahsCompleted;
                studentDetails.PreviousKnowledge = request.PreviousKnowledge ?? studentDetails.PreviousKnowledge;
                studentDetails.PreviousInstitution = request.PreviousInstitution ?? studentDetails.PreviousInstitution;
                studentDetails.PreviousTeacher = request.PreviousTeacher ?? studentDetails.PreviousTeacher;
                studentDetails.PreviousLevel = request.PreviousLevel ?? studentDetails.PreviousLevel;
                studentDetails.SpecialNeeds = request.SpecialNeeds ?? studentDetails.SpecialNeeds;
                studentDetails.Goals = request.Goals ?? studentDetails.Goals;
                studentDetails.Notes = request.Notes ?? studentDetails.Notes;
                studentDetails.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "Student profile updated successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "An error occurred while updating student profile",
                    timestamp = DateTime.UtcNow,
                    path = $"/api/supervisors/students/{id}/profile"
                });
            }
        }

        // POST: api/supervisors
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateSupervisor([FromBody] CreateSupervisorRequest request)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);

                if (user == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        statusCode = 404,
                        message = "User not found",
                        timestamp = DateTime.UtcNow,
                        path = "/api/supervisors"
                    });
                }

                var existing = await _context.Supervisors.FirstOrDefaultAsync(s => s.UserId == request.UserId);

                if (existing != null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        statusCode = 400,
                        message = "User is already a supervisor",
                        timestamp = DateTime.UtcNow,
                        path = "/api/supervisors"
                    });
                }

                var supervisor = new Supervisor
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    NameAr = request.NameAr ?? user.NameAr,
                    NameEn = request.NameEn ?? user.NameEn,
                    Email = request.Email ?? user.Phone,
                    Phone = request.Phone ?? user.Phone,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Supervisors.Add(supervisor);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    data = supervisor,
                    message = "Supervisor created successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "An error occurred while creating supervisor",
                    timestamp = DateTime.UtcNow,
                    path = "/api/supervisors"
                });
            }
        }
    }
}
