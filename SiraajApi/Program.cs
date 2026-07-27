using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SiraajApi.Data;
using SiraajApi.Helpers;
using SiraajApi.Models;
using SiraajApi.Services;
using SiraajApi.Services.Interfaces;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Siraaj API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ===== SIMPLIFIED JWT AUTHENTICATION =====
var key = Encoding.UTF8.GetBytes("SiraajAcademySecretKey2024ForJWTTokenGeneration");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            RoleClaimType = ClaimTypes.Role
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Auth Failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine($"Token Validated!");
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                Console.WriteLine($"Challenge: {context.Error}");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// Services
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IPaymentService, RealStripePaymentService>();

// Payment settings
builder.Services.Configure<StripeSettings>(
    builder.Configuration.GetSection("StripeSettings"));
builder.Services.Configure<PaymobSettings>(
    builder.Configuration.GetSection("PaymobSettings"));

builder.Services.AddHttpClient<RealPaymobPaymentService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Debug endpoint
app.MapGet("/api/debug/claims", (HttpContext context) =>
{
    var user = context.User;
    return Results.Ok(new
    {
        IsAuthenticated = user.Identity?.IsAuthenticated ?? false,
        Claims = user.Claims.Select(c => new { c.Type, c.Value }),
        Roles = user.FindAll(ClaimTypes.Role).Select(c => c.Value)
    });
});

// Health check
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }));

