using Store_Ge.Data.Models;

namespace Store_Ge.Services.Services.EmailService
{
    public interface IEmailService
    {
        /// <summary>
        ///  Send confirmation mail to the user's email address
        /// </summary>
        /// <param name="emailToken"> An email confirmation token generated in the register process </param>
        /// <param name="user"> The user to send confirmation email </param>
        Task SendConfirmationMail(string emailToken, ApplicationUser user);

        /// <summary>
        ///  Resend the confirmation mail to the user. New email token is being generated in the process
        /// </summary>
        /// <param name="email"> The user's email address </param>
        Task ResendConfirmationMail(string email);

        /// <summary>
        /// Send password reset mail to the user's email address
        /// </summary>
        /// <param name="user"> The user to send password reset email </param>
        /// <param name="passwordResetToken"> The password reset token that has been generated </param>
        Task SendPasswordResetMail(ApplicationUser user, string passwordResetToken);
    }
}
