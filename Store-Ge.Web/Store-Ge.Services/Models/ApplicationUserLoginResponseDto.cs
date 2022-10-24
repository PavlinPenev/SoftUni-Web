using static Store_Ge.Common.Constants.ValidationConstants;

using System.ComponentModel.DataAnnotations;

namespace Store_Ge.Services.Models
{
    public class ApplicationUserLoginResponseDto
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
