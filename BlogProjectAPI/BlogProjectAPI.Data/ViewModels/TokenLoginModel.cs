using System.ComponentModel.DataAnnotations;

namespace BlogProjectAPI.Data.ViewModels
{
    public class TokenLoginModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
