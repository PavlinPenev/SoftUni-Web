using Store_Ge.Services.Models;
using Store_Ge.Services.Models.SupplierModels;

namespace Store_Ge.Services.Services.SuppliersService
{
    public interface ISuppliersService
    {
        /// <summary>
        ///  Gets all the suppliers the user added
        /// </summary>
        /// <param name="userId"> The user's Id </param>
        /// <returns> The suppliers </returns>
        Task<List<AddOrderSupplierDto>> GetUserSuppliers(string userId);

        /// <summary>
        ///  Gets suppliers for the given the user
        /// </summary>
        /// <param name="request"> The request with the parameters needed for getting the suppliers </param>
        /// <returns> The suppliers(paged) </returns>
        Task<PagedList<UserSupplierDto>> GetUserSuppliersPaged(UserSuppliersRequestDto request);

        /// <summary>
        ///  Adds a supplier for all the stores in the user's store chain
        /// </summary>
        /// <param name="request"> The request containing the name of the supplier and the user's Id </param>
        /// <returns> If the addition was successful </returns>
        Task<bool> AddSupplier(AddSupplierRequestDto request);
    }
}
