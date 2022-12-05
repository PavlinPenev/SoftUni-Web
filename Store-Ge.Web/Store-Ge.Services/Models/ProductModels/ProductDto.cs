using Store_Ge.Data.Enums;

namespace Store_Ge.Services.Models.ProductModels
{
    public class ProductDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public MeasurementUnitEnum MeasurementUnit { get; set; }

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
