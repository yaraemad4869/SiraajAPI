using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiraajApi.Data;
using SiraajApi.DTOs;

namespace SiraajApi.Controllers
{
    [ApiController]
    [Route("api/discounts")]
    public class DiscountController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DiscountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("validate")]
        public async Task<IActionResult> ValidateDiscount([FromBody] ValidateDiscountRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Code))
                {
                    return Ok(new
                    {
                        success = true,
                        data = new ValidateDiscountResponse
                        {
                            Valid = false,
                            DiscountAmount = 0,
                            Message = "يُرجى إدخال كود الخصم"
                        }
                    });
                }

                var discountCode = await _context.DiscountCodes
                    .FirstOrDefaultAsync(dc => dc.Code.ToUpper() == request.Code.ToUpper() && dc.UsedBy == null);

                if (discountCode == null)
                {
                    return Ok(new
                    {
                        success = true,
                        data = new ValidateDiscountResponse
                        {
                            Valid = false,
                            DiscountAmount = 0,
                            Message = "الكود غير صحيح أو غير موجود"
                        }
                    });
                }

                return Ok(new
                {
                    success = true,
                    data = new ValidateDiscountResponse
                    {
                        Valid = true,
                        DiscountAmount = discountCode.DiscountAmount,
                        Message = $"تم تطبيق خصم {discountCode.DiscountAmount} ريال"
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "حدث خطأ أثناء التحقق من الكود",
                    timestamp = DateTime.UtcNow,
                    path = "/api/discounts/validate"
                });
            }
        }
    }
}
