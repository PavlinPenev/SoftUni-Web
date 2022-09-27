namespace Store_Ge.Data.Models
{
    public class UserStore
    {
        public int UserId { get; set; }

        public int StoreId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Store Store { get; set; }
    }
}
