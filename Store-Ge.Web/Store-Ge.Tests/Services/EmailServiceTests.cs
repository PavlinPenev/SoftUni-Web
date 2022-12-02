using Microsoft.AspNetCore.WebUtilities;
using NUnit.Framework;
using Store_Ge.Data.Models;
using Store_Ge.Services.Services.EmailService;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store_Ge.Tests.Services
{
    public class EmailServiceTests : TestsSetUp
    {
        private IEmailService emailService;

        [SetUp]
        public async Task Setup()
        {
            await InitializeDbContext();

            var userManager = GetUserManager();
            var emailSender = GetEmailSender();
            var storeGeAppSettings = GetStoreGeAppSettings();
            var dataProtectionProvider = GetProtectionProvider();

            emailService = new EmailService(
                userManager,
                emailSender,
                storeGeAppSettings,
                dataProtectionProvider);
        }

        [Test]
        public void SendConfirmationEmailDoesntThrowError()
        {
            var emailToken = MOCK_EMAIL_CONFIRMATION_TOKEN;

            var user = context.Set<ApplicationUser>().FirstOrDefault();

            Assert.DoesNotThrowAsync(async () => await emailService.SendConfirmationMail(emailToken, user));
        }

        [Test]
        public void ResendConfirmationEmailDoesntThrowError()
        {
            var email = MOCK_EMAIL_ADDRESS;
            var encodedEmail = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(email));

            Assert.DoesNotThrowAsync(async () => await emailService.ResendConfirmationMail(encodedEmail));
        }

        [Test]
        public void SendPasswordResetMailDoesntThrowError()
        {
            var user = context.Set<ApplicationUser>().FirstOrDefault();
            var token = MOCK_RESET_PASSWORD_TOKEN;

            Assert.DoesNotThrowAsync(async () => await emailService.SendPasswordResetMail(user, token));
        }
    }
}
