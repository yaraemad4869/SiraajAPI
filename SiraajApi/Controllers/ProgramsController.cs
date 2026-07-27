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
    [Route("api/programs")]
    public class ProgramsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProgramsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/programs
        [HttpGet]
        public async Task<IActionResult> GetPrograms()
        {
            try
            {
                var programs = await _context.Programs
                    .Where(p => p.IsActive)
                    .Select(p => new ProgramDto
                    {
                        Id = p.Id,
                        Icon = p.Icon,
                        NameAr = p.NameAr,
                        NameEn = p.NameEn,
                        DescAr = p.DescAr,
                        DescEn = p.DescEn,
                        IsActive = p.IsActive
                    })
                    .ToListAsync();

                return Ok(new { success = true, data = programs });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "حدث خطأ أثناء جلب البرامج",
                    timestamp = DateTime.UtcNow,
                    path = "/api/programs"
                });
            }
        }

        // GET: api/programs/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProgram(Guid id)
        {
            try
            {
                var program = await _context.Programs
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (program == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        statusCode = 404,
                        message = "البرنامج غير موجود",
                        timestamp = DateTime.UtcNow,
                        path = $"/api/programs/{id}"
                    });
                }

                return Ok(new
                {
                    success = true,
                    data = new ProgramDto
                    {
                        Id = program.Id,
                        Icon = program.Icon,
                        NameAr = program.NameAr,
                        NameEn = program.NameEn,
                        DescAr = program.DescAr,
                        DescEn = program.DescEn,
                        IsActive = program.IsActive
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "حدث خطأ أثناء جلب البرنامج",
                    timestamp = DateTime.UtcNow,
                    path = $"/api/programs/{id}"
                });
            }
        }

        // POST: api/programs (Admin only)
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateProgram([FromBody] CreateProgramRequest request)
        {
            try
            {
                var program = new QuranProgram
                {
                    Id = Guid.NewGuid(),
                    Icon = request.Icon,
                    NameAr = request.NameAr,
                    NameEn = request.NameEn,
                    DescAr = request.DescAr,
                    DescEn = request.DescEn,
                    IsActive = request.IsActive,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Programs.Add(program);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    data = new ProgramDto
                    {
                        Id = program.Id,
                        Icon = program.Icon,
                        NameAr = program.NameAr,
                        NameEn = program.NameEn,
                        DescAr = program.DescAr,
                        DescEn = program.DescEn,
                        IsActive = program.IsActive
                    },
                    message = "تم إضافة البرنامج بنجاح"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "حدث خطأ أثناء إضافة البرنامج",
                    timestamp = DateTime.UtcNow,
                    path = "/api/programs"
                });
            }
        }

        // PUT: api/programs/{id} (Admin only)
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateProgram(Guid id, [FromBody] UpdateProgramRequest request)
        {
            try
            {
                var program = await _context.Programs
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (program == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        statusCode = 404,
                        message = "البرنامج غير موجود",
                        timestamp = DateTime.UtcNow,
                        path = $"/api/programs/{id}"
                    });
                }

                program.Icon = request.Icon;
                program.NameAr = request.NameAr;
                program.NameEn = request.NameEn;
                program.DescAr = request.DescAr;
                program.DescEn = request.DescEn;
                program.IsActive = request.IsActive;
                program.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    data = new ProgramDto
                    {
                        Id = program.Id,
                        Icon = program.Icon,
                        NameAr = program.NameAr,
                        NameEn = program.NameEn,
                        DescAr = program.DescAr,
                        DescEn = program.DescEn,
                        IsActive = program.IsActive
                    },
                    message = "تم تحديث البرنامج بنجاح"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "حدث خطأ أثناء تحديث البرنامج",
                    timestamp = DateTime.UtcNow,
                    path = $"/api/programs/{id}"
                });
            }
        }

        // DELETE: api/programs/{id} (Admin only)
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteProgram(Guid id)
        {
            try
            {
                var program = await _context.Programs
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (program == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        statusCode = 404,
                        message = "البرنامج غير موجود",
                        timestamp = DateTime.UtcNow,
                        path = $"/api/programs/{id}"
                    });
                }

                // Soft delete - just deactivate
                program.IsActive = false;
                program.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "تم حذف البرنامج بنجاح"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "حدث خطأ أثناء حذف البرنامج",
                    timestamp = DateTime.UtcNow,
                    path = $"/api/programs/{id}"
                });
            }
        }
    }
}
