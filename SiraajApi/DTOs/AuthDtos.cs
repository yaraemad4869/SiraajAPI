using System.ComponentModel.DataAnnotations;

namespace SiraajApi.DTOs
{
    public class LoginRequest
    {
        [Required]
        public string Phone { get; set; } = string.Empty;
        
        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        [Required]
        public string NameAr { get; set; } = string.Empty;
        
        public string NameEn { get; set; } = string.Empty;
        
        [Required]
        [MinLength(10)]
        public string Phone { get; set; } = string.Empty;
        
        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
        
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
        
        [Required]
        public string Role { get; set; } = string.Empty;
        
        public string GuardianPhone { get; set; } = string.Empty;
        public string ReferralCode { get; set; } = string.Empty;
    }

    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class AuthResponse
    {
        public UserDto User { get; set; }
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public int? DiscountApplied { get; set; }
    }

    public class UserDto
    {
        public Guid Id { get; set; }
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string ReferralCode { get; set; } = string.Empty;
        public string PackageId { get; set; } = string.Empty;
        public Guid? TeacherId { get; set; }
        public string Schedule { get; set; } = "[]";
        public string JoinDate { get; set; } = string.Empty;
    }
}
