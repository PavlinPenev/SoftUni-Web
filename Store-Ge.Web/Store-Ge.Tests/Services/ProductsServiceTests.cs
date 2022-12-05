using NUnit.Framework;
using Store_Ge.Data.Enums;
using Store_Ge.Data.Models;
using Store_Ge.Services.Models.ProductModels;
using Store_Ge.Services.Services.AuditTrailService;
using Store_Ge.Services.Services.ProductsService;
using Store_Ge.Services.Services.StoresService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store_Ge.Tests.Services
{
    public class ProductsServiceTests : TestsSetUp
    {
        private IAuditTrailService auditTrailService;
        private IStoresService storesService;
        private IProductsService productsService;

        [SetUp]
        public async Task Setup()
        {
            await InitializeDbContext();

            var dataProtectionProvider = GetProtectionProvider();
            var productRepository = GetProductRepository();
            var auditTrailRepository = GetAuditTrailRepository();
            var mapper = GetAutoMapper();
            var storeRepository = GetStoreRepository();
            var userStoreRepository = GetUserStoreRepository();
            var userManager = GetUserManager();
            var storeGeAppSettings = GetStoreGeAppSettings();

            auditTrailService = new AuditTrailService(auditTrailRepository, mapper);
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
        }

        [Test]
        public async Task GetStoreProductsReturnsCorrectResult()
        {
            var request = new StoreProductsRequestDto
            {
                StoreId = MOCK_STORE_ID,
                SearchTerm = "",
                OrderBy = "name",
                IsDescending = false,
                DateAddedFrom = null,
                DateAddedTo = null,
                Skip = 0,
                Take = 5
            };

            var result = await productsService.GetStoreProducts(request);

            Assert.AreEqual(2, result.Items.Count);
            Assert.AreEqual(2, result.TotalItemsCount);
        }

        [Test]
        public async Task GetStoreAddProductsReturnsCorrectResult()
        {
            var storeId = MOCK_STORE_ID;

            var result = await productsService.GetStoreAddProducts(storeId);

            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task SellProductsSellsTheRequestProductsProperly()
        {
            var request = new SaleRequestDto
            {
                StoreId = MOCK_STORE_ID,
                Products = new List<AddProductDto>
                {
                    new AddProductDto
                    {
                        Id = MOCK_PRODUCT_ID,
                        MeasurementUnit = MeasurementUnitEnum.Kilogram,
                        Name = "Cheese",
                        Quantity = 5,
                        PlusQuantity = 2,
                        Price = 13
                    },
                }
            };

            var soldProductQuantityBeforeSale = 
                productsCopy.FirstOrDefault(x => x.Id == 6).Quantity;

            var result = await productsService.SellProducts(request);

            var soldProductQuantityAfterSale =
                productsCopy.FirstOrDefault(x => x.Id == 6).Quantity;

            Assert.AreEqual(5, soldProductQuantityBeforeSale);
            Assert.AreEqual(3, soldProductQuantityAfterSale);
        }

        [Test]
        public async Task UpsertProductsUpsertsCorrectly()
        {
            var products = new List<AddProductDto>
            {
                new AddProductDto
                {
                    Id = MOCK_PRODUCT_ID,
                    MeasurementUnit = MeasurementUnitEnum.Kilogram,
                    Name = "Cheese",
                    Quantity = 5,
                    PlusQuantity = 2,
                    Price = 13
                },
            };
            var storeId = 6;

            var soldProductQuantityBeforeSale =
                productsCopy.FirstOrDefault(x => x.Id == 6).Quantity;

            Assert.DoesNotThrowAsync(async () => await productsService.UpsertProducts(products, storeId));

            var soldProductQuantityAfterSale =
                productsCopy.FirstOrDefault(x => x.Id == 6).Quantity;

            Assert.AreEqual(5, soldProductQuantityBeforeSale);
            Assert.AreEqual(7, soldProductQuantityAfterSale);
        }
    }
}
