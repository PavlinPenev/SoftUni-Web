using Store_Ge.Services.Models;

namespace Store_Ge.Services.Services.EmailService
{
    public interface IEmailService
    {
        Task SendConfirmationMail(string email);
    }
}
