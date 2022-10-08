namespace Store_Ge.Services.Services.EmailService
{
    public interface IEmailService
    {
        /// <summary>
        ///  Send confirmation mail to the user's email address
        /// </summary>
        /// <param name="emailToken"> An email confirmation token generated in the register process </param>
        /// <param name="userId"> The user's Id </param>
        Task SendConfirmationMail(string emailToken, int userId);

        /// <summary>
        ///  Resend the confirmation mail to the user. New email token is being generated in the process
        /// </summary>
        /// <param name="email"> The user's email address </param>
        Task ResendConfirmationMail(string email);
    }
}
