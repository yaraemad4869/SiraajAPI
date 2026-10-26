using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiraajApi.DTOs;
using SiraajApi.Services.Interfaces;

namespace SiraajApi.Controllers
{
    [ApiController]
    [Route("api/analytics")]
    [Authorize]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        // GET: api/analytics/periods
        [HttpGet("periods")]
        public IActionResult GetPeriods()
        {
            var periods = new[]
            {
                new { id = "this_month", labelAr = "هذا الشهر", labelEn = "This Month" },
                new { id = "last_3_months", labelAr = "آخر ٣ أشهر", labelEn = "Last 3 Months" },
                new { id = "last_6_months", labelAr = "آخر ٦ أشهر", labelEn = "Last 6 Months" },
                new { id = "this_year", labelAr = "هذا العام", labelEn = "This Year" },
                new { id = "custom", labelAr = "مخصص", labelEn = "Custom" }
            };

            return Ok(new { success = true, data = periods });
        }

        // GET: api/analytics/owner
        [HttpGet("owner")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetOwnerDashboard([FromQuery] string period = "this_month", [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var request = new PeriodFilterRequest
                {
                    PeriodType = period,
                    StartDate = startDate,
                    EndDate = endDate
                };

                var userId = Guid.Parse(User.FindFirst("userId")?.Value ?? Guid.Empty.ToString());
                var stats = await _analyticsService.GetOwnerDashboardStats(userId, request);
                return Ok(new { success = true, data = stats });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "An error occurred while fetching owner dashboard",
                    timestamp = DateTime.UtcNow,
                    path = "/api/analytics/owner"
                });
            }
        }

        // GET: api/analytics/supervisor/{id}
        [HttpGet("supervisor/{id}")]
        [Authorize(Roles = "admin,supervisor")]
        public async Task<IActionResult> GetSupervisorDashboard(Guid id, [FromQuery] string period = "this_month", [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var request = new PeriodFilterRequest
                {
                    PeriodType = period,
                    StartDate = startDate,
                    EndDate = endDate
                };

                var stats = await _analyticsService.GetSupervisorDashboardStats(id, request);
                return Ok(new { success = true, data = stats });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "An error occurred while fetching supervisor dashboard",
                    timestamp = DateTime.UtcNow,
                    path = $"/api/analytics/supervisor/{id}"
                });
            }
        }

        // GET: api/analytics/teacher/{id}
        [HttpGet("teacher/{id}")]
        [Authorize(Roles = "admin,supervisor,teacher")]
        public async Task<IActionResult> GetTeacherDashboard(Guid id, [FromQuery] string period = "this_month", [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var request = new PeriodFilterRequest
                {
                    PeriodType = period,
                    StartDate = startDate,
                    EndDate = endDate
                };

                var stats = await _analyticsService.GetTeacherDashboardStats(id, request);
                return Ok(new { success = true, data = stats });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "An error occurred while fetching teacher dashboard",
                    timestamp = DateTime.UtcNow,
                    path = $"/api/analytics/teacher/{id}"
                });
            }
        }

        // GET: api/analytics/student/{id}/progress
        [HttpGet("student/{id}/progress")]
        [Authorize]
        public async Task<IActionResult> GetStudentProgress(Guid id, [FromQuery] string period = "this_month", [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var request = new PeriodFilterRequest
                {
                    PeriodType = period,
                    StartDate = startDate,
                    EndDate = endDate
                };

                var stats = await _analyticsService.GetStudentProgress(id, request);
                return Ok(new { success = true, data = stats });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "An error occurred while fetching student progress",
                    timestamp = DateTime.UtcNow,
                    path = $"/api/analytics/student/{id}/progress"
                });
            }
        }

        // POST: api/analytics/period-info
        [HttpPost("period-info")]
        public async Task<IActionResult> GetPeriodInfo([FromBody] PeriodFilterRequest request)
        {
            try
            {
                var info = await _analyticsService.GetPeriodInfo(request);
                return Ok(new { success = true, data = info });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "An error occurred while fetching period info",
                    timestamp = DateTime.UtcNow,
                    path = "/api/analytics/period-info"
                });
            }
        }
    }
}
