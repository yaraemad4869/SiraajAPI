using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiraajApi.Data;
using SiraajApi.DTOs;
using SiraajApi.Helpers;
using SiraajApi.Models;
using SiraajApi.Services.Interfaces;

namespace SiraajApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;

        public AuthController(ApplicationDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Student)
                    .Include(u => u.Guardian)
                    .Include(u => u.Teacher)
                    .FirstOrDefaultAsync(u => u.Phone == request.Phone);

                if (user == null || !PasswordHelper.VerifyPassword(request.Password, user.PasswordHash))
                {
                    return Unauthorized(new
                    {
                        success = false,
                        statusCode = 401,
                        message = "رقم الجوال أو كلمة المرور غير صحيحة",
                        timestamp = DateTime.UtcNow,
                        path = "/api/auth/login"
                    });
                }

                var token = _jwtService.GenerateAccessToken(user);
                var refreshToken = PasswordHelper.GenerateRefreshToken();

                var refreshTokenEntity = new RefreshToken
                {
                    UserId = user.Id,
                    Token = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddDays(7),
                    Revoked = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.RefreshTokens.Add(refreshTokenEntity);
                await _context.SaveChangesAsync();

                var userDto = new UserDto
                {
                    Id = user.Id,
                    NameAr = user.NameAr,
                    NameEn = user.NameEn,
                    Phone = user.Phone,
                    Role = user.Role,
                    ReferralCode = user.ReferralCode,
                    PackageId = user.Student?.PackageId ?? string.Empty,
                    TeacherId = user.Student?.TeacherId,
                    Schedule = user.Student?.Schedule ?? "[]",
                    JoinDate = user.JoinDate.ToString("yyyy-MM-dd")
                };

                return Ok(new
                {
                    success = true,
                    data = new AuthResponse
                    {
                        User = userDto,
                        Token = token,
                        RefreshToken = refreshToken
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "حدث خطأ أثناء تسجيل الدخول",
                    timestamp = DateTime.UtcNow,
                    path = "/api/auth/login"
                });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                if (await _context.Users.AnyAsync(u => u.Phone == request.Phone))
                {
                    return BadRequest(new
                    {
                        success = false,
                        statusCode = 400,
                        message = "رقم الجوال مستخدم بالفعل",
                        errors = new { phone = new[] { "رقم الجوال مستخدم بالفعل" } },
                        timestamp = DateTime.UtcNow,
                        path = "/api/auth/register"
                    });
                }

                if (request.Role != "student" && request.Role != "guardian")
                {
                    return BadRequest(new
                    {
                        success = false,
                        statusCode = 400,
                        message = "نوع الحساب غير صحيح",
                        timestamp = DateTime.UtcNow,
                        path = "/api/auth/register"
                    });
                }

                if (request.Role == "student" && string.IsNullOrEmpty(request.GuardianPhone))
                {
                    return BadRequest(new
                    {
                        success = false,
                        statusCode = 400,
                        message = "رقم ولي الأمر مطلوب",
                        errors = new { guardianPhone = new[] { "رقم ولي الأمر مطلوب" } },
                        timestamp = DateTime.UtcNow,
                        path = "/api/auth/register"
                    });
                }

                int discountApplied = 0;
                Guid? guardianId = null;

                if (!string.IsNullOrEmpty(request.ReferralCode))
                {
                    var discountCode = await _context.DiscountCodes
                        .FirstOrDefaultAsync(dc => dc.Code.ToUpper() == request.ReferralCode.ToUpper() && dc.UsedBy == null);

                    if (discountCode != null)
                    {
                        discountApplied = discountCode.DiscountAmount;
                    }
                }

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    NameAr = request.NameAr,
                    NameEn = request.NameEn ?? request.NameAr,
                    Phone = request.Phone,
                    PasswordHash = PasswordHelper.HashPassword(request.Password),
                    Role = request.Role,
                    ReferralCode = PasswordHelper.GenerateReferralCode(request.NameAr),
                    JoinDate = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);

                if (request.Role == "student")
                {
                    var guardian = await _context.Users
                        .Include(u => u.Guardian)
                        .FirstOrDefaultAsync(u => u.Phone == request.GuardianPhone && u.Role == "guardian");

                    if (guardian != null)
                    {
                        guardianId = guardian.Id;
                    }

                    var student = new Student
                    {
                        UserId = user.Id,
                        PackageId = "basic",
                        GuardianId = guardianId,
                        Status = "pending",
                        Schedule = "[]"
                    };
                    _context.Students.Add(student);
                }
                else if (request.Role == "guardian")
                {
                    var guardian = new Guardian
                    {
                        UserId = user.Id,
                        ChildrenIds = "[]"
                    };
                    _context.Guardians.Add(guardian);
                }

                await _context.SaveChangesAsync();

                var token = _jwtService.GenerateAccessToken(user);
                var refreshToken = PasswordHelper.GenerateRefreshToken();

                var refreshTokenEntity = new RefreshToken
                {
                    UserId = user.Id,
                    Token = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddDays(7),
                    Revoked = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.RefreshTokens.Add(refreshTokenEntity);
                await _context.SaveChangesAsync();

                var userDto = new UserDto
                {
                    Id = user.Id,
                    NameAr = user.NameAr,
                    NameEn = user.NameEn,
                    Phone = user.Phone,
                    Role = user.Role,
                    ReferralCode = user.ReferralCode,
                    PackageId = request.Role == "student" ? "basic" : string.Empty,
                    JoinDate = user.JoinDate.ToString("yyyy-MM-dd")
                };

                return Ok(new
                {
                    success = true,
                    data = new AuthResponse
                    {
                        User = userDto,
                        Token = token,
                        RefreshToken = refreshToken,
                        DiscountApplied = discountApplied > 0 ? discountApplied : null
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "حدث خطأ أثناء التسجيل",
                    timestamp = DateTime.UtcNow,
                    path = "/api/auth/register"
                });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var refreshToken = await _context.RefreshTokens
                    .Include(rt => rt.User)
                    .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken && !rt.Revoked);

                if (refreshToken == null || refreshToken.ExpiresAt < DateTime.UtcNow)
                {
                    return Unauthorized(new
                    {
                        success = false,
                        statusCode = 401,
                        message = "Refresh token غير صالح",
                        timestamp = DateTime.UtcNow,
                        path = "/api/auth/refresh-token"
                    });
                }

                var user = refreshToken.User;
                var newToken = _jwtService.GenerateAccessToken(user);
                var newRefreshToken = PasswordHelper.GenerateRefreshToken();

                refreshToken.Revoked = true;
                refreshToken.UpdatedAt = DateTime.UtcNow;

                var newRefreshTokenEntity = new RefreshToken
                {
                    UserId = user.Id,
                    Token = newRefreshToken,
                    ExpiresAt = DateTime.UtcNow.AddDays(7),
                    Revoked = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.RefreshTokens.Add(newRefreshTokenEntity);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    data = new
                    {
                        token = newToken,
                        refreshToken = newRefreshToken
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    statusCode = 500,
                    message = "حدث خطأ أثناء تحديث التوكن",
                    timestamp = DateTime.UtcNow,
                    path = "/api/auth/refresh-token"
                });
            }
        }
    }
}
