using Store_Ge.Services.Models.ProductModels;

namespace Store_Ge.Services.Models.OrderModels
{
    public class AddOrderRequestDto
    {
        public string StoreId { get; set; }

        public int OrderNumber { get; set; }

        public List<AddProductDto> Products { get; set; }

        public string SupplierId { get; set; }
    }
}
