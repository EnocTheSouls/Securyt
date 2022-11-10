using System.ComponentModel.DataAnnotations;

namespace Securyt.DTOs
{
    public class AgregarRolDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
    }
}
