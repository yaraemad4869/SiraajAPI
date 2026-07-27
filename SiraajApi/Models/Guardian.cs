using System;

namespace SiraajApi.Models
{
    public class Guardian
    {
        public Guid UserId { get; set; }
        public string ChildrenIds { get; set; } = "[]";
        
        public virtual User? User { get; set; }
    }
}
