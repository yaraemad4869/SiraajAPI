# Seed Programs
Write-Host "Seeding Programs..." -ForegroundColor Yellow

# Login first
$body = @{ phone = "01000000000"; password = "admin123" } | ConvertTo-Json
$login = Invoke-RestMethod -Uri "http://localhost:5000/api/auth/login" -Method Post -Body $body -ContentType "application/json"
$token = $login.data.token

# Programs to seed
$programs = @(
    @{
        icon = "📖"
        nameAr = "تعليم القراءة النورانية"
        nameEn = "Nourania Reading"
        descAr = "للمبتدئين من الصغر"
        descEn = "For young beginners"
        isActive = $true
    },
    @{
        icon = "🌙"
        nameAr = "حفظ القرآن الكريم"
        nameEn = "Quran Memorization"
        descAr = "بمنهج متسلسل ومنظم"
        descEn = "With a structured curriculum"
        isActive = $true
    },
    @{
        icon = "✨"
        nameAr = "أحكام التجويد"
        nameEn = "Tajweed Rules"
        descAr = "بتطبيق عملي مستمر"
        descEn = "With continuous practical application"
        isActive = $true
    },
    @{
        icon = "📚"
        nameAr = "تفسير القرآن"
        nameEn = "Quran Tafseer"
        descAr = "فهم معاني الآيات الكريمة"
        descEn = "Understanding Quranic meanings"
        isActive = $true
    }
)

foreach ($program in $programs) {
    $body = $program | ConvertTo-Json -Depth 5
    try {
        $result = Invoke-RestMethod -Uri "http://localhost:5000/api/programs" -Method Post -Body $body -ContentType "application/json; charset=utf-8" -Headers @{Authorization = "Bearer $token"}
        Write-Host "Created: $($program.nameAr)" -ForegroundColor Green
    } catch {
        Write-Host "Failed to create: $($program.nameAr)" -ForegroundColor Red
        Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Yellow
    }
}

# Packages to seed
$packages = @(
    @{
        nameAr = "الباقة الأساسية"
        nameEn = "Basic"
        price = 360
        originalPrice = $null
        sessions = 8
        sessionsPerWeek = 2
        color = "#1E6B4A"
        isHighlighted = $false
        badgeAr = ""
        badgeEn = ""
        featuresAr = @("معلم/معلمة متخصص/ة", "خطة تعليمية منظمة", "متابعة الحفظ والمراجعة", "واجبات وأنشطة تعليمية", "تقرير متابعة دوري لولي الأمر", "شهادة إتمام المستوى")
        featuresEn = @("Specialized teacher", "Organized educational plan", "Memorization and revision tracking", "Educational assignments and activities", "Periodic progress report for guardian", "Level completion certificate")
    },
    @{
        nameAr = "الباقة المميزة"
        nameEn = "Premium"
        price = 450
        originalPrice = 540
        sessions = 12
        sessionsPerWeek = 3
        color = "#C9A84C"
        isHighlighted = $true
        badgeAr = "الأكثر طلباً"
        badgeEn = "Most Popular"
        featuresAr = @("جميع مميزات الباقة الأساسية", "زيادة عدد الحصص لتسريع التقدم", "متابعة مكثفة لتحقيق أفضل النتائج", "جلسات مراجعة إضافية")
        featuresEn = @("All Basic package features", "More sessions to accelerate progress", "Intensive monitoring for best results", "Additional revision sessions")
    },
    @{
        nameAr = "باقة VIP"
        nameEn = "VIP"
        price = 750
        originalPrice = 900
        sessions = 20
        sessionsPerWeek = 5
        color = "#7C3AED"
        isHighlighted = $true
        badgeAr = "الأفضل"
        badgeEn = "Best Value"
        featuresAr = @("جميع مميزات الباقة الأساسية", "برنامج مكثف للحفظ والمراجعة", "أسرع معدل تقدم بإذن الله", "متابعة تعليمية مستمرة", "تقارير تفصيلية أسبوعية")
        featuresEn = @("All Basic package features", "Intensive memorization program", "Fastest possible progress rate", "Continuous educational monitoring", "Detailed weekly progress reports")
    }
)

Write-Host "`nSeeding Packages..." -ForegroundColor Yellow

foreach ($package in $packages) {
    $body = $package | ConvertTo-Json -Depth 5
    try {
        $result = Invoke-RestMethod -Uri "http://localhost:5000/api/packages" -Method Post -Body $body -ContentType "application/json; charset=utf-8" -Headers @{Authorization = "Bearer $token"}
        Write-Host "Created: $($package.nameAr)" -ForegroundColor Green
    } catch {
        Write-Host "Failed to create: $($package.nameAr)" -ForegroundColor Red
        Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Yellow
    }
}

Write-Host "`nSeeding complete!" -ForegroundColor Green
