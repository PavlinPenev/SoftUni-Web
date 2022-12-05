using System.ComponentModel.DataAnnotations;

using static Store_Ge.Common.Constants.ValidationConstants;

namespace Store_Ge.Services.Models
{
    public class ApplicationUserLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(PASSWORD_VALIDATION_PATTERN)]
        public string Password { get; set; }
    }
}
