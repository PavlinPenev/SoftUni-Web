namespace Store_Ge.Data.Models
{
    public class UserStore
    {
        public int UserId { get; set; }

        public int StoreId { get; set; }

        public virtual User User { get; set; }

        public virtual Store Store { get; set; }
    }
}
