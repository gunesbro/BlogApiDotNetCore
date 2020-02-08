using System.ComponentModel.DataAnnotations;

namespace BlogProjectAPI.Data.Models
{
    public class Roles
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleDesc { get; set; }

    }
}
