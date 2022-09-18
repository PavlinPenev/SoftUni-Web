namespace Store_Ge.Data.Models
{
    public class SupplierProduct
    {
        public int SupplierId { get; set; }

        public int ProductId { get; set; }

        public virtual Supplier Supplier { get; set; }

        public virtual Product Product { get; set; }
    }
}
