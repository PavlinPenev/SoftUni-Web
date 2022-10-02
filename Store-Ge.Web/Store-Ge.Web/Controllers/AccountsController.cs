using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store_Ge.Services.Services.AccountsService;
using static Store_Ge.Web.Constants.Constants.Accounts;

namespace Store_Ge.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route(ACCOUNTS_CONTROLLER_ROUTE)]
    public class AccountsController : Controller
    {
        private readonly IAccountsService accountsService;

        public AccountsController(IAccountsService accountsService)
        {
            this.accountsService = accountsService;
        }
    }
}
