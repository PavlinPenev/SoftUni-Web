using System.ComponentModel.DataAnnotations;

using static Store_Ge.Common.Constants.ValidationConstants;

namespace Store_Ge.Services.Models
{
    public class ApplicationUserRegisterDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [RegularExpression(PASSWORD_VALIDATION_PATTERN)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
