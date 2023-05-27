using HangfireAndSendGrid.Models;

namespace HangfireAndSendGrid.Contracts;

public interface IEmailSenderService
{
    Task<bool> SendEmailAsync(Email email);
}