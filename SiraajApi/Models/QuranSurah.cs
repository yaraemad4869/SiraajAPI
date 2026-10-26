using System;

namespace SiraajApi.Models
{
    public class QuranSurah
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public int VersesCount { get; set; }
        public int JuzNumber { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
