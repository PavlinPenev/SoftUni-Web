using static Store_Ge.Common.Constants.ValidationConstants;

using System.ComponentModel.DataAnnotations;

namespace Store_Ge.Services.Models
{
    public class ApplicationUserLoginResponseDto
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(PASSWORD_VALIDATION_PATTERN)]
        public string Password { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
