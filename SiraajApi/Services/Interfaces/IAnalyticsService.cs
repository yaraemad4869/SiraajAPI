using System;
using System.Threading.Tasks;
using SiraajApi.DTOs;

namespace SiraajApi.Services.Interfaces
{
    public interface IAnalyticsService
    {
        Task<OwnerDashboardStats> GetOwnerDashboardStats(Guid? userId, PeriodFilterRequest request);
        Task<SupervisorDashboardStats> GetSupervisorDashboardStats(Guid supervisorId, PeriodFilterRequest request);
        Task<TeacherDashboardStats> GetTeacherDashboardStats(Guid teacherId, PeriodFilterRequest request);
        Task<StudentProgressData> GetStudentProgress(Guid studentId, PeriodFilterRequest request);
        Task<PeriodFilterResponse> GetPeriodInfo(PeriodFilterRequest request);
    }
}
