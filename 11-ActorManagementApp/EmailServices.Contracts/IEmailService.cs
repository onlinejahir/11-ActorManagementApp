namespace _11_ActorManagementApp.EmailServices.Contracts
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string toEmail, string subject, string body);
        Task<bool> SendBulkEmailAsync(List<string> toEmails, string subject, string body);
    }
}
