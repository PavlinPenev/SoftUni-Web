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
    }
}
