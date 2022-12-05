using Microsoft.Extensions.DependencyInjection;
using Store_Ge.Services.Services.EmailService;
using Store_Ge.Services.Services.AccountsService;
using Store_Ge.Services.Services.EmailService.EmailSender;
using Store_Ge.Services.Services.StoresService;
using Store_Ge.Services.Services.AuditTrailService;
using Store_Ge.Services.Services.ProductsService;
using Store_Ge.Services.Services.OrdersService;
using Store_Ge.Services.Services.SuppliersService;

namespace Store_Ge.Services.Configurations
{
    public static class ServicesConfiguration
    {
        public static void AddServiceLayer(this IServiceCollection services)
        {
            services.AddScoped<IAccountsService,AccountsService>();
            services.AddScoped<IStoresService, StoresService>();
            services.AddScoped<IOrdersService, OrdersService>();
            services.AddScoped<IProductsService, ProductsService>();
            services.AddScoped<ISuppliersService, SuppliersService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddTransient<IEmailSender, SendGridEmailSender>();
            services.AddScoped<IAuditTrailService, AuditTrailService>();
        }
    }
}
