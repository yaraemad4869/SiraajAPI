using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiraajApi.Data;
using SiraajApi.DTOs;
using SiraajApi.Helpers;
using SiraajApi.Models;

namespace SiraajApi.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize] // Changed from [Authorize(Roles = "admin")] to [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ── Teachers ──
        [HttpGet("teachers")]
        public async Task<IActionResult> GetTeachers()
        {
            try
            {
                // Check if user is admin
                var userRole = User.FindFirst("role")?.Value ?? User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
                if (userRole != "admin")
                {
                    return Unauthorized(new
                    {
                        success = false,
                        statusCode = 401,
                        message = "Only admins can access this endpoint",
                        timestamp = DateTime.UtcNow,
                        path = "/api/admin/teachers"
                    });
                }

                var teachers = await _context.Users
                    .Include(u => u.Teacher)
                    .Where(u => u.Role == "teacher")
                    .Select(u => new TeacherDto
                    {
                        Id = u.Id,
                        NameAr = u.NameAr,
                        NameEn = u.NameEn,
                        Phone = u.Phone,
                        Specialization = u.Teacher != null ? u.Teacher.Specialization : "",
                        StudentIds = u.Teacher != null ? u.Teacher.StudentIds : "[]",
                        JoinDate = u.JoinDate.ToString("yyyy-MM-dd")
                    })
                    .ToListAsync();

                return Ok(new { success = true, data = teachers });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "حدث خطأ أثناء جلب المعلمين",
                    timestamp = DateTime.UtcNow,
                    path = "/api/admin/teachers"
                });
            }
        }

        [HttpPost("teachers")]
        public async Task<IActionResult> CreateTeacher([FromBody] CreateTeacherRequest request)
        {
            try
            {
                // Check if user is admin
                var userRole = User.FindFirst("role")?.Value ?? User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
                if (userRole != "admin")
                {
                    return Unauthorized(new
                    {
                        success = false,
                        statusCode = 401,
                        message = "Only admins can access this endpoint",
                        timestamp = DateTime.UtcNow,
                        path = "/api/admin/teachers"
                    });
                }

                if (await _context.Users.AnyAsync(u => u.Phone == request.Phone))
                {
                    return BadRequest(new
                    {
                        success = false,
                        statusCode = 400,
                        message = "رقم الجوال مستخدم بالفعل",
                        timestamp = DateTime.UtcNow,
                        path = "/api/admin/teachers"
                    });
                }

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    NameAr = request.NameAr,
                    NameEn = request.NameEn ?? request.NameAr,
                    Phone = request.Phone,
                    PasswordHash = PasswordHelper.HashPassword(request.Password),
                    Role = "teacher",
                    ReferralCode = $"TEACHER-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}",
                    JoinDate = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var teacher = new Teacher
                {
                    UserId = user.Id,
                    Specialization = request.Specialization,
                    StudentIds = "[]"
                };

                _context.Users.Add(user);
                _context.Teachers.Add(teacher);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    data = new TeacherDto
                    {
                        Id = user.Id,
                        NameAr = user.NameAr,
                        NameEn = user.NameEn,
                        Phone = user.Phone,
                        Specialization = teacher.Specialization,
                        JoinDate = user.JoinDate.ToString("yyyy-MM-dd")
                    },
                    message = "تم إضافة المعلم بنجاح"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "حدث خطأ أثناء إضافة المعلم",
                    timestamp = DateTime.UtcNow,
                    path = "/api/admin/teachers"
                });
            }
        }

        [HttpPut("teachers/{id}")]
        public async Task<IActionResult> UpdateTeacher(Guid id, [FromBody] UpdateTeacherRequest request)
        {
            try
            {
                // Check if user is admin
                var userRole = User.FindFirst("role")?.Value ?? User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
                if (userRole != "admin")
                {
                    return Unauthorized(new
                    {
                        success = false,
                        statusCode = 401,
                        message = "Only admins can access this endpoint",
                        timestamp = DateTime.UtcNow,
                        path = "/api/admin/teachers"
                    });
                }

                var user = await _context.Users
                    .Include(u => u.Teacher)
                    .FirstOrDefaultAsync(u => u.Id == id && u.Role == "teacher");

                if (user == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        statusCode = 404,
                        message = "المعلم غير موجود",
                        timestamp = DateTime.UtcNow,
                        path = $"/api/admin/teachers/{id}"
                    });
                }

                user.NameAr = request.NameAr;
                user.NameEn = request.NameEn ?? request.NameAr;
                user.Phone = request.Phone;
                user.UpdatedAt = DateTime.UtcNow;

                if (user.Teacher != null)
                {
                    user.Teacher.Specialization = request.Specialization;
                }

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    data = new TeacherDto
                    {
                        Id = user.Id,
                        NameAr = user.NameAr,
                        NameEn = user.NameEn,
                        Phone = user.Phone,
                        Specialization = user.Teacher?.Specialization ?? string.Empty,
                        JoinDate = user.JoinDate.ToString("yyyy-MM-dd")
                    },
                    message = "تم تحديث بيانات المعلم بنجاح"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "حدث خطأ أثناء تحديث المعلم",
                    timestamp = DateTime.UtcNow,
                    path = $"/api/admin/teachers/{id}"
                });
            }
        }

        [HttpDelete("teachers/{id}")]
        public async Task<IActionResult> DeleteTeacher(Guid id)
        {
            try
            {
                // Check if user is admin
                var userRole = User.FindFirst("role")?.Value ?? User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
                if (userRole != "admin")
                {
                    return Unauthorized(new
                    {
                        success = false,
                        statusCode = 401,
                        message = "Only admins can access this endpoint",
                        timestamp = DateTime.UtcNow,
                        path = "/api/admin/teachers"
                    });
                }

                var user = await _context.Users
                    .Include(u => u.Teacher)
                    .FirstOrDefaultAsync(u => u.Id == id && u.Role == "teacher");

                if (user == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        statusCode = 404,
                        message = "المعلم غير موجود",
                        timestamp = DateTime.UtcNow,
                        path = $"/api/admin/teachers/{id}"
                    });
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "تم حذف المعلم بنجاح"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "حدث خطأ أثناء حذف المعلم",
                    timestamp = DateTime.UtcNow,
                    path = $"/api/admin/teachers/{id}"
                });
            }
        }
    }
}
