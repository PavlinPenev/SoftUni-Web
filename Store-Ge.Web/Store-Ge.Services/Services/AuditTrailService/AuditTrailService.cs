using Store_Ge.Data.Models;
using Store_Ge.Data.Repositories;
using Store_Ge.Services.Models.StoreModels;

namespace Store_Ge.Services.Services.AuditTrailService
{
    public class AuditTrailService : IAuditTrailService
    {
        private readonly IRepository<AuditEvent> auditTrailRepository;

        public AuditTrailService(IRepository<AuditEvent> auditTrailRepository)
        {
            this.auditTrailRepository = auditTrailRepository;
        }

        public async Task AddStore(AddStoreDto addStoreDtoRequest, int storeId)
        {
            var auditEvent = new AuditEvent
            {
                Action = nameof(AddStore),
                Description = $"Added Store with name {addStoreDtoRequest.Name} and type {addStoreDtoRequest.Type}.",
                StoreId = storeId
            };

            await auditTrailRepository.AddAsync(auditEvent);
            await auditTrailRepository.SaveChangesAsync();  
        }
    }
}
