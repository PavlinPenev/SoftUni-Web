namespace Store_Ge.Data.Models
{
    public class AuditEvent
    {
        public int Id { get; set; }

        public string Action { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public int StoreId { get; set; }

        public Store Store { get; set; }
    }
}
