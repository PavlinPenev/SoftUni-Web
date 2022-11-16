using System.ComponentModel.DataAnnotations;

namespace Store_Ge.Services.Models.OrderModels
{
    public class UserOrdersRequestDto : FilterRequestDto
    {
        [Required]
        public string UserId { get; set; }
    }
}
