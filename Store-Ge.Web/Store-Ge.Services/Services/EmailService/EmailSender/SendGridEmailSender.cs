using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Store_Ge.Services.Services.EmailService.EmailSender
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly SendGridClient client;

        public SendGridEmailSender(IOptions<SendGridSettings> sendGridOptions)
        {
            client = new SendGridClient(sendGridOptions.Value.SendGridApiKey);
        }

        public async Task SendEmailAsync(
            string from, 
            string fromName,
            string to, 
            string subject,
            string htmlContent, 
            IEnumerable<EmailAttachment> attachments = null)
        {
            if (string.IsNullOrWhiteSpace(subject) && string.IsNullOrWhiteSpace(htmlContent))
            {
                throw new ArgumentException("Subject and message should be provided.");
            }

            var fromAddress = new EmailAddress(from, fromName);
            var toAddress = new EmailAddress(to);
            var message = MailHelper.CreateSingleEmail(fromAddress, toAddress, subject, null, htmlContent);
            if (attachments?.Any() == true)
            {
                foreach (var attachment in attachments)
                {
                    message.AddAttachment(attachment.FileName, Convert.ToBase64String(attachment.Content), attachment.MimeType);
                }
            }

            try
            {
                var response = await this.client.SendEmailAsync(message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
