using AutoMapper;
using LinqKit;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Store_Ge.Data.Models;
using Store_Ge.Data.Repositories;
using Store_Ge.Services.Configurations;
using Store_Ge.Services.Models;
using Store_Ge.Services.Models.OrderModels;
using Store_Ge.Services.Services.AuditTrailService;
using Store_Ge.Services.Services.ProductsService;
using System.Linq.Expressions;
using System.Reflection;

namespace Store_Ge.Services.Services.OrdersService
{
    public class OrdersService : IOrdersService
    {
        private readonly StoreGeAppSettings appSettings;
        private readonly IDataProtector dataProtector;
        private readonly IProductsService productsService;
        private readonly IRepository<UserStore> userStoreRepository;
        private readonly IAuditTrailService auditTrailService;
        private readonly IRepository<Order> orderRepository;
        private readonly IMapper mapper;

        public OrdersService(
            IOptions<StoreGeAppSettings> appSettings,
            IDataProtectionProvider dataProtectionProvider,
            IProductsService productsService,
            IRepository<UserStore> userStoreRepository,
            IAuditTrailService auditTrailService,
            IRepository<Order> orderRepository,
            IMapper mapper)
        {
            this.appSettings = appSettings.Value;
            this.dataProtector = dataProtectionProvider.CreateProtector(this.appSettings.DataProtectionKey);
            this.productsService = productsService;
            this.userStoreRepository = userStoreRepository;
            this.auditTrailService = auditTrailService;
            this.orderRepository = orderRepository;
            this.mapper = mapper;
        }

        public async Task<PagedList<UserOrderDto>> GetUserOrders(UserOrdersRequestDto request)
        {
            var decryptedUserId = int.Parse(dataProtector.Unprotect(request.UserId));

            var storeIds = await userStoreRepository
                .GetAll()
                .Where(x => x.UserId == decryptedUserId)
                .Select(x => x.StoreId)
                .ToListAsync();

            var orders = await orderRepository
                .GetAll()
                .Include(x => x.Store)
                .Where(x => storeIds.Contains(x.StoreId))
                .ToListAsync();

            var mappedOrders = mapper.Map<List<UserOrderDto>>(orders);

            mappedOrders = FilterUserOrders(mappedOrders, request);
            mappedOrders.ForEach(x => x.Id = dataProtector.Protect(x.Id));

            var pagedOrders = new PagedList<UserOrderDto>(mappedOrders, orders.Count);

            return pagedOrders;
        }


        public async Task<PagedList<OrderDto>> GetStoreOrders(StoreOrdersRequestDto request)
        {
            var decodedStoreId = dataProtector.Unprotect(request.StoreId);

            var orders = await orderRepository
                .GetAll()
                .Include(x => x.Supplier)
                .Where(x => x.StoreId == int.Parse(decodedStoreId))
                .ToListAsync();

            var mappedOrders = mapper.Map<List<OrderDto>>(orders);
            foreach (var order in mappedOrders)
            {
                var dbModel = orders.FirstOrDefault(x => x.Id == int.Parse(order.Id));
                order.SupplierName = dbModel.Supplier.Name;
                order.Id = dataProtector.Protect(order.Id);
            }
            mappedOrders = FilterOrders(mappedOrders, request);

            var pagedOrders = new PagedList<OrderDto>(mappedOrders, orders.Count);

            return pagedOrders;
        }

        public async Task<bool> AddOrder(AddOrderRequestDto request)
        {
            var decryptedStoreId = int.Parse(dataProtector.Unprotect(request.StoreId));
            var decryptedSupplierId = int.Parse(dataProtector.Unprotect(request.SupplierId));

            var order = new Order
            {
                OrderNumber = request.OrderNumber,
                StoreId = decryptedStoreId,
                SupplierId = decryptedSupplierId
            };

            await orderRepository.AddAsync(order);
            await orderRepository.SaveChangesAsync();

            var savedOrder = await orderRepository.GetAll().FirstOrDefaultAsync(x => x.OrderNumber == request.OrderNumber);
            if (savedOrder == null)
            {
                return false;
            }

            await auditTrailService.AddOrder(order, decryptedStoreId);

            await productsService.UpsertProducts(request.Products, decryptedStoreId);

            return true;
        }

