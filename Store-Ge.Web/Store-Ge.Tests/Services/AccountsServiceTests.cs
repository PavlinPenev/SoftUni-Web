using NUnit.Framework;
using Store_Ge.Services.Models;
using Store_Ge.Services.Services.AccountsService;
using System.Threading.Tasks;

namespace Store_Ge.Tests.Services
{
    public class AccountsServiceTests : TestsSetUp
    {
        private AccountsService accountsService;

        [SetUp]
        public async Task Setup()
        {
            await InitializeDbContext();

            var usersRepository = GetUserRepository();
            var userManager = GetUserManager();
            var roleManager = GetRoleManager();
            var dataProtectionProvider = GetProtectionProvider();
            var jwtSettingsOptions = GetJwtSettingsOptions();
            var storeGeAppSettingsOptions = GetStoreGeAppSettings();
            var mapper = GetAutoMapper();

            accountsService = new AccountsService(
                usersRepository,
                userManager, 
                roleManager, 
                dataProtectionProvider, 
                jwtSettingsOptions, 
                storeGeAppSettingsOptions,
                mapper);
        }

        [Test]
        public async Task AuthenticateUserReturnsResponseDto()
        {
            var appUserLoginDto = new ApplicationUserLoginDto
            {
                Email = "asfg@asg.bas",
                Password = "Aa!123456"
            };

            var loginResponseDto = await accountsService.AuthenticateUser(appUserLoginDto);

            Assert.AreEqual(appUserLoginDto.Email, loginResponseDto.Email);
            Assert.AreEqual(users[0].UserName, loginResponseDto.UserName);
        }

        [Test]
        public async Task RegisterUserReturnsCorrectResult()
        {
            var appUserRegisterDto = new ApplicationUserRegisterDto
            {
                UserName = "Paf",
                Email = "asfg@asg.bas",
                Password = "Aa!123456",
                ConfirmPassword = "Aa!123456"
            };

            var registerResponseDto = await accountsService.RegisterUser(appUserRegisterDto);

            Assert.AreEqual(true, registerResponseDto.Succeeded);
        }

        [Test]
        public async Task GenerateConfirmationEmailTokenReturnsToken()
        {
            var user = users[2];

            var token = await accountsService.GenerateConfirmationEmailToken(user);

            Assert.IsNotNull(token);
        }

        [Test]
        public async Task GetUserByEmailReturnsUser()
        {
            var email = MOCK_EMAIL_ADDRESS;

            var user = await accountsService.GetUserByEmail(email);

            Assert.AreEqual(email, user.Email);
        }

        [Test]
        public async Task RefreshAccessTokenReturnsTokens()
        {
            var token = MOCK_REFRESH_TOKEN;
            var userId = MOCK_USER_ID;

            var tokens = await accountsService.RefreshAccessTokenAsync(token, userId);

            Assert.IsNotNull(tokens);
        }

        [Test]
        public async Task GetUser()
        {
            var userId = MOCK_USER_ID;

            var user = await accountsService.GetUser(userId);

            Assert.AreEqual(users[0].UserName, user.UserName);
            Assert.AreEqual(users[0].Email, user.Email);
        }

        [Test]
        public async Task RegisterCashier()
        {
            var appCashierRegisterDto = new AddCashierRequestDto
            {
                StoreId = MOCK_STORE_ID,
                UserName = "CashierPaf",
                Email = "dsfg@asg.bas",
                Password = "Aa!123456",
                ConfirmPassword = "Aa!123456"
            };

            var usersBeforeRegister = users.Count;

            var result = await accountsService.RegisterCashier(appCashierRegisterDto);

            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(usersBeforeRegister + 1, 4);
        }

        [Test]
        public async Task UpdateUserUpdatesUser()
        {
            var userId = MOCK_USER_ID;
            var email = MOCK_EMAIL_ADDRESS_FOR_UPDATE;
            var username = MOCK_USERNAME;

            var result = await accountsService.UpdateUser(userId, email, username);

            Assert.IsTrue(result);
            Assert.AreEqual("mockPaf", users[0].UserName);
            Assert.AreEqual("mockEmailUpdated@asd.bg", users[0].Email);
        }
    }
}
