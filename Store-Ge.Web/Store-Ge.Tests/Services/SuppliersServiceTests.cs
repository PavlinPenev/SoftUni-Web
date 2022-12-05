using NUnit.Framework;
using Store_Ge.Data.Models;
using Store_Ge.Services.Models.SupplierModels;
using Store_Ge.Services.Services.AccountsService;
using Store_Ge.Services.Services.AuditTrailService;
using Store_Ge.Services.Services.SuppliersService;
using System.Linq;
using System.Threading.Tasks;

namespace Store_Ge.Tests.Services
{
    public class SuppliersServiceTests : TestsSetUp
    {
        private IAccountsService accountsService;
        private IAuditTrailService auditTrailService;
        private ISuppliersService suppliersService;

        [SetUp]
        public async Task Setup()
        {
            await InitializeDbContext();
            var userRepository = GetUserRepository();
            var userManager = GetUserManager();
            var roleManager = GetRoleManager();
            var dataProtectionProvider = GetProtectionProvider();
            var jwtSettingsOptions = GetJwtSettingsOptions();
            var storeGeAppSettingsOptions = GetStoreGeAppSettings();
            var mapper = GetAutoMapper();
            var auditTrailRepository = GetAuditTrailRepository();
            var storeSupplierRepository = GetStoreSupplierRepository();
            var supplierRepository = GetSupplierRepository();
            var userStoreRepository = GetUserStoreRepository();

            accountsService = new AccountsService(
                userRepository,
                userManager,
                roleManager,
                dataProtectionProvider,
                jwtSettingsOptions,
                storeGeAppSettingsOptions,
                mapper);
            auditTrailService = new AuditTrailService(
                auditTrailRepository,
                mapper);
            suppliersService = new SuppliersService(
                accountsService,
                auditTrailService,
                dataProtectionProvider,
                storeGeAppSettingsOptions,
                storeSupplierRepository,
                userStoreRepository,
                supplierRepository,
                mapper);
        }

        [Test]
        public async Task GetUserSuppliersReturnsAllTheUserSuppliers()
        {
            var userId = MOCK_USER_ID;

            var suppliers = await suppliersService.GetUserSuppliers(userId);

            Assert.NotNull(suppliers);
            Assert.AreEqual(1, suppliers.Count);
        }

        [Test]
        public async Task GetUserSuppliersPagedReturnsTheUserSuppliersPaged()
        {
            var request = new UserSuppliersRequestDto
            {
                UserId = MOCK_USER_ID,
                SearchTerm = "",
                OrderBy = "name",
                IsDescending = false,
                DateAddedFrom = null,
                DateAddedTo = null,
                Skip = 0,
                Take = 5
            };

            var suppliers = await suppliersService.GetUserSuppliersPaged(request);

            Assert.NotNull(suppliers);
            Assert.AreEqual(1, suppliers.Items.Count);
            Assert.AreEqual(1, suppliers.TotalItemsCount);
        }

        [Test]
        public async Task AddSupplierAddsSupplierCorrectly()
        {
            var request = new AddSupplierRequestDto
            {
                Name = "Test Supplier Name",
                UserId = MOCK_USER_ID,
            };

            var result = await suppliersService.AddSupplier(request);

            var supplier = context.Set<Supplier>().FirstOrDefault(x => x.Name == "Test Supplier Name");
            var storeSuppliers = context.Set<StoreSupplier>();

            Assert.IsTrue(result);
            Assert.NotNull(supplier);
            Assert.AreEqual(4, storeSuppliers.Count());
        }
    }
}
