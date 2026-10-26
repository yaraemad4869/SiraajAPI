using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiraajApi.Data;
using SiraajApi.Models;

namespace SiraajApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class ContentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ContentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/discounts/family
        [HttpGet("discounts/family")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFamilyDiscounts()
        {
            try
            {
                var discounts = await _context.FamilyDiscounts
                    .AsNoTracking()
                    .OrderBy(fd => fd.Order)
                    .Select(fd => new
                    {
                        id = fd.Id,
                        order = fd.Order,
                        discount = fd.DiscountPercentage,
                        labelAr = fd.LabelAr,
                        labelEn = fd.LabelEn,
                        icon = fd.Icon
                    })
                    .ToListAsync();

                return Ok(new { success = true, data = discounts });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "حدث خطأ أثناء جلب خصومات العائلة", error = ex.Message });
            }
        }

        // GET: api/content/features
        [HttpGet("content/features")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFeatures()
        {
            try
            {
                var features = await _context.Features
                    .AsNoTracking()
                    .Select(f => new
                    {
                        id = f.Id,
                        icon = f.Icon,
                        nameAr = f.NameAr,
                        nameEn = f.NameEn,
                        titleAr = f.TitleAr,
                        titleEn = f.TitleEn,
                        descAr = f.DescAr,
                        descEn = f.DescEn
                    })
                    .ToListAsync();

                return Ok(new { success = true, data = features });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "حدث خطأ أثناء جلب المميزات", error = ex.Message });
            }
        }
    }
}
