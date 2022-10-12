using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Store_Ge.Data.Models;
using Store_Ge.Data.Repositories;
using Store_Ge.Services.Configurations;
using Store_Ge.Services.EmailSender;
using Store_Ge.Services.Services.AccountsService;
using System.Reflection;
using System.Text;
using static Store_Ge.Services.Constants.Constants.EmailService;

namespace Store_Ge.Services.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IAccountsService accountsService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRepository<ApplicationUser> usersRepository;
        private readonly IEmailSender emailSender;
        private readonly StoreGeAppSettings storeGeAppSettings;

        public EmailService(
            IAccountsService accountsService,
            UserManager<ApplicationUser> userManager,
            IRepository<ApplicationUser> usersRepository,
            IEmailSender emailSender,
            IOptions<StoreGeAppSettings> storeGeAppSettings)
        {
            this.accountsService = accountsService;
            this.userManager = userManager;
            this.usersRepository = usersRepository;
            this.emailSender = emailSender;
            this.storeGeAppSettings = storeGeAppSettings.Value;
        }

        public async Task SendConfirmationMail(string emailToken, int userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            var token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailToken));

            var htmlString = GenerateHtmlContent(token, userId);

            await emailSender.SendEmailAsync(
                STORE_GE_EMAIL_ADDRESS, 
                STORE_GE_STRING_LITERAL,
                user.Email,
                CONFIRM_EMAIL_STRING_LITERAL, 
                htmlString);
        }

        public async Task ResendConfirmationMail(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            usersRepository.Update(user);
            await usersRepository.SaveChangesAsync();

            await SendConfirmationMail(token, user.Id);
        }

        private string GenerateHtmlContent(string emailToken, int userId)
        {
            var asm = Assembly.GetExecutingAssembly();
            var path = Path.GetDirectoryName(asm.Location);
            var htmlString = File.ReadAllText(path + "/EmailSender/MailTemplates/ConfirmEmailTemplate.html");

            var htmlContent = string.Format(
                htmlString,
                storeGeAppSettings.StoreGeAppBaseUrl,
                storeGeAppSettings.StoreGeAppConfirmEmailUrl,
                emailToken,
                userId);

            return htmlContent;
        }

    }
}
