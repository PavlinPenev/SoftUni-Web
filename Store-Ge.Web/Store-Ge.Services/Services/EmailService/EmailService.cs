﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Store_Ge.Data.Models;
using Store_Ge.Data.Repositories;
using Store_Ge.Services.Configurations;
using Store_Ge.Services.Services.EmailService.EmailSender;
using Store_Ge.Services.Services.AccountsService;
using System.Reflection;
using System.Text;
using static Store_Ge.Services.Constants.Constants.EmailService;
using Microsoft.AspNetCore.DataProtection;

namespace Store_Ge.Services.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailSender emailSender;
        private readonly StoreGeAppSettings storeGeAppSettings;
        private readonly IDataProtector dataProtector;

        public EmailService(
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            IOptions<StoreGeAppSettings> storeGeAppSettings,
            IDataProtectionProvider dataProtectionProvider)
        {
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.storeGeAppSettings = storeGeAppSettings.Value;
            this.dataProtector = dataProtectionProvider.CreateProtector(EMAIL_SERVICE_PROTECTION);
        }

        public async Task SendConfirmationMail(string emailToken, ApplicationUser user)
        {
            var token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailToken));

            var encodedUserId = dataProtector.Protect(Encoding.UTF8.GetBytes(user.Id.ToString()));

            var htmlString = GenerateHtmlContent(token, encodedUserId.ToString(), "ConfirmEmailTemplate.html");

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

            await SendConfirmationMail(token, user);
        }

        public async Task SendPasswordResetMail(ApplicationUser user, string passwordResetToken)
        {
            var token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(passwordResetToken));

            var encodedUserId = dataProtector.Protect(Encoding.UTF8.GetBytes(user.Email));

            var htmlString = GenerateHtmlContent(token, encodedUserId.ToString(), "PasswordResetEmailTemplate.html");

            await emailSender.SendEmailAsync(
                STORE_GE_EMAIL_ADDRESS,
                STORE_GE_STRING_LITERAL,
                user.Email,
                RESET_PASSWORD_EMAIL_STRING_LITERAL,
                htmlString);
        }

        private string GenerateHtmlContent(string emailToken, string userId, string fileName)
        {
            var asm = Assembly.GetExecutingAssembly();
            var path = Path.GetDirectoryName(asm.Location);
            var htmlString = File.ReadAllText(path + $"/Services/EmailService/EmailSender/MailTemplates/{fileName}");

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
