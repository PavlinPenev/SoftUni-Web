using System.ComponentModel.DataAnnotations;

namespace Store_Ge.Data.Models
{
    public class Order
    {
        public Order()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderNumber { get; set; }

        public int SupplierId { get; set; }

        public int StoreId { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual Supplier Supplier { get; set; }

        public virtual Store Store { get; set; }

        public virtual ICollection<Product> Products { get; set; } 
    }
}
