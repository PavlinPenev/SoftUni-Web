using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetUserSuppliers([FromQuery] string userId)
        {
            var suppliers = await suppliersService.GetUserSuppliers(userId);
            if (suppliers == null)
            {
                return NotFound();
            }

            return Ok(suppliers);
        }
    }
}
