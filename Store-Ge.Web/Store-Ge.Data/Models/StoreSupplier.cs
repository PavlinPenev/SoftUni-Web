namespace Store_Ge.Data.Models
{
    public class StoreSupplier
    {
        public int StoreId { get; set; }

        public int SupplierId { get; set; }

        public virtual Store Store { get; set; }

        public virtual Supplier Supplier { get; set; }
    }
}
