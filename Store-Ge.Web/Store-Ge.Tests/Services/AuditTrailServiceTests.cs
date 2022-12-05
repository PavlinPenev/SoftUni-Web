using NUnit.Framework;
using Store_Ge.Data.Enums;
using Store_Ge.Data.Models;
using Store_Ge.Services.Models.ProductModels;
using Store_Ge.Services.Models.StoreModels;
using Store_Ge.Services.Services.AuditTrailService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store_Ge.Tests.Services
{
    public class AuditTrailServiceTests : TestsSetUp
    {
        private AuditTrailService auditTrailService;

        [SetUp]
        public async Task Setup()
        {
            await InitializeDbContext();

            var auditTrailRepository = GetAuditTrailRepository();
            var mapper = GetAutoMapper();

            auditTrailService = new AuditTrailService(
                auditTrailRepository,
                mapper);
        }

        [Test]
        public async Task GetAllReturnsTheCorrespondingStoreEvents()
        {
            var storeId = 6;

            var events = await auditTrailService.GetAll(storeId);

            Assert.AreEqual(1, events.Count);
            Assert.AreEqual("MockAction", events[0].Action);
        }

        [Test]
        public async Task AddStoreAddsEventForAddedStore()
        {
            var request = new AddStoreDto
            {
                UserId = "1",
                Name = "Test Store Name",
                Type = StoreTypeEnum.Supermarket
            };

            var storeId = 6;

            await auditTrailService.AddStore(request, storeId);

            var events = context.Set<AuditEvent>().ToList();
            var countOfEvents = events.Count;
            var lastEvent = events.Last();

            Assert.AreEqual(3, countOfEvents);
            Assert.AreEqual("AddStore", lastEvent.Action);
            Assert.AreEqual("Added Store with name Test Store Name and type Supermarket.", lastEvent.Description);
        }

        [Test]
        public async Task AddProductAddsEventForAddedProduct()
        {
            var request = new AddProductDto
            {
                Name = "Test product",
                MeasurementUnit = MeasurementUnitEnum.SingularPiece,
                Quantity = 30,
                Price = 3.6M
            };

            var storeId = 6;

            await auditTrailService.AddProduct(request, storeId);

            var events = context.Set<AuditEvent>().ToList();
            var countOfEvents = events.Count;
            var lastEvent = events.Last();

            Assert.AreEqual(3, countOfEvents);
            Assert.AreEqual("AddProduct", lastEvent.Action);
            Assert.AreEqual(
                "Added a product with name Test product with quantity 30, price 3.6 and a measurementUnit SingularPiece.", 
                lastEvent.Description);
        }

        [Test]
        public async Task SellProductAddsEventForSoldProduct()
        {
            var request = new AddProductDto
            {
                Name = "Test product",
                MeasurementUnit = MeasurementUnitEnum.SingularPiece,
                Quantity = 30,
                PlusQuantity = 5,
                Price = 3.6M
            };

            var storeId = 6;

            await auditTrailService.SellProduct(request, storeId);

            var events = context.Set<AuditEvent>().ToList();
            var countOfEvents = events.Count;
            var lastEvent = events.Last();

            Assert.AreEqual(3, countOfEvents);
            Assert.AreEqual("SellProduct", lastEvent.Action);
            Assert.AreEqual(
                "Sold a product with name Test product, quantity 5.",
                lastEvent.Description);
        }

        [Test]
        public async Task AddProductQuantityAddsEventForAddProductQuantity()
        {
            var request = new AddProductDto
            {
                Name = "Test product",
                MeasurementUnit = MeasurementUnitEnum.SingularPiece,
                Quantity = 30,
                PlusQuantity = 5,
                Price = 3.6M
            };

            var storeId = 6;

            await auditTrailService.AddProductQuantity(request, storeId);

            var events = context.Set<AuditEvent>().ToList();
            var countOfEvents = events.Count;
            var lastEvent = events.Last();

            Assert.AreEqual(3, countOfEvents);
            Assert.AreEqual("AddProductQuantity", lastEvent.Action);
            Assert.AreEqual(
                "Added 5 quantity to product with name Test product, price 3.6 and a measurementUnit SingularPiece.",
                lastEvent.Description);
        }

        [Test]
        public async Task AddOrderAddsEventForAddingOrder()
        {
            var order = new Order
            {
                OrderNumber = 1
            };

            var storeId = 6;

            await auditTrailService.AddOrder(order, storeId);

            var events = context.Set<AuditEvent>().ToList();
            var countOfEvents = events.Count;
            var lastEvent = events.Last();

            Assert.AreEqual(3, countOfEvents);
            Assert.AreEqual("AddOrder", lastEvent.Action);
            Assert.AreEqual(
                "Added order with number 1.",
                lastEvent.Description);
        }

        [Test]
        public async Task AddSupplierAddsEventForAddingSupplier()
        {
            var supplier = new Supplier
            {
                Name = "Test supplier"
            };

            var storeIds = new List<int> { 6, 2 };

            await auditTrailService.AddSupplier(supplier, storeIds);

            var events = context.Set<AuditEvent>().ToList();
            var countOfEvents = events.Count;
            var lastEvent = events.Last();

            Assert.AreEqual(4, countOfEvents);
            Assert.AreEqual("AddSupplier", lastEvent.Action);
            Assert.AreEqual(
                "Added supplier with name Test supplier.",
                lastEvent.Description);
        }
    }
}
