using Microsoft.AspNetCore.Mvc;
using Store_Ge.Services.Models.ProductModels;
using Store_Ge.Services.Services.ProductsService;
using static Store_Ge.Web.Constants.Constants.Products;

namespace Store_Ge.Web.Controllers
{
    [Route(Routes.PRODUCTS_CONTROLLER)]
    public class ProductsController : StoreGeBaseController
    {
        private readonly IProductsService productsService;

        public ProductsController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        [HttpPost]
        [Route(Routes.GET_STORE_PRODUCTS_ENDPOINT)]
        public async Task<IActionResult> GetStoreProducts([FromBody] StoreProductsRequestDto request)
        {
            var products = await productsService.GetStoreProducts(request);
            if (products == null)
            {
                return NotFound();
            }

            return Ok(products);
        }
    }
}
