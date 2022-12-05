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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStoreProducts([FromBody] StoreProductsRequestDto request)
        {
            var products = await productsService.GetStoreProducts(request);
            if (products == null)
            {
                return NotFound();
            }

            return Ok(products);
        }

        [HttpGet]
        [Route(Routes.GET_STORE_ADD_PRODUCTS_ENDPOINT)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStoreAddProducts([FromQuery] string storeId)
        {
            var products = await productsService.GetStoreAddProducts(storeId);
            if (products == null)
            {
                return NotFound();
            }

            return Ok(products);
        }

        [HttpPost]
        [Route(Routes.SELL_PRODUCTS_ENDPOINT)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SellProducts([FromBody] SaleRequestDto request)
        {
            var result = await productsService.SellProducts(request);
            if (!result)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
