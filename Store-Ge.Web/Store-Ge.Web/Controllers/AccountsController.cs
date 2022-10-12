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
    [Route(Routes.ACCOUNTS_CONTROLLER)]
    public class AccountsController : ControllerBase
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
        [Route(Routes.ACCOUNTS_LOGIN_ENDPOINT)]
        public async Task<IActionResult> Login([FromBody] ApplicationUserLoginDto userLoginModel)
        {
            var user = new ApplicationUserLoginResponseDto();

            try
            {
                user = await this.accountsService.AuthenticateUser(userLoginModel);
            }
            catch (NullReferenceException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                if (e is InvalidOperationException || e is MemberAccessException)
                {
                    return Unauthorized(e.Message);
                }
            }

            return Ok(user);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(Routes.ACCOUNTS_REGISTER_ENDPOINT)]
        public async Task<IActionResult> Register([FromBody] ApplicationUserRegisterDto userRegisterModel)
        {
            var result = await accountsService.RegisterUser(userRegisterModel);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var user = await accountsService.GetUserByEmail(userRegisterModel.Email);

            var emailConfirmationToken = await accountsService.GenerateConfirmationEmailToken(user);

            await emailService.SendConfirmationMail(emailConfirmationToken, user.Id);

            return StatusCode(201);
        }



        [HttpPost]
        [AllowAnonymous]
        [Route(Routes.REFRESH_ACCESS_TOKEN_ENDPOINT)]
        public async Task<IActionResult> RefreshAccessToken([FromQuery] string refreshToken, [FromQuery] int userId)
        {
            var result = new ApplicationUserTokensDto();

            try
            {
                result = await accountsService.RefreshAccessTokenAsync(refreshToken, userId);
            }
            catch (NullReferenceException e)
            {
                return NotFound(e.Message);
            }
            catch (MemberAccessException e)
            {
                return Unauthorized(e.Message);
            }

            return Ok(result);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route(Routes.CONFIRM_EMAIL_ENDPOINT)]
        public async Task<IActionResult> ConfirmEmail([FromQuery] int userId, [FromQuery] string emailToken)
        {
            var result = await accountsService.ConfirmEmail(userId, emailToken);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route(Routes.RESEND_EMAIL_ENDPOINT)]
        public async Task<IActionResult> ResendConfirmationEmail([FromQuery] string email)
        {
            await emailService.ResendConfirmationMail(email);

            return Ok();
        }
    }
}
