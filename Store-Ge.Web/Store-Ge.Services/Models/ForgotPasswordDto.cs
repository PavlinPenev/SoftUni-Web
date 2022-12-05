using System.ComponentModel.DataAnnotations;

namespace Store_Ge.Services.Models
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
