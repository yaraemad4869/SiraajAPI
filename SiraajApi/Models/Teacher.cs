using System;

namespace SiraajApi.Models
{
    public class Teacher
    {
        public Guid UserId { get; set; }
        public string Specialization { get; set; } = string.Empty;
        public string StudentIds { get; set; } = "[]";
        
        public virtual User? User { get; set; }
    }
}
