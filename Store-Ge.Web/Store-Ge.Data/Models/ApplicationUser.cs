using Microsoft.AspNetCore.Identity;

namespace Store_Ge.Data.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public ApplicationUser()
        {
            UsersStores = new HashSet<UserStore>();
            Roles = new HashSet<IdentityUserRole<int>>();
            Claims = new HashSet<IdentityUserClaim<int>>();
            Logins = new HashSet<IdentityUserLogin<int>>();
        }

        public string? AccessToken { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpirationDate { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<UserStore> UsersStores { get; set; }

        public virtual ICollection<IdentityUserRole<int>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<int>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<int>> Logins { get; set; }
    }
}
