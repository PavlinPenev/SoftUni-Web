using Microsoft.AspNetCore.Identity;
using Store_Ge.Data.Models;
using Store_Ge.Data.Repositories;
using Store_Ge.Services.Services.AccountsService;

namespace Store_Ge.Services.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IAccountsService accountsService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRepository<ApplicationUser> usersRepository;

        public EmailService(
            IAccountsService accountsService,
            UserManager<ApplicationUser> userManager,
            IRepository<ApplicationUser> usersRepository)
        {
            this.accountsService = accountsService;
            this.userManager = userManager;
            this.usersRepository = usersRepository;
        }

        public async Task SendConfirmationMail(string emailToken, int userId)
        {
            throw new NotImplementedException(); // TODO:
        }

        public async Task ResendConfirmationMail(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            usersRepository.Update(user);
            await usersRepository.SaveChangesAsync();

            await SendConfirmationMail(token, user.Id);
        }
    }
}
