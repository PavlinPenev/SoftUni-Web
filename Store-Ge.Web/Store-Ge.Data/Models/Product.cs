using Store_Ge.Data.Enums;

using System.ComponentModel.DataAnnotations;

using static Store_Ge.Common.Constants.ValidationConstants;

namespace Store_Ge.Data.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(PRODUCT_NAME_MAX_LENGTH)]
        public string Name { get; set; }

        [Required]
        public MeasurementUnitEnum MeasurementUnit { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public int StoreId { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<Supplier> Suppliers { get; set; }

        public virtual Store Store { get; set; }
    }
}
