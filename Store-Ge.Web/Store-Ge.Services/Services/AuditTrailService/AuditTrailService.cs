using Store_Ge.Data.Models;
using Store_Ge.Data.Repositories;
using Store_Ge.Services.Models.ProductModels;
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

        public async Task AddProduct(AddProductDto product, int storeId)
        {
            var auditEvent = new AuditEvent
            {
                Action = nameof(AddProduct),
                Description = $"Added a product with name {product.Name} with quantity {product.Quantity}, price {product.Price} and a measurementUnit {product.MeasurementUnit}.",
                StoreId = storeId
            };

            await auditTrailRepository.AddAsync(auditEvent);
            await auditTrailRepository.SaveChangesAsync();
        }

        public async Task AddProductQuantity(AddProductDto product, int storeId)
        {
            var auditEvent = new AuditEvent
            {
                Action = nameof(AddProductQuantity),
                Description = $"Added {product.PlusQuantity} quantity to product with name {product.Name}, price {product.Price} and a measurementUnit {product.MeasurementUnit}.",
                StoreId = storeId
            };

            await auditTrailRepository.AddAsync(auditEvent);
            await auditTrailRepository.SaveChangesAsync();
        }

        public async Task AddOrder(Order order, int storeId)
        {
            var auditEvent = new AuditEvent
            {
                Action = nameof(AddOrder),
                Description = $"Added order with number {order.OrderNumber}.",
                StoreId = storeId
            };

            await auditTrailRepository.AddAsync(auditEvent);
            await auditTrailRepository.SaveChangesAsync();
        }

        public async Task AddSupplier(Supplier supplier, List<int> storeIds)
        {
            foreach (var storeId in storeIds)
            {
                var auditEvent = new AuditEvent
                {
                    Action = nameof(AddSupplier),
                    Description = $"Added supplier with name {supplier.Name}.",
                    StoreId = storeId
                };

                await auditTrailRepository.AddAsync(auditEvent);
            }

            await auditTrailRepository.SaveChangesAsync();
        }
    }
}
