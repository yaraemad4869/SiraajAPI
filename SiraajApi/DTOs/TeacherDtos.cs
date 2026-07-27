using System;

namespace SiraajApi.DTOs
{
    public class TeacherDto
    {
        public Guid Id { get; set; }
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string StudentIds { get; set; } = "[]";
        public string JoinDate { get; set; } = string.Empty;
    }

    public class CreateTeacherRequest
    {
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
    }

    public class UpdateTeacherRequest
    {
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
    }
}
