using Store_Ge.Data.Enums;
using System.ComponentModel.DataAnnotations;
using static Store_Ge.Data.Constants.ValidationConstants;

namespace Store_Ge.Data.Models
{
    public class Product
    {
        public Product()
        {
            StoresProducts = new HashSet<StoreProduct>();
            OrdersProducts = new HashSet<OrderProduct>();
            SuppliersProducts = new HashSet<SupplierProduct>();
        }

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

        public virtual ICollection<StoreProduct> StoresProducts { get; set; }

        public virtual ICollection<OrderProduct> OrdersProducts { get; set; }

        public virtual ICollection<SupplierProduct> SuppliersProducts { get; set; }
    }
}