        #region Private Methods
        private List<OrderDto> FilterOrders(List<OrderDto> ordersToFilter, StoreOrdersRequestDto request)
        {
            Expression<Func<OrderDto, bool>> filterTemplate = x => true;

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                filterTemplate = filterTemplate.And(x =>
                    x.OrderNumber.ToString().Contains(request.SearchTerm)
                    || x.SupplierName.Contains(request.SearchTerm));
            }

            if (request.DateAddedFrom.HasValue || request.DateAddedTo.HasValue)
            {
                if (request.DateAddedFrom.HasValue && !request.DateAddedTo.HasValue)
                {
                    filterTemplate = filterTemplate.And(x => x.CreatedOn >= request.DateAddedFrom);
                }
                else if (!request.DateAddedFrom.HasValue && request.DateAddedTo.HasValue)
                {
                    filterTemplate = filterTemplate.And(x => x.CreatedOn <= request.DateAddedTo);
                }
                else
                {
                    filterTemplate = filterTemplate.And(x =>
                        x.CreatedOn >= request.DateAddedFrom && x.CreatedOn <= request.DateAddedTo);
                }
            }

            ordersToFilter = ordersToFilter.Where(filterTemplate.Compile()).ToList();

            if (!string.IsNullOrEmpty(request.OrderBy))
            {
                var flags = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public;

                var orderByPropertyInfo = typeof(OrderDto).GetProperty(request.OrderBy, flags);

                if (!request.IsDescending)
                {
                    ordersToFilter = ordersToFilter
                        .OrderBy(x => orderByPropertyInfo.GetValue(x)).ToList();
                }
                else
                {
                    ordersToFilter = ordersToFilter
                        .OrderByDescending(x => orderByPropertyInfo.GetValue(x)).ToList();
                }
            }

            return ordersToFilter.Skip(request.Skip).Take(request.Take).ToList();
        }

        private List<UserOrderDto> FilterUserOrders(List<UserOrderDto> ordersToFilter, UserOrdersRequestDto request)
        {
            Expression<Func<UserOrderDto, bool>> filterTemplate = x => true;

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                filterTemplate = filterTemplate.And(x =>
                    x.OrderNumber.ToString().Contains(request.SearchTerm)
                    || x.StoreName.Contains(request.SearchTerm));
            }

            if (request.DateAddedFrom.HasValue || request.DateAddedTo.HasValue)
            {
                if (request.DateAddedFrom.HasValue && !request.DateAddedTo.HasValue)
                {
                    filterTemplate = filterTemplate.And(x => x.CreatedOn >= request.DateAddedFrom);
                }
                else if (!request.DateAddedFrom.HasValue && request.DateAddedTo.HasValue)
                {
                    filterTemplate = filterTemplate.And(x => x.CreatedOn <= request.DateAddedTo);
                }
                else
                {
                    filterTemplate = filterTemplate.And(x =>
                        x.CreatedOn >= request.DateAddedFrom && x.CreatedOn <= request.DateAddedTo);
                }
            }

            ordersToFilter = ordersToFilter.Where(filterTemplate.Compile()).ToList();

            if (!string.IsNullOrEmpty(request.OrderBy))
            {
                var flags = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public;

                var orderByPropertyInfo = typeof(UserOrderDto).GetProperty(request.OrderBy, flags);

                if (!request.IsDescending)
                {
                    ordersToFilter = ordersToFilter
                        .OrderBy(x => orderByPropertyInfo.GetValue(x)).ToList();
                }
                else
                {
                    ordersToFilter = ordersToFilter
                        .OrderByDescending(x => orderByPropertyInfo.GetValue(x)).ToList();
                }
            }

            return ordersToFilter.Skip(request.Skip).Take(request.Take).ToList();
        }
        #endregion
    }
}
