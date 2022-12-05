using NUnit.Framework;
using Store_Ge.Data.Enums;
using Store_Ge.Services.Models.OrderModels;
using Store_Ge.Services.Models.ProductModels;
using Store_Ge.Services.Services.AuditTrailService;
using Store_Ge.Services.Services.OrdersService;
using Store_Ge.Services.Services.ProductsService;
using Store_Ge.Services.Services.StoresService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store_Ge.Tests.Services
{
    public class OrdersServiceTests : TestsSetUp
    {
        private IOrdersService ordersService;
        private IAuditTrailService auditTrailService;
        private IProductsService productsService;
        private IStoresService storesService;

        [SetUp]
        public async Task Setup()
        {
            await InitializeDbContext();

            var storeGeAppSettings = GetStoreGeAppSettings();
            var dataProtectionProvider = GetProtectionProvider();
            var mapper = GetAutoMapper();
            var userStoreRepository = GetUserStoreRepository();
            var auditTrailRepository = GetAuditTrailRepository();
            var orderRepository = GetOrderRepository();
            var productRepository = GetProductRepository();
            var storeRepository = GetStoreRepository();
            var userManager = GetUserManager();

            auditTrailService = new AuditTrailService(
                auditTrailRepository,
                mapper);

            storesService = new StoresService(
                storeRepository,
                auditTrailService,
                userStoreRepository,
                userManager,
                dataProtectionProvider,
                storeGeAppSettings,
                mapper);

            productsService = new ProductsService(
                dataProtectionProvider,
                productRepository,
                auditTrailService,
                storesService,
                mapper,
                storeGeAppSettings);

            ordersService = new OrdersService(
                storeGeAppSettings,
                dataProtectionProvider,
                productsService,
                userStoreRepository,
                auditTrailService,
                orderRepository,
                mapper);
        }

        [Test]
        public async Task GetUserOrdersReturnsCorrectResult()
        {
            var request = new UserOrdersRequestDto
            {
                UserId = MOCK_USER_ID,
                SearchTerm = string.Empty,
                OrderBy = "orderNumber",
                IsDescending = false,
                DateAddedFrom = null,
                DateAddedTo = null,
                Skip = 0,
                Take = 5
            };

            var result = await ordersService.GetUserOrders(request);

            Assert.AreEqual(3, result.TotalItemsCount);
            Assert.AreEqual(3, result.Items.Count);
        }

        [Test]
        public async Task GetStoreOrdersReturnsCorrectResult()
        {
            var request = new StoreOrdersRequestDto
            {
                StoreId = MOCK_STORE_ID,
                SearchTerm = string.Empty,
                OrderBy = "orderNumber",
                IsDescending = false,
                DateAddedFrom = null,
                DateAddedTo = null,
                Skip = 0,
                Take = 5
            };

            var result = await ordersService.GetStoreOrders(request);

            Assert.AreEqual(2, result.TotalItemsCount);
            Assert.AreEqual(2, result.Items.Count);
        }

        [Test]
        public async Task AddOrderAddsOrder()
        {
            var request = new AddOrderRequestDto
            {
                OrderNumber = 456321,
                StoreId = MOCK_STORE_ID,
                SupplierId = MOCK_SUPPLIER_ID,
                Products = new List<AddProductDto>
                {
                    new AddProductDto
                    {
                        Name = "test product",
                        MeasurementUnit = MeasurementUnitEnum.Kilogram,
                        Quantity = 0,
                        PlusQuantity = 5,
                        Price = 10
                    },
                    new AddProductDto
                    {
                        Name = "test product 2",
                        MeasurementUnit = MeasurementUnitEnum.SingularPiece,
                        Quantity = 0,
                        PlusQuantity = 5,
                        Price = 10
                    },
                }
            };

            var result = await ordersService.AddOrder(request);

            Assert.IsTrue(result);
        }
    }
}
