using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiraajApi.Data;
using SiraajApi.DTOs;
using SiraajApi.Models;
using System.Text.Json;

namespace SiraajApi.Controllers
{
    [ApiController]
    [Route("api/packages")]
    public class PackagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PackagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/packages
        [HttpGet]
        public async Task<IActionResult> GetPackages()
        {
            try
            {
                // 1. جلب البيانات من الداتابيز بدون الـ Navigation Properties
                var packages = await _context.Packages
                    .AsNoTracking()
                    .Where(p => p.IsActive)
                    .ToListAsync();

                // 2. تحويلها إلى DTO لتجنب Circular Reference أو مشاكل الـ Serialization
                var result = packages.Select(p => new PackageDto
                {
                    Id = p.Id,
                    NameAr = p.NameAr,
                    NameEn = p.NameEn,
                    Price = p.Price,
                    OriginalPrice = p.OriginalPrice,
                    PriceUsd = p.PriceUsd,
                    OriginalPriceUsd = p.OriginalPriceUsd,
                    DefaultCurrency = p.DefaultCurrency,
                    Sessions = p.Sessions,
                    SessionsPerWeek = p.SessionsPerWeek,
                    Color = p.Color,
                    IsHighlighted = p.IsHighlighted,
                    BadgeAr = p.BadgeAr,
                    BadgeEn = p.BadgeEn,
                    // Safely deserialize JSON strings
                    FeaturesAr = TryDeserializeList(p.FeaturesAr),
                    FeaturesEn = TryDeserializeList(p.FeaturesEn),
                    IsActive = p.IsActive
                }).ToList();

                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                // Log the actual error for debugging
                Console.WriteLine($"Error fetching packages: {ex.Message}");
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "حدث خطأ أثناء جلب الباقات",
                    error = ex.Message // مؤقتاً للتشخيص
                });
            }
        }

        // GET: api/packages/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPackage(Guid id)
        {
            try
            {
                var package = await _context.Packages
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (package == null)
                    return NotFound(new { success = false, message = "الباقة غير موجودة" });

                return Ok(new
                {
                    success = true,
                    data = new PackageDto
                    {
                        Id = package.Id,
                        NameAr = package.NameAr,
                        NameEn = package.NameEn,
                        Price = package.Price,
                        OriginalPrice = package.OriginalPrice,
                        PriceUsd = package.PriceUsd,
                        OriginalPriceUsd = package.OriginalPriceUsd,
                        DefaultCurrency = package.DefaultCurrency,
                        Sessions = package.Sessions,
                        SessionsPerWeek = package.SessionsPerWeek,
                        Color = package.Color,
                        IsHighlighted = package.IsHighlighted,
                        BadgeAr = package.BadgeAr,
                        BadgeEn = package.BadgeEn,
                        FeaturesAr = TryDeserializeList(package.FeaturesAr),
                        FeaturesEn = TryDeserializeList(package.FeaturesEn),
                        IsActive = package.IsActive
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "حدث خطأ أثناء جلب الباقة", error = ex.Message });
            }
        }

        // Helper method for safe JSON deserialization
        private List<string> TryDeserializeList(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return new List<string>();
            try
            {
                return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }

        // POST, PUT, DELETE methods... (يمكنك إضافتها حسب الحاجة)
    }
}
