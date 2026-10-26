using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiraajApi.Services.Interfaces;

namespace SiraajApi.Controllers
{
    [ApiController]
    [Route("api/reports")]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public ReportsController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        // GET: api/reports/student/{id}
        [HttpGet("student/{id}")]
        [Authorize]
        public async Task<IActionResult> GetStudentReport(Guid id)
        {
            try
            {
                var report = await _feedbackService.GetStudentReportSummary(id);
                if (report == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        statusCode = 404,
                        message = "Student report not found",
                        timestamp = DateTime.UtcNow,
                        path = $"/api/reports/student/{id}"
                    });
                }

                return Ok(new { success = true, data = report });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "An error occurred while fetching student report",
                    timestamp = DateTime.UtcNow,
                    path = $"/api/reports/student/{id}"
                });
            }
        }

        // GET: api/reports/student/{id}/pdf
        [HttpGet("student/{id}/pdf")]
        [Authorize]
        public async Task<IActionResult> GetStudentReportPdf(Guid id)
        {
            try
            {
                var pdf = await _feedbackService.GenerateStudentReportPdf(id);
                if (pdf == null || pdf.Length == 0)
                {
                    return NotFound(new
                    {
                        success = false,
                        statusCode = 404,
                        message = "PDF report not found",
                        timestamp = DateTime.UtcNow,
                        path = $"/api/reports/student/{id}/pdf"
                    });
                }

                return File(pdf, "application/pdf", $"Student-Report-{id}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "An error occurred while generating PDF",
                    timestamp = DateTime.UtcNow,
                    path = $"/api/reports/student/{id}/pdf"
                });
            }
        }

        // GET: api/reports/student/{id}/summary
        [HttpGet("student/{id}/summary")]
        [Authorize]
        public async Task<IActionResult> GetStudentSummary(Guid id)
        {
            try
            {
                var summary = await _feedbackService.GetStudentReportSummary(id);
                if (summary == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        statusCode = 404,
                        message = "Student summary not found",
                        timestamp = DateTime.UtcNow,
                        path = $"/api/reports/student/{id}/summary"
                    });
                }

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
                    path = $"/api/reports/student/{id}/summary"
                });
            }
        }
    }
}
