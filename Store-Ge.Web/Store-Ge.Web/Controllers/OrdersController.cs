using Microsoft.AspNetCore.Mvc;
using Store_Ge.Services.Models.OrderModels;
using Store_Ge.Services.Services.OrdersService;
using static Store_Ge.Web.Constants.Constants.Orders;

namespace Store_Ge.Web.Controllers
{
    [Route(Routes.ORDERS_CONTROLLER)]
    public class OrdersController : StoreGeBaseController
    {
        private readonly IOrdersService ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            this.ordersService = ordersService;
        }

        [HttpPost]
        [Route(Routes.GET_USER_ORDERS_ENDPOINT)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserOrders(UserOrdersRequestDto request)
        {
            var orders = await ordersService.GetUserOrders(request);
            if (orders == null)
            {
                return NotFound();
            }

            return Ok(orders);
        }

        [HttpPost]
        [Route(Routes.GET_STORE_ORDERS_ENDPOINT)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStoreOrders(StoreOrdersRequestDto request) 
        {
            var orders = await ordersService.GetStoreOrders(request);
            if (orders == null)
            {
                return NotFound();
            }

            return Ok(orders);
        }

        [HttpPost]
        [Route(Routes.ADD_ORDER_ENDPOINT)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddOrder([FromBody] AddOrderRequestDto request)
        {
            var result = await ordersService.AddOrder(request);
            if (!result)
            {
                return BadRequest();
            }

            return Ok(result);
        }
    }
}
