using Microsoft.AspNetCore.Mvc;
using Store_Ge.Services.Models.StoreModels;
using Store_Ge.Services.Services.AuditTrailService;
using Store_Ge.Services.Services.StoresService;
using static Store_Ge.Web.Constants.Constants.Stores;

namespace Store_Ge.Web.Controllers
{
    [Route(Routes.STORES_CONTROLLER)]
    public class StoresController : StoreGeBaseController
    {
        private readonly IStoresService storesService;
        private readonly IAuditTrailService auditTrailService;

        public StoresController(IStoresService storesService, IAuditTrailService auditTrailService)
        {
            this.storesService = storesService;
            this.auditTrailService = auditTrailService;
        }

        [HttpGet]
        [Route(Routes.GET_USER_STORES_ENDPOINT)]
        public async Task<IActionResult> GetStores([FromQuery] string userId)
        {
            var result = await storesService.GetStores(userId);

            if (!result.Any() && result == null)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpPost]
        [Route(Routes.ADD_STORE)]
        public async Task<IActionResult> AddStore([FromBody] AddStoreDto addStoreRequest)
        {
            var result = await storesService.AddStore(addStoreRequest);

            if (!result)
            {
                return BadRequest(result);
            }

            var store = await storesService.GetStoreByNameAndUserId(addStoreRequest.UserId, addStoreRequest.Name);

            await auditTrailService.AddStore(addStoreRequest, int.Parse(store.Id));

            return StatusCode(201);
        }
    }
}
