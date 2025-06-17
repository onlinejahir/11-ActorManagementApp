using _11_ActorManagementApp.EmailServices.Contracts;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;


namespace _11_ActorManagementApp.EmailServices
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            this._emailSettings = emailSettings.Value;
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var email = CreateEmailMessage(toEmail, subject, body);
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                // Optional: Log the exception somewhere
                Console.WriteLine($"Error sending email: {ex.Message}");

                return false; // Email sending failed
            }
        }

        public async Task<bool> SendBulkEmailAsync(List<string> toEmails, string subject, string body)
        {
            try
            {
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);
                foreach (var toEmail in toEmails)
                {
                    var email = CreateEmailMessage(toEmail, subject, body);
                    await smtp.SendAsync(email);

                    // Optional: Delay to avoid rate limits
                    await Task.Delay(200); // 0.2 second delay between emails
                }
                await smtp.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                // Optional: Log the exception somewhere
                Console.WriteLine($"Error sending email: {ex.Message}");

                return false; // Email sending failed
            }
        }

        private MimeMessage CreateEmailMessage(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_emailSettings.SenderEmail));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = body
            };
            return email;
        }

    }
}
