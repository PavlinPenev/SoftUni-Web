using Store_Ge.Services.Models.StoreModels;

namespace Store_Ge.Services.Services.StoresService
{
    public interface IStoresService
    {
        /// <summary>
        ///  Gets all the stores for the user by given his id
        /// </summary>
        /// <param name="userId"> The user's Id </param>
        /// <returns> All the stores for the user </returns>
        Task<List<StoreDto>> GetStores(string userId);

        /// <summary>
        ///  Gets a store by user Id and store name
        /// </summary>
        /// <param name="userId"> The user's Id </param>
        /// <param name="name"> The store's name </param>
        /// <returns> The store </returns>
        Task<StoreDto> GetStoreByNameAndUserId(string userId, string name);

        /// <summary>
        /// Gets a store by its Id
        /// </summary>
        /// <param name="storeId"> The store's Id </param>
        /// <returns> The store </returns>
        Task<StoreDto> GetStore(string storeId);

        /// <summary>
        ///  Adds a store to the user's stores
        /// </summary>
        /// <param name="addStoreDto"> The add store request model containing request params(UserId, Name, Type) </param>
        /// <returns> If the addition was successful </returns>
        Task<bool> AddStore(AddStoreDto addStoreDto);

        /// <summary>
        ///  Gets an excel file with events ocurred for the given store
        /// </summary>
        /// <param name="storeId"> The store's Id </param>
        /// <returns> The file </returns>
        Task<byte[]> GetReportFile(string storeId);
    }
}
