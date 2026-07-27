using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiraajApi.Data;
using SiraajApi.DTOs;
using SiraajApi.Models;
using SiraajApi.Services.Interfaces;

namespace SiraajApi.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPaymentService _paymentService;

        public PaymentsController(ApplicationDbContext context, IPaymentService paymentService)
        {
            _context = context;
            _paymentService = paymentService;
        }

        // POST: api/payments/initiate
        [HttpPost("initiate")]
        [Authorize]
        public async Task<IActionResult> InitiatePayment([FromBody] InitiatePaymentRequest request)
        {
            try
            {
                // Validate student exists
                var student = await _context.Students
                    .Include(s => s.User)
                    .FirstOrDefaultAsync(s => s.UserId == request.StudentId);

                if (student == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        statusCode = 404,
                        message = "الطالب غير موجود",
                        timestamp = DateTime.UtcNow,
                        path = "/api/payments/initiate"
                    });
                }

                // Validate student program
                var studentProgram = await _context.StudentPrograms
                    .Include(sp => sp.Package)
                    .Include(sp => sp.Program)
                    .FirstOrDefaultAsync(sp => sp.Id == request.StudentProgramId && sp.StudentId == request.StudentId);

                if (studentProgram == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        statusCode = 404,
                        message = "برنامج الطالب غير موجود",
                        timestamp = DateTime.UtcNow,
                        path = "/api/payments/initiate"
                    });
                }

                // Process payment
                var result = await _paymentService.ProcessPayment(request);

                // Save payment record
                var payment = new Payment
                {
                    Id = result.PaymentId,
                    StudentId = request.StudentId,
                    StudentProgramId = request.StudentProgramId,
                    Amount = studentProgram.Package.Price,
                    DiscountApplied = 0, // Calculate based on family/referral discounts
                    FinalAmount = studentProgram.Package.Price,
                    PaymentMethod = request.PaymentMethod,
                    PaymentGateway = request.PaymentMethod == "stripe" ? "stripe" : 
                                    request.PaymentMethod == "paymob" ? "paymob" : "manual",
                    TransactionId = result.TransactionId,
                    Status = result.Status,
                    PaymentDate = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Payments.Add(payment);

                // If payment successful, create subscription
                if (result.Status == "completed")
                {
                    var subscription = new Subscription
                    {
                        Id = Guid.NewGuid(),
                        StudentId = request.StudentId,
                        PackageId = studentProgram.PackageId,
                        ProgramId = studentProgram.ProgramId,
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddMonths(1),
                        IsActive = true,
                        AutoRenew = false,
                        Status = "active",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    _context.Subscriptions.Add(subscription);

                    // Generate invoice
                    var invoice = new Invoice
                    {
                        Id = Guid.NewGuid(),
                        InvoiceNumber = $"INV-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8)}",
                        PaymentId = payment.Id,
                        StudentId = request.StudentId,
                        StudentName = student.User?.NameAr ?? "Student",
                        PackageName = studentProgram.Package.NameAr,
                        Amount = payment.FinalAmount,
                        Tax = 0, // No tax for now
                        TotalAmount = payment.FinalAmount,
                        Currency = "SAR",
                        IssueDate = DateTime.UtcNow,
                        DueDate = DateTime.UtcNow.AddDays(7),
                        IsPaid = true,
                        PdfUrl = $"https://siraaj.com/invoices/{Guid.NewGuid()}.pdf",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    _context.Invoices.Add(invoice);
                }

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    data = result,
                    message = result.Status == "completed" ? "تم الدفع بنجاح" : "جاري معالجة الدفع"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "حدث خطأ أثناء معالجة الدفع",
                    timestamp = DateTime.UtcNow,
                    path = "/api/payments/initiate"
                });
            }
        }

        // POST: api/payments/callback
        [HttpPost("callback")]
        public async Task<IActionResult> PaymentCallback([FromBody] PaymentCallbackRequest request)
        {
            try
            {
                var result = await _paymentService.HandleCallback(request);

                // Update payment status
                var payment = await _context.Payments
                    .FirstOrDefaultAsync(p => p.TransactionId == request.TransactionId);

                if (payment != null)
                {
                    payment.Status = request.Status;
                    payment.UpdatedAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }

                return Ok(new
                {
                    success = true,
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "حدث خطأ أثناء معالجة رد الدفع",
                    timestamp = DateTime.UtcNow,
                    path = "/api/payments/callback"
                });
            }
        }

        // GET: api/payments/history/{studentId}
        [HttpGet("history/{studentId}")]
        [Authorize]
        public async Task<IActionResult> GetPaymentHistory(Guid studentId)
        {
            try
            {
                var payments = await _context.Payments
                    .Include(p => p.StudentProgram)
                    .ThenInclude(sp => sp.Package)
                    .Where(p => p.StudentId == studentId)
                    .OrderByDescending(p => p.PaymentDate)
                    .Select(p => new PaymentHistoryDto
                    {
                        Id = p.Id,
                        PaymentDate = p.PaymentDate,
                        Amount = p.FinalAmount,
                        PaymentMethod = p.PaymentMethod,
                        Status = p.Status,
                        TransactionId = p.TransactionId,
                        PackageName = p.StudentProgram != null ? p.StudentProgram.Package.NameAr : ""
                    })
                    .ToListAsync();

                return Ok(new
                {
                    success = true,
                    data = payments
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "حدث خطأ أثناء جلب سجل الدفعات",
                    timestamp = DateTime.UtcNow,
                    path = $"/api/payments/history/{studentId}"
                });
            }
        }

        // GET: api/payments/invoices/{studentId}
        [HttpGet("invoices/{studentId}")]
        [Authorize]
        public async Task<IActionResult> GetInvoices(Guid studentId)
        {
            try
            {
                var invoices = await _context.Invoices
                    .Where(i => i.StudentId == studentId)
                    .OrderByDescending(i => i.IssueDate)
                    .Select(i => new InvoiceDto
                    {
                        Id = i.Id,
                        InvoiceNumber = i.InvoiceNumber,
                        Amount = i.Amount,
                        Tax = i.Tax,
                        TotalAmount = i.TotalAmount,
                        IssueDate = i.IssueDate,
                        DueDate = i.DueDate,
                        IsPaid = i.IsPaid,
                        PdfUrl = i.PdfUrl
                    })
                    .ToListAsync();

                return Ok(new
                {
                    success = true,
                    data = invoices
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "حدث خطأ أثناء جلب الفواتير",
                    timestamp = DateTime.UtcNow,
                    path = $"/api/payments/invoices/{studentId}"
                });
            }
        }

        // GET: api/payments/subscriptions/{studentId}
        [HttpGet("subscriptions/{studentId}")]
        [Authorize]
        public async Task<IActionResult> GetSubscriptions(Guid studentId)
        {
            try
            {
                var subscriptions = await _context.Subscriptions
                    .Include(s => s.Package)
                    .Include(s => s.Program)
                    .Where(s => s.StudentId == studentId && s.IsActive)
                    .Select(s => new SubscriptionDto
                    {
                        Id = s.Id,
                        PackageName = s.Package.NameAr,
                        ProgramName = s.Program.NameAr,
                        StartDate = s.StartDate,
                        EndDate = s.EndDate,
                        IsActive = s.IsActive,
                        Status = s.Status,
                        AutoRenew = s.AutoRenew
                    })
                    .ToListAsync();

                return Ok(new
                {
                    success = true,
                    data = subscriptions
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "حدث خطأ أثناء جلب الاشتراكات",
                    timestamp = DateTime.UtcNow,
                    path = $"/api/payments/subscriptions/{studentId}"
                });
            }
        }
    }
}
