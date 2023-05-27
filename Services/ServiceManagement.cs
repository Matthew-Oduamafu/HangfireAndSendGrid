using HangfireAndSendGrid.Contracts;
using HangfireAndSendGrid.Models;
using Microsoft.EntityFrameworkCore;

namespace HangfireAndSendGrid.Services;

public class ServiceManagement : IServiceManagement
{
    private readonly HangfireSendGridDbContext _dbContext;
    private readonly IEmailSenderService _emailSenderService;

    public ServiceManagement(HangfireSendGridDbContext dbContext, IEmailSenderService emailSenderService)
    {
        _dbContext = dbContext;
        _emailSenderService = emailSenderService;
    }

    public async Task SendBatchMail()
    {
        var users = await _dbContext.Users.ToListAsync();

        var response = await _emailSenderService.SendEmailAsync(
            users,
            subject: "Monthly Subscription News Letter",
            plaintext: EmailTemplates.HtmlEmailTemplate,
            htmlContext: EmailTemplates.HtmlEmailTemplate);

        if (!response) Console.WriteLine("Batch mail failed to send");
    }
}