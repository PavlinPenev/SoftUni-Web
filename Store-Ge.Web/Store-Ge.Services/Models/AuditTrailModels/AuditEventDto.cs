namespace Store_Ge.Services.Models.AuditTrailModels
{
    public class AuditEventDto
    {
        public string Action { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
