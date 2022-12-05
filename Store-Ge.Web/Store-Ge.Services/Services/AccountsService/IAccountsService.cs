using Microsoft.AspNetCore.Identity;
using Store_Ge.Data.Models;
using Store_Ge.Services.Models;

namespace Store_Ge.Services.Services.AccountsService
{
    public interface IAccountsService
    {
        /// <summary>
        ///  Authenticates the user. Login method is with password.
        /// </summary>
        /// <param name="user"> Contains the user's password and email address </param>
        /// <returns> A model of the user metadata(Id, UserName, Email, Password, AccessToken, RefreshToken) </returns>
        Task<ApplicationUserLoginResponseDto> AuthenticateUser(ApplicationUserLoginDto user);

        /// <summary>
        ///  Registers the user.
        /// </summary>
        /// <param name="user"> Contains the user's required register parameters(UserName, Email, Password, ConfirmPassword) </param>
        /// <returns> Metadata about the register process. It determines if the registration was successful </returns>
        Task<IdentityResult> RegisterUser(ApplicationUserRegisterDto user);

        /// <summary>
        ///  Generates Confirmation Email token
        /// </summary>
        /// <param name="user"> A user record taken from the database on which an email token generation is needed </param>
        /// <returns> The token itself </returns>
        Task<string> GenerateConfirmationEmailToken(ApplicationUser user);

        /// <summary>
        ///  Refreshes the JWT access token with the use of the user's refresh token
        /// </summary>
        /// <param name="refreshToken"> The user's refresh token </param>
        /// <param name="userId"> The user's Id </param>
        /// <returns> A model containing both Refresh and Access tokens </returns>
        Task<ApplicationUserTokensDto> RefreshAccessTokenAsync(string refreshToken, string userId);


        /// <summary>
        ///  Gets a user from the database using his email address 
        /// </summary>
        /// <param name="email"> The user's email address </param>
        /// <returns> The user </returns>
        Task<ApplicationUser> GetUserByEmail(string email);

        /// <summary>
        ///  Gets a user using his user Id
        /// </summary>
        /// <param name="userId"> The user's Id(protected) </param>
        /// <returns> The user </returns>
        Task<ApplicationUserDto> GetUser(string userId);

        /// <summary>
        ///  Confirms a user's email address
        /// </summary>
        /// <param name="confirmEmailDto"> Model containing the user's Id and EmailToken </param>
        /// <returns> Metadata about the confirmation process. It determines if the confirmation was successful </returns>
        Task<IdentityResult> ConfirmEmail(ConfirmEmailDto confirmEmailDto);

        /// <summary>
        ///  Generates a reset password token for when user's has forgotten his password. Also sends email for the reset itself
        /// </summary>
        /// <param name="forgotPassword"> Model which contains the user's email address </param>
        /// <returns> The reset token </returns>
        Task<string> GenerateForgottenPasswordResetToken(ForgotPasswordDto forgotPassword);

        /// <summary>
        ///  Resets the password, setting a new one to the user
        /// </summary>
        /// <param name="passwordResetDto"> A model containing the user info needed for password reset(Password, ConfirmPassword, Email, ResetToken) </param>
        /// <returns> Metadata info about the reset process. It determines if the reset was successful </returns>
        Task<IdentityResult> PasswordReset(PasswordResetDto passwordResetDto);

        /// <summary>
        ///  Registers a cashier for the given store
        /// </summary>
        /// <param name="request"> Request with needed parameters for the registration </param>
        /// <returns> Metadata info about the registration process. It determines if the registration was successful </returns>
        Task<IdentityResult> RegisterCashier(AddCashierRequestDto request);

        /// <summary>
        ///  Updates the user's info
        /// </summary>
        /// <param name="userId"> The user's Id </param>
        /// <param name="email"> The user's new Email Address </param>
        /// <param name="userName"> The user's new UserName </param>
        /// <returns> If the update was successful </returns>
        Task<bool> UpdateUser(string userId, string email, string userName);
    }
}
