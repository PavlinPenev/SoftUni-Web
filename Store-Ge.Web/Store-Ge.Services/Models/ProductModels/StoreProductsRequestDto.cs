using System.ComponentModel.DataAnnotations;

namespace Store_Ge.Services.Models.ProductModels
{
    public class StoreProductsRequestDto : FilterRequestDto
    {
        [Required]
        public string StoreId { get; set; }
    }
}
