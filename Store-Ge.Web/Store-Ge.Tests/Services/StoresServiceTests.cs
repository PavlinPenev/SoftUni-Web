using NUnit.Framework;
using Store_Ge.Data.Enums;
using Store_Ge.Data.Models;
using Store_Ge.Services.Models.StoreModels;
using Store_Ge.Services.Services.AuditTrailService;
using Store_Ge.Services.Services.StoresService;
using System.Linq;
using System.Threading.Tasks;

namespace Store_Ge.Tests.Services
{
    public class StoresServiceTests : TestsSetUp
    {
        private IAuditTrailService auditTrailService;
        private IStoresService storesService;

        [SetUp]
        public async Task Setup()
        {
            await InitializeDbContext();

            var storeRepository = GetStoreRepository();
            var auditEventRepository = GetAuditTrailRepository();
            var mapper = GetAutoMapper();
            var userStoreRepository = GetUserStoreRepository();
            var userManager = GetUserManager();
            var dataProtectionProvider = GetProtectionProvider();
            var storeGeAppSettingsOptions = GetStoreGeAppSettings();

            auditTrailService = new AuditTrailService(auditEventRepository, mapper);

            storesService = new StoresService(
                storeRepository,
                auditTrailService,
                userStoreRepository,
                userManager,
                dataProtectionProvider,
                storeGeAppSettingsOptions,
                mapper);
        }

        [Test]
        public async Task GetStoresReturnsTheUserStores()
        {
            var userId = MOCK_USER_ID;

            var result = await storesService.GetStores(userId);

            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task AddStoreAddsStoreToDb()
        {
            var request = new AddStoreDto
            {
                UserId = MOCK_USER_ID,
                Name = "Test Store Name",
                Type = StoreTypeEnum.Supermarket
            };

            var result = await storesService.AddStore(request);

            var stores = context.Set<Store>();
            var usersStores = context.Set<UserStore>();

            Assert.IsTrue(result);
            Assert.AreEqual(3, stores.Count());
            Assert.AreEqual(3, usersStores.Count());
        }

        [Test]
        public async Task GetStoreReturnsTheCorrectStore()
        {
            var storeId = MOCK_STORE_ID;

            var store = await storesService.GetStore(storeId);

            Assert.NotNull(store);
            Assert.AreEqual("Paf Supermarket 2", store.Name);
            Assert.AreEqual(StoreTypeEnum.Supermarket, store.Type);
        }

        [Test]
        public async Task GetStoreByNameAndUserIdRetursTheCorrectStore()
        {
            var userId = "6";
            var name = "Paf Supermarket 2";

            var store = await storesService.GetStoreByNameAndUserId(userId, name);

            Assert.NotNull(store);
            Assert.AreEqual("Paf Supermarket 2", store.Name);
            Assert.AreEqual(StoreTypeEnum.Supermarket, store.Type);
        }

        [Test]
        public async Task GetReportFileReturnsTheExcelFileAsByteArray()
        {
            var storeId = MOCK_STORE_ID;

            var file = await storesService.GetReportFile(storeId);

            Assert.NotNull(file);
            Assert.AreEqual(typeof(byte[]), file.GetType());
        }
    }
}
