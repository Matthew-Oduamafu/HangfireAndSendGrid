using HangfireAndSendGrid.Models;

namespace HangfireAndSendGrid.Contracts;

public interface IEmailSenderService
{
    Task<bool> SendEmailAsync(Email email);

    Task<bool> SendEmailAsync(IReadOnlyList<User> users, string subject, string plaintext, string htmlContext);
}