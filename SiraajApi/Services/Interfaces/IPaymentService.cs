using System.Threading.Tasks;
using SiraajApi.DTOs;

namespace SiraajApi.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponse> ProcessPayment(InitiatePaymentRequest request);
        Task<bool> VerifyPayment(string transactionId);
        Task<PaymentResponse> HandleCallback(PaymentCallbackRequest request);
    }
}
