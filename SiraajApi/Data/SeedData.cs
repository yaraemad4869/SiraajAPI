using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SiraajApi.Models;

namespace SiraajApi.Data
{
    public static class SeedData
    {
        public static async Task SeedQuranSurahs(ApplicationDbContext context)
        {
            if (await context.QuranSurahs.AnyAsync())
            {
                return;
            }

            var surahs = new List<QuranSurah>
            {
                new QuranSurah { Number = 1, NameAr = "الفاتحة", NameEn = "Al-Fatihah", VersesCount = 7, JuzNumber = 1 },
                new QuranSurah { Number = 2, NameAr = "البقرة", NameEn = "Al-Baqarah", VersesCount = 286, JuzNumber = 1 },
                new QuranSurah { Number = 3, NameAr = "آل عمران", NameEn = "Al-Imran", VersesCount = 200, JuzNumber = 3 },
                new QuranSurah { Number = 4, NameAr = "النساء", NameEn = "An-Nisa", VersesCount = 176, JuzNumber = 4 },
                new QuranSurah { Number = 5, NameAr = "المائدة", NameEn = "Al-Maidah", VersesCount = 120, JuzNumber = 6 },
                new QuranSurah { Number = 6, NameAr = "الأنعام", NameEn = "Al-An'am", VersesCount = 165, JuzNumber = 7 },
                new QuranSurah { Number = 7, NameAr = "الأعراف", NameEn = "Al-A'raf", VersesCount = 206, JuzNumber = 8 },
                new QuranSurah { Number = 8, NameAr = "الأنفال", NameEn = "Al-Anfal", VersesCount = 75, JuzNumber = 9 },
                new QuranSurah { Number = 9, NameAr = "التوبة", NameEn = "At-Tawbah", VersesCount = 129, JuzNumber = 10 },
                new QuranSurah { Number = 10, NameAr = "يونس", NameEn = "Yunus", VersesCount = 109, JuzNumber = 11 },
                new QuranSurah { Number = 11, NameAr = "هود", NameEn = "Hud", VersesCount = 123, JuzNumber = 11 },
                new QuranSurah { Number = 12, NameAr = "يوسف", NameEn = "Yusuf", VersesCount = 111, JuzNumber = 12 },
                new QuranSurah { Number = 13, NameAr = "الرعد", NameEn = "Ar-Ra'd", VersesCount = 43, JuzNumber = 13 },
                new QuranSurah { Number = 14, NameAr = "إبراهيم", NameEn = "Ibrahim", VersesCount = 52, JuzNumber = 13 },
                new QuranSurah { Number = 15, NameAr = "الحجر", NameEn = "Al-Hijr", VersesCount = 99, JuzNumber = 14 },
                new QuranSurah { Number = 16, NameAr = "النحل", NameEn = "An-Nahl", VersesCount = 128, JuzNumber = 14 },
                new QuranSurah { Number = 17, NameAr = "الإسراء", NameEn = "Al-Isra", VersesCount = 111, JuzNumber = 15 },
                new QuranSurah { Number = 18, NameAr = "الكهف", NameEn = "Al-Kahf", VersesCount = 110, JuzNumber = 15 },
                new QuranSurah { Number = 19, NameAr = "مريم", NameEn = "Maryam", VersesCount = 98, JuzNumber = 16 },
                new QuranSurah { Number = 20, NameAr = "طه", NameEn = "Taha", VersesCount = 135, JuzNumber = 16 },
                new QuranSurah { Number = 21, NameAr = "الأنبياء", NameEn = "Al-Anbiya", VersesCount = 112, JuzNumber = 17 },
                new QuranSurah { Number = 22, NameAr = "الحج", NameEn = "Al-Hajj", VersesCount = 78, JuzNumber = 17 },
                new QuranSurah { Number = 23, NameAr = "المؤمنون", NameEn = "Al-Mu'minun", VersesCount = 118, JuzNumber = 18 },
                new QuranSurah { Number = 24, NameAr = "النور", NameEn = "An-Nur", VersesCount = 64, JuzNumber = 18 },
                new QuranSurah { Number = 25, NameAr = "الفرقان", NameEn = "Al-Furqan", VersesCount = 77, JuzNumber = 18 },
                new QuranSurah { Number = 26, NameAr = "الشعراء", NameEn = "Ash-Shu'ara", VersesCount = 227, JuzNumber = 19 },
                new QuranSurah { Number = 27, NameAr = "النمل", NameEn = "An-Naml", VersesCount = 93, JuzNumber = 19 },
                new QuranSurah { Number = 28, NameAr = "القصص", NameEn = "Al-Qasas", VersesCount = 88, JuzNumber = 20 },
                new QuranSurah { Number = 29, NameAr = "العنكبوت", NameEn = "Al-Ankabut", VersesCount = 69, JuzNumber = 20 },
                new QuranSurah { Number = 30, NameAr = "الروم", NameEn = "Ar-Rum", VersesCount = 60, JuzNumber = 21 },
                new QuranSurah { Number = 31, NameAr = "لقمان", NameEn = "Luqman", VersesCount = 34, JuzNumber = 21 },
                new QuranSurah { Number = 32, NameAr = "السجدة", NameEn = "As-Sajdah", VersesCount = 30, JuzNumber = 21 },
                new QuranSurah { Number = 33, NameAr = "الأحزاب", NameEn = "Al-Ahzab", VersesCount = 73, JuzNumber = 21 },
                new QuranSurah { Number = 34, NameAr = "سبأ", NameEn = "Saba", VersesCount = 54, JuzNumber = 22 },
                new QuranSurah { Number = 35, NameAr = "فاطر", NameEn = "Fatir", VersesCount = 45, JuzNumber = 22 },
                new QuranSurah { Number = 36, NameAr = "يس", NameEn = "Ya-Sin", VersesCount = 83, JuzNumber = 22 },
                new QuranSurah { Number = 37, NameAr = "الصافات", NameEn = "As-Saffat", VersesCount = 182, JuzNumber = 23 },
                new QuranSurah { Number = 38, NameAr = "ص", NameEn = "Sad", VersesCount = 88, JuzNumber = 23 },
                new QuranSurah { Number = 39, NameAr = "الزمر", NameEn = "Az-Zumar", VersesCount = 75, JuzNumber = 23 },
                new QuranSurah { Number = 40, NameAr = "غافر", NameEn = "Ghafir", VersesCount = 85, JuzNumber = 24 },
                new QuranSurah { Number = 41, NameAr = "فصلت", NameEn = "Fussilat", VersesCount = 54, JuzNumber = 24 },
                new QuranSurah { Number = 42, NameAr = "الشورى", NameEn = "Ash-Shura", VersesCount = 53, JuzNumber = 25 },
                new QuranSurah { Number = 43, NameAr = "الزخرف", NameEn = "Az-Zukhruf", VersesCount = 89, JuzNumber = 25 },
                new QuranSurah { Number = 44, NameAr = "الدخان", NameEn = "Ad-Dukhan", VersesCount = 59, JuzNumber = 25 },
                new QuranSurah { Number = 45, NameAr = "الجاثية", NameEn = "Al-Jathiyah", VersesCount = 37, JuzNumber = 25 },
                new QuranSurah { Number = 46, NameAr = "الأحقاف", NameEn = "Al-Ahqaf", VersesCount = 35, JuzNumber = 26 },
                new QuranSurah { Number = 47, NameAr = "محمد", NameEn = "Muhammad", VersesCount = 38, JuzNumber = 26 },
                new QuranSurah { Number = 48, NameAr = "الفتح", NameEn = "Al-Fath", VersesCount = 29, JuzNumber = 26 },
                new QuranSurah { Number = 49, NameAr = "الحجرات", NameEn = "Al-Hujurat", VersesCount = 18, JuzNumber = 26 },
                new QuranSurah { Number = 50, NameAr = "ق", NameEn = "Qaf", VersesCount = 45, JuzNumber = 26 },
                new QuranSurah { Number = 51, NameAr = "الذاريات", NameEn = "Adh-Dhariyat", VersesCount = 60, JuzNumber = 26 },
                new QuranSurah { Number = 52, NameAr = "الطور", NameEn = "At-Tur", VersesCount = 49, JuzNumber = 27 },
                new QuranSurah { Number = 53, NameAr = "النجم", NameEn = "An-Najm", VersesCount = 62, JuzNumber = 27 },
                new QuranSurah { Number = 54, NameAr = "القمر", NameEn = "Al-Qamar", VersesCount = 55, JuzNumber = 27 },
                new QuranSurah { Number = 55, NameAr = "الرحمن", NameEn = "Ar-Rahman", VersesCount = 78, JuzNumber = 27 },
                new QuranSurah { Number = 56, NameAr = "الواقعة", NameEn = "Al-Waqi'ah", VersesCount = 96, JuzNumber = 27 },
                new QuranSurah { Number = 57, NameAr = "الحديد", NameEn = "Al-Hadid", VersesCount = 29, JuzNumber = 27 },
                new QuranSurah { Number = 58, NameAr = "المجادلة", NameEn = "Al-Mujadilah", VersesCount = 22, JuzNumber = 28 },
                new QuranSurah { Number = 59, NameAr = "الحشر", NameEn = "Al-Hashr", VersesCount = 24, JuzNumber = 28 },
                new QuranSurah { Number = 60, NameAr = "الممتحنة", NameEn = "Al-Mumtahanah", VersesCount = 13, JuzNumber = 28 },
                new QuranSurah { Number = 61, NameAr = "الصف", NameEn = "As-Saff", VersesCount = 14, JuzNumber = 28 },
                new QuranSurah { Number = 62, NameAr = "الجمعة", NameEn = "Al-Jumu'ah", VersesCount = 11, JuzNumber = 28 },
                new QuranSurah { Number = 63, NameAr = "المنافقون", NameEn = "Al-Munafiqun", VersesCount = 11, JuzNumber = 28 },
                new QuranSurah { Number = 64, NameAr = "التغابن", NameEn = "At-Taghabun", VersesCount = 18, JuzNumber = 28 },
                new QuranSurah { Number = 65, NameAr = "الطلاق", NameEn = "At-Talaq", VersesCount = 12, JuzNumber = 28 },
                new QuranSurah { Number = 66, NameAr = "التحريم", NameEn = "At-Tahrim", VersesCount = 12, JuzNumber = 28 },
                new QuranSurah { Number = 67, NameAr = "الملك", NameEn = "Al-Mulk", VersesCount = 30, JuzNumber = 29 },
                new QuranSurah { Number = 68, NameAr = "القلم", NameEn = "Al-Qalam", VersesCount = 52, JuzNumber = 29 },
                new QuranSurah { Number = 69, NameAr = "الحاقة", NameEn = "Al-Haqqah", VersesCount = 52, JuzNumber = 29 },
                new QuranSurah { Number = 70, NameAr = "المعارج", NameEn = "Al-Ma'arij", VersesCount = 44, JuzNumber = 29 },
                new QuranSurah { Number = 71, NameAr = "نوح", NameEn = "Nuh", VersesCount = 28, JuzNumber = 29 },
                new QuranSurah { Number = 72, NameAr = "الجن", NameEn = "Al-Jinn", VersesCount = 28, JuzNumber = 29 },
                new QuranSurah { Number = 73, NameAr = "المزمل", NameEn = "Al-Muzzammil", VersesCount = 20, JuzNumber = 29 },
                new QuranSurah { Number = 74, NameAr = "المدثر", NameEn = "Al-Muddathir", VersesCount = 56, JuzNumber = 29 },
                new QuranSurah { Number = 75, NameAr = "القيامة", NameEn = "Al-Qiyamah", VersesCount = 40, JuzNumber = 29 },
                new QuranSurah { Number = 76, NameAr = "الإنسان", NameEn = "Al-Insan", VersesCount = 31, JuzNumber = 29 },
                new QuranSurah { Number = 77, NameAr = "المرسلات", NameEn = "Al-Mursalat", VersesCount = 50, JuzNumber = 29 },
                new QuranSurah { Number = 78, NameAr = "النبأ", NameEn = "An-Naba", VersesCount = 40, JuzNumber = 30 },
                new QuranSurah { Number = 79, NameAr = "النازعات", NameEn = "An-Nazi'at", VersesCount = 46, JuzNumber = 30 },
                new QuranSurah { Number = 80, NameAr = "عبس", NameEn = "Abasa", VersesCount = 42, JuzNumber = 30 },
                new QuranSurah { Number = 81, NameAr = "التكوير", NameEn = "At-Takwir", VersesCount = 29, JuzNumber = 30 },
                new QuranSurah { Number = 82, NameAr = "الانفطار", NameEn = "Al-Infitar", VersesCount = 19, JuzNumber = 30 },
                new QuranSurah { Number = 83, NameAr = "المطففين", NameEn = "Al-Mutaffifin", VersesCount = 36, JuzNumber = 30 },
                new QuranSurah { Number = 84, NameAr = "الانشقاق", NameEn = "Al-Inshiqaq", VersesCount = 25, JuzNumber = 30 },
                new QuranSurah { Number = 85, NameAr = "البروج", NameEn = "Al-Buruj", VersesCount = 22, JuzNumber = 30 },
                new QuranSurah { Number = 86, NameAr = "الطارق", NameEn = "At-Tariq", VersesCount = 17, JuzNumber = 30 },
                new QuranSurah { Number = 87, NameAr = "الأعلى", NameEn = "Al-A'la", VersesCount = 19, JuzNumber = 30 },
                new QuranSurah { Number = 88, NameAr = "الغاشية", NameEn = "Al-Ghashiyah", VersesCount = 26, JuzNumber = 30 },
                new QuranSurah { Number = 89, NameAr = "الفجر", NameEn = "Al-Fajr", VersesCount = 30, JuzNumber = 30 },
                new QuranSurah { Number = 90, NameAr = "البلد", NameEn = "Al-Balad", VersesCount = 20, JuzNumber = 30 },
                new QuranSurah { Number = 91, NameAr = "الشمس", NameEn = "Ash-Shams", VersesCount = 15, JuzNumber = 30 },
                new QuranSurah { Number = 92, NameAr = "الليل", NameEn = "Al-Layl", VersesCount = 21, JuzNumber = 30 },
                new QuranSurah { Number = 93, NameAr = "الضحى", NameEn = "Ad-Duha", VersesCount = 11, JuzNumber = 30 },
                new QuranSurah { Number = 94, NameAr = "الشرح", NameEn = "Ash-Sharh", VersesCount = 8, JuzNumber = 30 },
                new QuranSurah { Number = 95, NameAr = "التين", NameEn = "At-Tin", VersesCount = 8, JuzNumber = 30 },
                new QuranSurah { Number = 96, NameAr = "العلق", NameEn = "Al-Alaq", VersesCount = 19, JuzNumber = 30 },
                new QuranSurah { Number = 97, NameAr = "القدر", NameEn = "Al-Qadr", VersesCount = 5, JuzNumber = 30 },
                new QuranSurah { Number = 98, NameAr = "البينة", NameEn = "Al-Bayyinah", VersesCount = 8, JuzNumber = 30 },
                new QuranSurah { Number = 99, NameAr = "الزلزلة", NameEn = "Az-Zalzalah", VersesCount = 8, JuzNumber = 30 },
                new QuranSurah { Number = 100, NameAr = "العاديات", NameEn = "Al-Adiyat", VersesCount = 11, JuzNumber = 30 },
                new QuranSurah { Number = 101, NameAr = "القارعة", NameEn = "Al-Qari'ah", VersesCount = 11, JuzNumber = 30 },
                new QuranSurah { Number = 102, NameAr = "التكاثر", NameEn = "At-Takathur", VersesCount = 8, JuzNumber = 30 },
                new QuranSurah { Number = 103, NameAr = "العصر", NameEn = "Al-Asr", VersesCount = 3, JuzNumber = 30 },
                new QuranSurah { Number = 104, NameAr = "الهمزة", NameEn = "Al-Humazah", VersesCount = 9, JuzNumber = 30 },
                new QuranSurah { Number = 105, NameAr = "الفيل", NameEn = "Al-Fil", VersesCount = 5, JuzNumber = 30 },
                new QuranSurah { Number = 106, NameAr = "قريش", NameEn = "Quraysh", VersesCount = 4, JuzNumber = 30 },
                new QuranSurah { Number = 107, NameAr = "الماعون", NameEn = "Al-Ma'un", VersesCount = 7, JuzNumber = 30 },
                new QuranSurah { Number = 108, NameAr = "الكوثر", NameEn = "Al-Kawthar", VersesCount = 3, JuzNumber = 30 },
                new QuranSurah { Number = 109, NameAr = "الكافرون", NameEn = "Al-Kafirun", VersesCount = 6, JuzNumber = 30 },
                new QuranSurah { Number = 110, NameAr = "النصر", NameEn = "An-Nasr", VersesCount = 3, JuzNumber = 30 },
                new QuranSurah { Number = 111, NameAr = "المسد", NameEn = "Al-Masad", VersesCount = 5, JuzNumber = 30 },
                new QuranSurah { Number = 112, NameAr = "الإخلاص", NameEn = "Al-Ikhlas", VersesCount = 4, JuzNumber = 30 },
                new QuranSurah { Number = 113, NameAr = "الفلق", NameEn = "Al-Falaq", VersesCount = 5, JuzNumber = 30 },
                new QuranSurah { Number = 114, NameAr = "الناس", NameEn = "An-Nas", VersesCount = 6, JuzNumber = 30 }
            };

            await context.QuranSurahs.AddRangeAsync(surahs);
            await context.SaveChangesAsync();
        }
    }
}
