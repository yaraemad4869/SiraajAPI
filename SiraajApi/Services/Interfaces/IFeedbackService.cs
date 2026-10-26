using System;
using System.Threading.Tasks;
using SiraajApi.DTOs;

namespace SiraajApi.Services.Interfaces
{
    public interface IFeedbackService
    {
        Task<SessionFeedbackResponse> SubmitFeedback(Guid teacherId, SessionFeedbackRequest request);
        Task<SessionFeedbackResponse> GetFeedbackById(Guid id);
        Task<SurahCompletionResponse> SubmitSurahReport(Guid teacherId, SurahCompletionRequest request);
        Task<SurahCompletionResponse> GetSurahReportById(Guid id);
        Task<StudentReportSummary> GetStudentReportSummary(Guid studentId);
        Task<byte[]> GenerateStudentReportPdf(Guid studentId);
    }
}
