using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Store_Ge.Services.Models;
using Store_Ge.Services.Services.AccountsService;
using Store_Ge.Services.Services.EmailService;
using static Store_Ge.Web.Constants.Constants.Accounts;

namespace Store_Ge.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route(Routes.ACCOUNTS_CONTROLLER_ROUTE)]
    public class AccountsController : Controller
    {
        private readonly IAccountsService accountsService;
        private readonly IEmailService emailService;

        public AccountsController(
            IAccountsService accountsService, 
            IEmailService emailService)
        {
            this.accountsService = accountsService;
            this.emailService = emailService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(Routes.ACCOUNTS_LOGIN_ENDPOINT_ROUTE)]
        public async Task<IActionResult> Login([FromBody] ApplicationUserLoginDto userLoginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await this.accountsService.AuthenticateUser(userLoginModel);

            if (user == null)
            {
                return Unauthorized(user);
            }

            return Ok(user);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(Routes.ACCOUNTS_REGISTER_ENDPOINT_ROUTE)]
        public async Task<IActionResult> Register([FromBody] ApplicationUserRegisterDto userRegisterModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (userRegisterModel.Password != userRegisterModel.ConfirmPassword)
            {
                ModelState.AddModelError(Shared.CONFIRM_PASSWORD_STRING_LITERAL, Shared.CONFIRM_PASSWORD_SHOULD_MATCH_THE_PASSWORD);

                return BadRequest(ModelState);
            }

            var result = await accountsService.RegisterUser(userRegisterModel);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.FirstOrDefault());
            }

            await emailService.SendConfirmationMail(userRegisterModel.Email);

            return StatusCode(201);
        }
    }
}
