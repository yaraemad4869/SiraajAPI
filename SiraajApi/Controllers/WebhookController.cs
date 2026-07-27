using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SiraajApi.Services;

namespace SiraajApi.Controllers
{
    [ApiController]
    [Route("api/webhook")]
    public class WebhookController : ControllerBase
    {
        private readonly RealStripePaymentService _stripeService;
        private readonly ILogger<WebhookController> _logger;

        public WebhookController(
            RealStripePaymentService stripeService,
            ILogger<WebhookController> logger)
        {
            _stripeService = stripeService;
            _logger = logger;
        }

        [HttpPost("stripe")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var signature = Request.Headers["Stripe-Signature"];

            try
            {
                var result = await _stripeService.HandleWebhook(json, signature);
                return Ok(new { success = result });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Webhook error");
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
