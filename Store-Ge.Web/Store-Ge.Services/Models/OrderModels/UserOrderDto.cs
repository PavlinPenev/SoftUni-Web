namespace Store_Ge.Services.Models.OrderModels
{
    public class UserOrderDto
    {
        public string Id { get; set; }

        public string StoreName { get; set; }

        public int OrderNumber { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
