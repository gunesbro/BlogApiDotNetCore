using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogProjectAPI.Data.Models
{
    public class TokenUser
    {
        [Key]
        public int TokenUserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsSuspended { get; set; } = false;
        [ForeignKey("Roles")]
        public int RoleId { get; set; }
        public virtual Roles Roles { get; set; }
        public string UserDesc { get; set; }
    }
}
