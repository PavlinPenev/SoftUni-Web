using Microsoft.AspNetCore.Mvc;
using Store_Ge.Services.Models.SupplierModels;
using Store_Ge.Services.Services.SuppliersService;
using static Store_Ge.Web.Constants.Constants.Suppliers;

namespace Store_Ge.Web.Controllers
{
    [Route(Routes.SUPPLIERS_CONTROLLER)]
    public class SuppliersController : StoreGeBaseController
    {
        private readonly ISuppliersService suppliersService;

        public SuppliersController(ISuppliersService suppliersService)
        {
            this.suppliersService = suppliersService;
        }

        [HttpGet]
        [Route(Routes.GET_USER_SUPPLIERS_ENDPOINT)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserSuppliers([FromQuery] string userId)
        {
            var suppliers = await suppliersService.GetUserSuppliers(userId);
            if (suppliers == null)
            {
                return NotFound();
            }

            return Ok(suppliers);
        }

        [HttpPost]
        [Route(Routes.GET_USER_SUPPLIERS_PAGED_ENDPOINT)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserSuppliersPaged([FromBody] UserSuppliersRequestDto request)
        {
            var suppliers = await suppliersService.GetUserSuppliersPaged(request);
            if (suppliers == null)
            {
                return BadRequest();
            }

            return Ok(suppliers);
        }

        [HttpPost]
        [Route(Routes.ADD_SUPPLIER_ENDPOINT)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddSupplier([FromBody] AddSupplierRequestDto request)
        {
            var result = await suppliersService.AddSupplier(request);
            if (!result)
            {
                return BadRequest();
            }

            return Ok(result);
        }
    }
}
