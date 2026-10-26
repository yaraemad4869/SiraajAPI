using System;
using System.Text;
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
    public class AnalyticsExportController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsExportController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        // GET: api/analytics/owner/export
        [HttpGet("owner/export")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ExportOwnerDashboard([FromQuery] string period = "this_month")
        {
            try
            {
                var request = new PeriodFilterRequest { PeriodType = period };
                var stats = await _analyticsService.GetOwnerDashboardStats(null, request);
                
                var csv = new StringBuilder();
                csv.AppendLine("Owner Dashboard Report");
                csv.AppendLine($"Period: {period}");
                csv.AppendLine($"Generated: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}");
                csv.AppendLine();
                csv.AppendLine("Metric,Value");
                csv.AppendLine($"Total Students,{stats.TotalStudents}");
                csv.AppendLine($"Active Students,{stats.ActiveStudents}");
                csv.AppendLine($"Total Teachers,{stats.TotalTeachers}");
                csv.AppendLine($"Total Supervisors,{stats.TotalSupervisors}");
                csv.AppendLine($"Total Revenue (SAR),{stats.TotalRevenue}");
                csv.AppendLine($"Revenue Growth (%),{stats.RevenueGrowth}");
                csv.AppendLine($"Retention Rate (%),{stats.RetentionRate}");
                csv.AppendLine($"Avg Sessions/Student,{stats.AverageSessionsPerStudent}");
                csv.AppendLine($"Avg Student Rating,{stats.AverageStudentRating}");
                csv.AppendLine($"Completion Rate (%),{stats.CompletionRate}");
                csv.AppendLine();
                
                csv.AppendLine("Revenue Data:");
                csv.AppendLine("Period,Amount (SAR),Count");
                foreach (var data in stats.RevenueData)
                {
                    csv.AppendLine($"{data.Period},{data.Amount},{data.Count}");
                }
                
                csv.AppendLine();
                csv.AppendLine("Student Growth:");
                csv.AppendLine("Period,Total,New");
                foreach (var data in stats.StudentGrowthData)
                {
                    csv.AppendLine($"{data.Period},{data.Total},{data.New}");
                }

                return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", $"Owner-Dashboard-{period}-{DateTime.UtcNow:yyyyMMdd}.csv");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "An error occurred while exporting owner dashboard",
                    timestamp = DateTime.UtcNow,
                    path = "/api/analytics/owner/export"
                });
            }
        }

        // GET: api/analytics/supervisor/export
        [HttpGet("supervisor/export")]
        [Authorize(Roles = "admin,supervisor")]
        public async Task<IActionResult> ExportSupervisorDashboard([FromQuery] string period = "this_month")
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst("userId")?.Value ?? Guid.Empty.ToString());
                var request = new PeriodFilterRequest { PeriodType = period };
                var stats = await _analyticsService.GetSupervisorDashboardStats(userId, request);
                
                var csv = new StringBuilder();
                csv.AppendLine("Supervisor Dashboard Report");
                csv.AppendLine($"Generated: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}");
                csv.AppendLine();
                csv.AppendLine("Metric,Value");
                csv.AppendLine($"Supervised Teachers,{stats.SupervisedTeachers}");
                csv.AppendLine($"Supervised Students,{stats.SupervisedStudents}");
                csv.AppendLine($"Completion Rate (%),{stats.CompletionRate}");
                csv.AppendLine($"Average Teacher Rating,{stats.AverageTeacherRating}");
                csv.AppendLine($"Students with Progress,{stats.StudentsWithProgress}");
                csv.AppendLine($"Students Needing Attention,{stats.StudentsNeedingAttention}");
                
                return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", $"Supervisor-Dashboard-{period}-{DateTime.UtcNow:yyyyMMdd}.csv");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "An error occurred while exporting supervisor dashboard",
                    timestamp = DateTime.UtcNow,
                    path = "/api/analytics/supervisor/export"
                });
            }
        }
    }
}
