using Store_Ge.Data.Enums;
using System.ComponentModel.DataAnnotations;
using static Store_Ge.Data.Constants.ValidationConstants;

namespace Store_Ge.Data.Models
{
    public class Store
    {
        public Store()
        {
            UsersStores = new HashSet<UserStore>();
            StoresProducts = new HashSet<StoreProduct>();
            Orders = new HashSet<Order>();
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

        public virtual ICollection<StoreProduct> StoresProducts { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
