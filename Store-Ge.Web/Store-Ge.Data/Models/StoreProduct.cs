namespace Store_Ge.Data.Models
{
    public class StoreProduct
    {
        public int StoreId { get; set; }

        public int ProductId { get; set; }

        public virtual Store Store { get; set; }

        public virtual Product Product { get; set; }
    }
}
