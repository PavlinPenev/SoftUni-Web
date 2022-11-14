using System.ComponentModel.DataAnnotations;

namespace Store_Ge.Services.Models.OrderModels
{
    public class StoreOrdersRequestDto : FilterRequestDto
    {
        [Required]
        public string StoreId { get; set; }
    }
}
