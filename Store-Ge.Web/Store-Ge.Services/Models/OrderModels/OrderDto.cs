namespace Store_Ge.Services.Models.OrderModels
{
    public class OrderDto
    {
        public string Id { get; set; }

        public int OrderNumber { get; set; }

        public string SupplierName { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
