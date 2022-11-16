using Store_Ge.Services.Models.ProductModels;
using System.ComponentModel.DataAnnotations;

namespace Store_Ge.Services.Models.OrderModels
{
    public class AddOrderRequestDto
    {
        [Required]
        public string StoreId { get; set; }

        [Required]
        public int OrderNumber { get; set; }

        [Required]
        public List<AddProductDto> Products { get; set; }

        [Required]
        public string SupplierId { get; set; }
    }
}
