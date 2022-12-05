using Store_Ge.Data.Models;
using Store_Ge.Services.Models.AuditTrailModels;
using Store_Ge.Services.Models.ProductModels;
using Store_Ge.Services.Models.StoreModels;

namespace Store_Ge.Services.Services.AuditTrailService
{
    public interface IAuditTrailService
    {
        /// <summary>
        ///  Gets a list with all the events for a given store
        /// </summary>
        /// <param name="storeId"> The store's Id </param>
        /// <returns> The events </returns>
        Task<List<AuditEventDto>> GetAll(int storeId);

        /// <summary>
        ///  Adds an 'Add Store' event into the audit trail
        /// </summary>
        /// <param name="addStoreDtoRequest"> The model of the request containing store info </param>
        /// <param name="storeId"> Id of the store </param>
        Task AddStore(AddStoreDto addStoreDtoRequest, int storeId);

        /// <summary>
        ///  Adds an 'Add Product Quantity' event into the audit trail
        /// </summary>
        /// <param name="product"> The product which the user added a certain amount to </param>
        /// <param name="storeId"> Id of the store </param>
        Task AddProductQuantity(AddProductDto product, int storeId);

        /// <summary>
        ///  Adds an 'Add Product' event into the audit trail
        /// </summary>
        /// <param name="product"> The product which the user added </param>
        /// <param name="storeId"> Id of the store </param>
        Task AddProduct(AddProductDto product, int storeId);

        /// <summary>
        ///  Adds a 'Sell Product' event into the audit trail
        /// </summary>
        /// <param name="product"> The product which the user sold </param>
        /// <param name="storeId"> Id of the store </param>
        Task SellProduct(AddProductDto product, int storeId);

        /// <summary>
        ///  Adds an 'Add Order' event into the audit trail
        /// </summary>
        /// <param name="order"> The order which the user added </param>
        /// <param name="storeId"> Id of the store </param>
        Task AddOrder(Order order, int storeId);

        /// <summary>
        ///  Adds an 'Add Supplier' event into the audit trail
        /// </summary>
        /// <param name="supplier"> The supplier added </param>
        /// <param name="storeIds"> The Ids of the stores the supplier was added to </param>
        Task AddSupplier(Supplier supplier, List<int> storeIds);
    }
}
