using System.ComponentModel.DataAnnotations;

namespace Store_Ge.Services.Models
{
    public class ConfirmEmailDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string EmailToken { get; set; }
    }
}
