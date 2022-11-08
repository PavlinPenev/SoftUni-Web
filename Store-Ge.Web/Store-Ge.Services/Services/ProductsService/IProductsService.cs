using Store_Ge.Services.Models;
using Store_Ge.Services.Models.ProductModels;

namespace Store_Ge.Services.Services.ProductsService
{
    public interface IProductsService
    {
        /// <summary>
        ///  Gets all products for a given store
        /// </summary>
        /// <param name="request"> The request for getting the products including filtration options </param>
        /// <returns> The store's products(paged) </returns>
        Task<PagedList<ProductDto>> GetStoreProducts(StoreProductsRequestDto request);
    }
}
