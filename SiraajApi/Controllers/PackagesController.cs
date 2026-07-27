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
                    .Where(p => p.IsActive)
                    .ToListAsync();

                var result = packages.Select(p => new PackageDto
                {
                    Id = p.Id,
                    NameAr = p.NameAr,
                    NameEn = p.NameEn,
                    Price = p.Price,
                    OriginalPrice = p.OriginalPrice,
                    Sessions = p.Sessions,
                    SessionsPerWeek = p.SessionsPerWeek,
                    Color = p.Color,
                    IsHighlighted = p.IsHighlighted,
                    BadgeAr = p.BadgeAr,
                    BadgeEn = p.BadgeEn,
                    FeaturesAr = JsonSerializer.Deserialize<List<string>>(p.FeaturesAr) ?? new List<string>(),
                    FeaturesEn = JsonSerializer.Deserialize<List<string>>(p.FeaturesEn) ?? new List<string>(),
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
                    timestamp = DateTime.UtcNow,
                    path = "/api/packages"
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
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (package == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        statusCode = 404,
                        message = "الباقة غير موجودة",
                        timestamp = DateTime.UtcNow,
                        path = $"/api/packages/{id}"
                    });
                }

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
                        Sessions = package.Sessions,
                        SessionsPerWeek = package.SessionsPerWeek,
                        Color = package.Color,
                        IsHighlighted = package.IsHighlighted,
                        BadgeAr = package.BadgeAr,
                        BadgeEn = package.BadgeEn,
                        FeaturesAr = JsonSerializer.Deserialize<List<string>>(package.FeaturesAr) ?? new List<string>(),
                        FeaturesEn = JsonSerializer.Deserialize<List<string>>(package.FeaturesEn) ?? new List<string>(),
                        IsActive = package.IsActive
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "حدث خطأ أثناء جلب الباقة",
                    timestamp = DateTime.UtcNow,
                    path = $"/api/packages/{id}"
                });
            }
        }

        // POST: api/packages (Admin only)
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
                        Sessions = package.Sessions,
                        SessionsPerWeek = package.SessionsPerWeek,
                        Color = package.Color,
                        IsHighlighted = package.IsHighlighted,
                        BadgeAr = package.BadgeAr,
                        BadgeEn = package.BadgeEn,
                        FeaturesAr = JsonSerializer.Deserialize<List<string>>(package.FeaturesAr) ?? new List<string>(),
                        FeaturesEn = JsonSerializer.Deserialize<List<string>>(package.FeaturesEn) ?? new List<string>(),
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
                    statusCode = 500,
                    message = "حدث خطأ أثناء إضافة الباقة",
                    timestamp = DateTime.UtcNow,
                    path = "/api/packages"
                });
            }
        }

        // PUT: api/packages/{id} (Admin only)
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdatePackage(Guid id, [FromBody] UpdatePackageRequest request)
        {
            try
            {
                var package = await _context.Packages
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (package == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        statusCode = 404,
                        message = "الباقة غير موجودة",
                        timestamp = DateTime.UtcNow,
                        path = $"/api/packages/{id}"
                    });
                }

                package.NameAr = request.NameAr;
                package.NameEn = request.NameEn;
                package.Price = request.Price;
                package.OriginalPrice = request.OriginalPrice;
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
                        Sessions = package.Sessions,
                        SessionsPerWeek = package.SessionsPerWeek,
                        Color = package.Color,
                        IsHighlighted = package.IsHighlighted,
                        BadgeAr = package.BadgeAr,
                        BadgeEn = package.BadgeEn,
                        FeaturesAr = JsonSerializer.Deserialize<List<string>>(package.FeaturesAr) ?? new List<string>(),
                        FeaturesEn = JsonSerializer.Deserialize<List<string>>(package.FeaturesEn) ?? new List<string>(),
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
                    statusCode = 500,
                    message = "حدث خطأ أثناء تحديث الباقة",
                    timestamp = DateTime.UtcNow,
                    path = $"/api/packages/{id}"
                });
            }
        }

        // DELETE: api/packages/{id} (Admin only)
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeletePackage(Guid id)
        {
            try
            {
                var package = await _context.Packages
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (package == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        statusCode = 404,
                        message = "الباقة غير موجودة",
                        timestamp = DateTime.UtcNow,
                        path = $"/api/packages/{id}"
                    });
                }

                // Soft delete - just deactivate
                package.IsActive = false;
                package.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "تم حذف الباقة بنجاح"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "حدث خطأ أثناء حذف الباقة",
                    timestamp = DateTime.UtcNow,
                    path = $"/api/packages/{id}"
                });
            }
        }
    }
}
