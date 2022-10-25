using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Store_Ge.Services.Models;
using Store_Ge.Services.Services.AccountsService;
using Store_Ge.Services.Services.EmailService;
using System.Text;
using static Store_Ge.Web.Constants.Constants.Accounts;

namespace Store_Ge.Web.Controllers
{
    [Route(Routes.ACCOUNTS_CONTROLLER)]
    public class AccountsController : StoreGeBaseController
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
                user = await accountsService.AuthenticateUser(userLoginModel);
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

            await emailService.SendConfirmationMail(emailConfirmationToken, user);

            var encodedEmail = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(user.Email));

            return StatusCode(201, encodedEmail);
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

        [HttpPost]
        [AllowAnonymous]
        [Route(Routes.CONFIRM_EMAIL_ENDPOINT)]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDto confirmEmailDto)
        {
            var result = new IdentityResult();

            try
            {
                result = await accountsService.ConfirmEmail(confirmEmailDto);
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }

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

        [HttpPost]
        [AllowAnonymous]
        [Route(Routes.FORGOT_PASSWORD_ENDPOINT)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPassword)
        {
            var user = await accountsService.GetUserByEmail(forgotPassword.Email);

            if (user == null)
            {
                return NotFound();
            }

            var passwordResetToken = await accountsService.GenerateForgottenPasswordResetToken(forgotPassword);

            await emailService.SendPasswordResetMail(user, passwordResetToken);

            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(Routes.PASSWORD_RESET_ENDPOINT)]
        public async Task<IActionResult> PasswordReset([FromBody] PasswordResetDto passwordResetDto)
        {
            var result = new IdentityResult();

            try
            {
                result = await accountsService.PasswordReset(passwordResetDto);
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }

            if (!result.Succeeded)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
