using Store_Ge.Services.Models;
using Store_Ge.Services.Models.OrderModels;

namespace Store_Ge.Services.Services.OrdersService
{
    public interface IOrdersService
    {
        /// <summary>
        ///  Gets all the given orders for the given user
        /// </summary>
        /// <param name="request"> A request model with all neccessary parameters for the filtration and the sorting </param>
        /// <returns> A paged collection with the user's orders </returns>
        Task<PagedList<UserOrderDto>> GetUserOrders(UserOrdersRequestDto request);

        /// <summary>
        ///  Gets all the orders for the given store
        /// </summary>
        /// <param name="request"> A request model with all neccessary parameters for the filtration and the sorting </param>
        /// <returns> A paged collection with the store's orders </returns>
        Task<PagedList<OrderDto>> GetStoreOrders(StoreOrdersRequestDto request);

        /// <summary>
        ///  Adds an order to the database
        /// </summary>
        /// <param name="request"> The request with all the necessary parameters </param>
        /// <returns> If the addition was successful </returns>
        Task<bool> AddOrder(AddOrderRequestDto request);
    }
}
