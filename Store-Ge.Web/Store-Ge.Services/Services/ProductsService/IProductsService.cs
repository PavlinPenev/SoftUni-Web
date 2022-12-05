using Store_Ge.Services.Models;
using Store_Ge.Services.Models.ProductModels;

namespace Store_Ge.Services.Services.ProductsService
{
    public interface IProductsService
    {
        /// <summary>
        ///  Gets products for a given store
        /// </summary>
        /// <param name="request"> The request for getting the products including filtration options </param>
        /// <returns> The store's products(paged) </returns>
        Task<PagedList<ProductDto>> GetStoreProducts(StoreProductsRequestDto request);

        /// <summary>
        ///  Gets all products for a given store
        /// </summary>
        /// <param name="storeId"> The store's Id for which we get the products for </param>
        /// <returns> The store's products </returns>
        Task<List<ProductDto>> GetStoreAddProducts(string storeId);

        /// <summary>
        ///  Sells the customer's products
        /// </summary>
        /// <param name="request"> The request for selling the products </param>
        /// <returns> If the sale was successful </returns>
        Task<bool> SellProducts(SaleRequestDto request);

        /// <summary>
        ///  Upsert products for a given order 
        /// </summary>
        /// <param name="products"> The products to upsert </param>
        /// <param name="storeId"> The store's Id </param>
        Task UpsertProducts(List<AddProductDto> products, int storeId);
    }
}
