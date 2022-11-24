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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

            return StatusCode(201, new { encodedEmail });
        }



        [HttpGet]
        [AllowAnonymous]
        [Route(Routes.REFRESH_ACCESS_TOKEN_ENDPOINT)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RefreshAccessToken([FromQuery] string refreshToken, [FromQuery] string userId)
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
            catch (MemberAccessException)
            {
                return Forbid();
            }

            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(Routes.CONFIRM_EMAIL_ENDPOINT)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

            return Ok(result.Succeeded);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route(Routes.RESEND_EMAIL_ENDPOINT)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ResendConfirmationEmail([FromQuery] string email)
        {
            await emailService.ResendConfirmationMail(email);

            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(Routes.FORGOT_PASSWORD_ENDPOINT)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

            return Ok(result.Succeeded);
        }

        [HttpPost]
        [Route(Routes.ADD_CASHIER_ENDPOINT)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCashier(AddCashierRequestDto request)
        {
            var result = await accountsService.RegisterCashier(request);
            if (!result.Succeeded)
            {
                return BadRequest();
            }

            var user = await accountsService.GetUserByEmail(request.Email);

            var emailConfirmationToken = await accountsService.GenerateConfirmationEmailToken(user);

            await emailService.SendConfirmationMail(emailConfirmationToken, user);

            return Ok();
        }

        [HttpGet]
        [Route(Routes.GET_USER_ENDPOINT)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUser([FromQuery] string userId)
        {
            var user = await accountsService.GetUser(userId);

            if (user == null)
            {
                return BadRequest(userId);
            }

            return Ok(user);
        }

        [HttpPut]
        [Route(Routes.UPDATE_USER_INFO_ENDPOINT)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUser([FromQuery] string userId, [FromQuery] string email, [FromQuery] string userName)
        {
            try
            {
                var result = await accountsService.UpdateUser(userId, email, userName);

                if (!result)
                {
                    return BadRequest();
                }

                return Ok();
            }
            catch (NullReferenceException)
            {

                return NotFound();
            }
        }
    }
}
