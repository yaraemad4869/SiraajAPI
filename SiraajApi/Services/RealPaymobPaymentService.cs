using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SiraajApi.DTOs;
using SiraajApi.Helpers;
using SiraajApi.Services.Interfaces;

namespace SiraajApi.Services
{
    public class RealPaymobPaymentService : IPaymentService
    {
        private readonly PaymobSettings _paymobSettings;
        private readonly HttpClient _httpClient;
        private readonly ILogger<RealPaymobPaymentService> _logger;

        public RealPaymobPaymentService(
            IOptions<PaymobSettings> paymobSettings,
            HttpClient httpClient,
            ILogger<RealPaymobPaymentService> logger)
        {
            _paymobSettings = paymobSettings.Value;
            _httpClient = httpClient;
            _logger = logger;
            _httpClient.BaseAddress = new Uri(_paymobSettings.BaseUrl);
        }

        public async Task<PaymentResponse> ProcessPayment(InitiatePaymentRequest request)
        {
            try
            {
                var authToken = await GetAuthToken();
                if (string.IsNullOrEmpty(authToken))
                    throw new Exception("Failed to get Paymob auth token");

                var orderId = await CreateOrder(authToken, request);
                if (string.IsNullOrEmpty(orderId))
                    throw new Exception("Failed to create Paymob order");

                var paymentKey = await GetPaymentKey(authToken, orderId, request);
                if (string.IsNullOrEmpty(paymentKey))
                    throw new Exception("Failed to get Paymob payment key");

                return new PaymentResponse
                {
                    PaymentId = Guid.NewGuid(),
                    TransactionId = orderId,
                    Status = "pending",
                    Amount = request.Amount,
                    PaymentUrl = $"https://accept.paymob.com/api/acceptance/iframes/your-iframe-id?payment_token={paymentKey}",
                    Message = "Payment initiated. Redirect user to Paymob."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Paymob payment failed");
                return new PaymentResponse
                {
                    Status = "failed",
                    Message = ex.Message
                };
            }
        }

        private async Task<string> GetAuthToken()
        {
            var request = new
            {
                api_key = _paymobSettings.ApiKey
            };
            
            var response = await _httpClient.PostAsync(
                "/auth/tokens",
                new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
            );
            
            if (!response.IsSuccessStatusCode)
                return string.Empty;

            var content = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(content);
            return json.RootElement.GetProperty("token").GetString() ?? string.Empty;
        }

        private async Task<string> CreateOrder(string authToken, InitiatePaymentRequest request)
        {
            var orderRequest = new
            {
                auth_token = authToken,
                delivery_needed = false,
                amount_cents = (long)(request.Amount * 100),
                currency = "EGP",
                items = new object[0]
            };

            var response = await _httpClient.PostAsync(
                "/ecommerce/orders",
                new StringContent(JsonSerializer.Serialize(orderRequest), Encoding.UTF8, "application/json")
            );

            if (!response.IsSuccessStatusCode)
                return string.Empty;

            var content = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(content);
            return json.RootElement.GetProperty("id").ToString();
        }

        private async Task<string> GetPaymentKey(string authToken, string orderId, InitiatePaymentRequest request)
        {
            var paymentKeyRequest = new
            {
                auth_token = authToken,
                amount_cents = (long)(request.Amount * 100),
                expiration = 3600,
                order_id = orderId,
                integration_id = _paymobSettings.IntegrationId,
                currency = "EGP",
                billing_data = new
                {
                    apartment = "NA",
                    email = "test@example.com",
                    floor = "NA",
                    first_name = request.CardHolderName ?? "Customer",
                    street = "NA",
                    building = "NA",
                    phone_number = "01000000000",
                    shipping_method = "NA",
                    postal_code = "NA",
                    city = "NA",
                    country = "NA",
                    last_name = "NA",
                    state = "NA"
                }
            };

            var response = await _httpClient.PostAsync(
                "/acceptance/payment_keys",
                new StringContent(JsonSerializer.Serialize(paymentKeyRequest), Encoding.UTF8, "application/json")
            );

            if (!response.IsSuccessStatusCode)
                return string.Empty;

            var content = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(content);
            return json.RootElement.GetProperty("token").GetString() ?? string.Empty;
        }

        public async Task<bool> VerifyPayment(string transactionId)
        {
            await Task.CompletedTask;
            return true;
        }

        public async Task<PaymentResponse> HandleCallback(PaymentCallbackRequest request)
        {
            var isValid = VerifyHMAC(request);
            if (!isValid)
                return new PaymentResponse { Status = "failed", Message = "Invalid HMAC signature" };

            return new PaymentResponse
            {
                TransactionId = request.TransactionId,
                Status = request.Status,
                Message = "Callback handled successfully"
            };
        }

        private bool VerifyHMAC(PaymentCallbackRequest request)
        {
            return true;
        }
    }
}
