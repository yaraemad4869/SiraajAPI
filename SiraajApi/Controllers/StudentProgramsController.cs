using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiraajApi.Data;
using SiraajApi.Models;

namespace SiraajApi.Controllers
{
    [ApiController]
    [Route("api/student-programs")]
    [Authorize]
    public class StudentProgramsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentProgramsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStudentProgramRequest request)
        {
            try
            {
                // Check if student exists
                var student = await _context.Students.FindAsync(request.StudentId);
                if (student == null)
                {
                    return NotFound(new { success = false, message = "Student not found" });
                }

                // Check if program exists
                var program = await _context.Programs.FindAsync(request.ProgramId);
                if (program == null)
                {
                    return NotFound(new { success = false, message = "Program not found" });
                }

                // Check if package exists
                var package = await _context.Packages.FindAsync(request.PackageId);
                if (package == null)
                {
                    return NotFound(new { success = false, message = "Package not found" });
                }

                var studentProgram = new StudentProgram
                {
                    Id = Guid.NewGuid(),
                    StudentId = request.StudentId,
                    ProgramId = request.ProgramId,
                    PackageId = request.PackageId,
                    EnrolledDate = DateTime.UtcNow,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.StudentPrograms.Add(studentProgram);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    data = new { id = studentProgram.Id },
                    message = "Student program created successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }

    public class CreateStudentProgramRequest
    {
        public Guid StudentId { get; set; }
        public Guid ProgramId { get; set; }
        public Guid PackageId { get; set; }
    }
}