// Seed database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await context.Database.EnsureCreatedAsync();

    if (!await context.Users.AnyAsync())
    {
        var now = DateTime.UtcNow;
        var adminId = Guid.NewGuid();

        var admin = new User
        {
            Id = adminId,
            NameAr = "إدارة سراج",
            NameEn = "Siraj Admin",
            Phone = "01000000000",
            PasswordHash = PasswordHelper.HashPassword("admin123"),
            Role = "admin",
            ReferralCode = "ADMIN001",
            JoinDate = now,
            CreatedAt = now,
            UpdatedAt = now
        };
        context.Users.Add(admin);

        var studentId = Guid.NewGuid();
        var student = new User
        {
            Id = studentId,
            NameAr = "أحمد محمد علي",
            NameEn = "Ahmed Mohamed Ali",
            Phone = "01012345678",
            PasswordHash = PasswordHelper.HashPassword("123456"),
            Role = "student",
            ReferralCode = "SIRAJ-AMA123",
            JoinDate = now,
            CreatedAt = now,
            UpdatedAt = now
        };
        context.Users.Add(student);

        var guardianId = Guid.NewGuid();
        var guardian = new User
        {
            Id = guardianId,
            NameAr = "محمد أحمد السيد",
            NameEn = "Mohamed Ahmed El-Sayed",
            Phone = "01098765432",
            PasswordHash = PasswordHelper.HashPassword("123456"),
            Role = "guardian",
            ReferralCode = "SIRAJ-MAS123",
            JoinDate = now,
            CreatedAt = now,
            UpdatedAt = now
        };
        context.Users.Add(guardian);

        var teacherId = Guid.NewGuid();
        var teacher = new User
        {
            Id = teacherId,
            NameAr = "الشيخ كريم رضا",
            NameEn = "Sheikh Karim Rida",
            Phone = "01111111111",
            PasswordHash = PasswordHelper.HashPassword("teacher123"),
            Role = "teacher",
            ReferralCode = "TEACHER-KR",
            JoinDate = now,
            CreatedAt = now,
            UpdatedAt = now
        };
        context.Users.Add(teacher);

        await context.SaveChangesAsync();

        // Student record
        var studentRecord = new Student
        {
            UserId = studentId,
            PackageId = "premium",
            Schedule = "[\"sat-09:00\",\"mon-09:00\",\"wed-09:00\"]",
            Status = "active",
            TeacherId = teacherId
        };
        context.Students.Add(studentRecord);

        // Guardian record
        context.Guardians.Add(new Guardian { UserId = guardianId, ChildrenIds = "[]" });

        // Teacher record
        context.Teachers.Add(new Teacher { UserId = teacherId, Specialization = "حفظ وتجويد", StudentIds = "[]" });

        // Hero Settings
        context.HeroSettings.Add(new HeroSettings
        {
            Id = Guid.NewGuid(),
            TitleAr = "أكاديمية متخصصة في تعليم القرآن الكريم وعلومه عبر الإنترنت",
            TitleEn = "A specialized academy for online Quran education and its sciences",
            SubtitleAr = "معلمون متخصصون • جداول مرنة • من أي مكان في العالم",
            SubtitleEn = "Expert teachers • Flexible schedules • From anywhere in the world",
            TaglineAr = "ابدأ رحلتك مع القرآن اليوم",
            TaglineEn = "Start your Quran journey today",
            InstapayNumber = "+20 100 000 0000",
            InstapayName = "أكاديمية سراج",
            ContactPhone = "+966 50 000 0000",
            ContactEmail = "info@sirajacademy.com",
            ReferralDiscount = 50,
            UpdatedAt = now
        });

        // Discount Codes
        context.DiscountCodes.AddRange(
            new DiscountCode { Id = Guid.NewGuid(), Code = "SIRAJ-AMA123", DiscountAmount = 50, CreatedBy = adminId, CreatedAt = now },
            new DiscountCode { Id = Guid.NewGuid(), Code = "SIRAJ-MAS123", DiscountAmount = 50, CreatedBy = adminId, CreatedAt = now },
            new DiscountCode { Id = Guid.NewGuid(), Code = "DISCOUNT50", DiscountAmount = 50, CreatedBy = adminId, CreatedAt = now }
        );

        // Family Discounts
        context.FamilyDiscounts.AddRange(
            new FamilyDiscount { Id = Guid.NewGuid(), Order = 1, DiscountPercentage = 0, LabelAr = "الطفل الأول", LabelEn = "1st Child", Icon = "👧", CreatedAt = now, UpdatedAt = now },
            new FamilyDiscount { Id = Guid.NewGuid(), Order = 2, DiscountPercentage = 10, LabelAr = "الطفل الثاني", LabelEn = "2nd Child", Icon = "👦", CreatedAt = now, UpdatedAt = now },
            new FamilyDiscount { Id = Guid.NewGuid(), Order = 3, DiscountPercentage = 15, LabelAr = "الطفل الثالث", LabelEn = "3rd Child", Icon = "👧", CreatedAt = now, UpdatedAt = now },
            new FamilyDiscount { Id = Guid.NewGuid(), Order = 4, DiscountPercentage = 20, LabelAr = "٤ أطفال فأكثر", LabelEn = "4+ Children", Icon = "👨‍👩‍👧‍👦", CreatedAt = now, UpdatedAt = now }
        );

        // Packages
        context.Packages.AddRange(
            new Package
            {
                Id = Guid.NewGuid(),
                NameAr = "الباقة الأساسية",
                NameEn = "Basic",
                Price = 360,
                Sessions = 8,
                SessionsPerWeek = 2,
                Color = "#1E6B4A",
                IsHighlighted = false,
                FeaturesAr = "[\"معلم/معلمة متخصص/ة\",\"خطة تعليمية منظمة\",\"متابعة الحفظ والمراجعة\",\"واجبات وأنشطة تعليمية\",\"تقرير متابعة دوري لولي الأمر\",\"شهادة إتمام المستوى\"]",
                FeaturesEn = "[\"Specialized teacher\",\"Organized educational plan\",\"Memorization and revision tracking\",\"Educational assignments and activities\",\"Periodic progress report for guardian\",\"Level completion certificate\"]",
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            },
            new Package
            {
                Id = Guid.NewGuid(),
                NameAr = "الباقة المميزة",
                NameEn = "Premium",
                Price = 450,
                OriginalPrice = 540,
                Sessions = 12,
                SessionsPerWeek = 3,
                Color = "#C9A84C",
                IsHighlighted = true,
                BadgeAr = "الأكثر طلباً",
                BadgeEn = "Most Popular",
                FeaturesAr = "[\"جميع مميزات الباقة الأساسية\",\"زيادة عدد الحصص لتسريع التقدم\",\"متابعة مكثفة لتحقيق أفضل النتائج\",\"جلسات مراجعة إضافية\"]",
                FeaturesEn = "[\"All Basic package features\",\"More sessions to accelerate progress\",\"Intensive monitoring for best results\",\"Additional revision sessions\"]",
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            },
            new Package
            {
                Id = Guid.NewGuid(),
                NameAr = "باقة VIP",
                NameEn = "VIP",
                Price = 750,
                OriginalPrice = 900,
                Sessions = 20,
                SessionsPerWeek = 5,
                Color = "#7C3AED",
                IsHighlighted = true,
                BadgeAr = "الأفضل",
                BadgeEn = "Best Value",
                FeaturesAr = "[\"جميع مميزات الباقة الأساسية\",\"برنامج مكثف للحفظ والمراجعة\",\"أسرع معدل تقدم بإذن الله\",\"متابعة تعليمية مستمرة\",\"تقارير تفصيلية أسبوعية\"]",
                FeaturesEn = "[\"All Basic package features\",\"Intensive memorization program\",\"Fastest possible progress rate\",\"Continuous educational monitoring\",\"Detailed weekly progress reports\"]",
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            }
        );

        await context.SaveChangesAsync();
        Console.WriteLine("✅ Database seeded!");
    }
}

app.Run();
