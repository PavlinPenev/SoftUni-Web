using Store_Ge.Data.Enums;
using System.ComponentModel.DataAnnotations;
using static Store_Ge.Common.Constants.ValidationConstants;

namespace Store_Ge.Services.Models.ProductModels
{
    public class AddProductDto
    {
        public string? Id { get; set; }

        [Required]
        [MaxLength(PRODUCT_NAME_MAX_LENGTH)]
        public string Name { get; set; }

        [Required]
        public MeasurementUnitEnum MeasurementUnit { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        public decimal? PlusQuantity { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
