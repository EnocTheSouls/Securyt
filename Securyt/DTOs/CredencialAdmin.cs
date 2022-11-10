using System.ComponentModel.DataAnnotations;

namespace Securyt.DTOs
{
    public class CredencialAdmin
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
