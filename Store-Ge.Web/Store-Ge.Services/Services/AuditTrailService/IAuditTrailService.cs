using Store_Ge.Services.Models.StoreModels;

namespace Store_Ge.Services.Services.AuditTrailService
{
    public interface IAuditTrailService
    {
        /// <summary>
        ///  Adds an 'Add Store' event into the audit trail
        /// </summary>
        /// <param name="addStoreDtoRequest"> The model of the request containing store info </param>
        /// <param name="storeId"> Id of the store </param>
        Task AddStore(AddStoreDto addStoreDtoRequest, int storeId);
    }
}
