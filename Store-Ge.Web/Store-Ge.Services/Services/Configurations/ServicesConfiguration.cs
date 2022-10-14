using Microsoft.Extensions.DependencyInjection;
using Store_Ge.Services.Services.EmailService;
using Store_Ge.Services.Services.AccountsService;
using Store_Ge.Services.Services.EmailService.EmailSender;

namespace Store_Ge.Services.Configurations
{
    public static class ServicesConfiguration
    {
        public static void AddServiceLayer(this IServiceCollection services)
        {
            services.AddScoped<IAccountsService,AccountsService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddTransient<IEmailSender, SendGridEmailSender>();
        }
    }
}
