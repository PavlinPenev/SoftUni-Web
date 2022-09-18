using Store_Ge.Data.Enums;
using System.ComponentModel.DataAnnotations;
using static Store_Ge.Data.Constants.ValidationConstants;

namespace Store_Ge.Data.Models
{
    public class User
    {
        public User()
        {
            UsersStores = new HashSet<UserStore>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [RegularExpression(PASSWORD_VALIDATION_PATTERN)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(EMAIL_MAX_LENGTH)]
        public string Email { get; set; }

        [Required]
        public RoleTypeEnum Role { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<UserStore> UsersStores { get; set; }
    }
}
