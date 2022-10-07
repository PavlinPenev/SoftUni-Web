using System.ComponentModel.DataAnnotations;

using static Store_Ge.Common.Constants.ValidationConstants;

namespace Store_Ge.Data.Models
{
    public class Supplier
    {
        public Supplier()
        {
            Orders = new HashSet<Order>();
            StoresSuppliers = new HashSet<StoreSupplier>();
            Products = new HashSet<Product>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(SUPPLIER_NAME_MAX_LENGTH)]
        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<StoreSupplier> StoresSuppliers { get; set; }
    }
}
