using Store_Ge.Data.Enums;

using System.ComponentModel.DataAnnotations;

using static Store_Ge.Common.Constants.ValidationConstants;

namespace Store_Ge.Data.Models
{
    public class Store
    {
        public Store()
        {
            UsersStores = new HashSet<UserStore>();
            Orders = new HashSet<Order>();
            Products = new HashSet<Product>();
            AuditEvents = new HashSet<AuditEvent>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(STORE_NAME_MAX_LENGTH)]
        public string Name { get; set; }

        [Required]
        public StoreTypeEnum Type { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<UserStore> UsersStores { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<AuditEvent> AuditEvents { get; set; }
    }
}
