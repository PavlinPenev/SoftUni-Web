namespace Store_Ge.Services.Models.ProductModels
{
    public class SaleRequestDto
    {
        public string StoreId { get; set; }

        public List<AddProductDto> Products { get; set; }
    }
}
