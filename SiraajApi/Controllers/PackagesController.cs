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
                var packages = await _context.Packages
                    .AsNoTracking()
                    .Where(p => p.IsActive)
                    .ToListAsync();

                var result = packages.Select(p => new PackageDto
                {
                    Id = p.Id,
                    NameAr = p.NameAr,
                    NameEn = p.NameEn,
                    Price = p.Price,
                    OriginalPrice = p.OriginalPrice,
                    PriceUsd = p.PriceUsd,
                    OriginalPriceUsd = p.OriginalPriceUsd,
                    DefaultCurrency = p.DefaultCurrency ?? "SAR",
                    Sessions = p.Sessions,
                    SessionsPerWeek = p.SessionsPerWeek,
                    Color = p.Color,
                    IsHighlighted = p.IsHighlighted,
                    BadgeAr = p.BadgeAr,
                    BadgeEn = p.BadgeEn,
                    FeaturesAr = TryDeserializeList(p.FeaturesAr),
                    FeaturesEn = TryDeserializeList(p.FeaturesEn),
                    IsActive = p.IsActive
                }).ToList();

                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "حدث خطأ أثناء جلب الباقات",
                    error = ex.Message
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
                        DefaultCurrency = package.DefaultCurrency ?? "SAR",
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

        // POST: api/packages (Admin)
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreatePackage([FromBody] CreatePackageRequest request)
        {
            try
            {
                var package = new Package
                {
                    Id = Guid.NewGuid(),
                    NameAr = request.NameAr,
                    NameEn = request.NameEn,
                    Price = request.Price,
                    OriginalPrice = request.OriginalPrice,
                    PriceUsd = request.PriceUsd,
                    OriginalPriceUsd = request.OriginalPriceUsd,
                    DefaultCurrency = string.IsNullOrEmpty(request.DefaultCurrency) ? "SAR" : request.DefaultCurrency,
                    Sessions = request.Sessions,
                    SessionsPerWeek = request.SessionsPerWeek,
                    Color = request.Color,
                    IsHighlighted = request.IsHighlighted,
                    BadgeAr = request.BadgeAr,
                    BadgeEn = request.BadgeEn,
                    FeaturesAr = JsonSerializer.Serialize(request.FeaturesAr ?? new List<string>()),
                    FeaturesEn = JsonSerializer.Serialize(request.FeaturesEn ?? new List<string>()),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Packages.Add(package);
                await _context.SaveChangesAsync();

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
                        FeaturesAr = request.FeaturesAr ?? new List<string>(),
                        FeaturesEn = request.FeaturesEn ?? new List<string>(),
                        IsActive = package.IsActive
                    },
                    message = "تم إضافة الباقة بنجاح"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "حدث خطأ أثناء إضافة الباقة",
                    error = ex.Message
                });
            }
        }

        // PUT: api/packages/{id} (Admin)
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdatePackage(Guid id, [FromBody] UpdatePackageRequest request)
        {
            try
            {
                var package = await _context.Packages.FirstOrDefaultAsync(p => p.Id == id);

                if (package == null)
                    return NotFound(new { success = false, message = "الباقة غير موجودة" });

                package.NameAr = request.NameAr;
                package.NameEn = request.NameEn;
                package.Price = request.Price;
                package.OriginalPrice = request.OriginalPrice;
                package.PriceUsd = request.PriceUsd;
                package.OriginalPriceUsd = request.OriginalPriceUsd;
                package.DefaultCurrency = string.IsNullOrEmpty(request.DefaultCurrency) ? "SAR" : request.DefaultCurrency;
                package.Sessions = request.Sessions;
                package.SessionsPerWeek = request.SessionsPerWeek;
                package.Color = request.Color;
                package.IsHighlighted = request.IsHighlighted;
                package.BadgeAr = request.BadgeAr;
                package.BadgeEn = request.BadgeEn;
                package.FeaturesAr = JsonSerializer.Serialize(request.FeaturesAr ?? new List<string>());
                package.FeaturesEn = JsonSerializer.Serialize(request.FeaturesEn ?? new List<string>());
                package.IsActive = request.IsActive;
                package.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

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
                        FeaturesAr = request.FeaturesAr ?? new List<string>(),
                        FeaturesEn = request.FeaturesEn ?? new List<string>(),
                        IsActive = package.IsActive
                    },
                    message = "تم تحديث الباقة بنجاح"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "حدث خطأ أثناء تحديث الباقة",
                    error = ex.Message
                });
            }
        }

        // DELETE: api/packages/{id} (Admin)
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeletePackage(Guid id)
        {
            try
            {
                var package = await _context.Packages.FirstOrDefaultAsync(p => p.Id == id);

                if (package == null)
                    return NotFound(new { success = false, message = "الباقة غير موجودة" });

                package.IsActive = false;
                package.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "تم حذف الباقة بنجاح" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "حدث خطأ أثناء حذف الباقة",
                    error = ex.Message
                });
            }
        }

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
    }
}
