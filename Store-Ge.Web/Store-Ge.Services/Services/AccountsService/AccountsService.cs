using Microsoft.AspNetCore.Identity;
using Store_Ge.Data.Models;

namespace Store_Ge.Services.Services.AccountsService
{
    public class AccountsService : IAccountsService
    {
        public AccountsService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {

        }
    }
}
