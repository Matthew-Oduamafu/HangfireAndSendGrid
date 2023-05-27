using Hangfire;
using HangfireAndSendGrid.Contracts;
using HangfireAndSendGrid.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HangfireAndSendGrid.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ApplicationUsersController : ControllerBase
{
    private readonly HangfireSendGridDbContext _dbContext;
    private readonly IEmailSenderService _emailSenderService;

    public ApplicationUsersController(HangfireSendGridDbContext dbContext, IEmailSenderService emailSenderService)
    {
        _dbContext = dbContext;
        _emailSenderService = emailSenderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _dbContext.Users.ToListAsync();

        return Ok(users);
    }

    [HttpGet]
    public async Task<IActionResult> GetSingleUser([FromQuery] int id)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (user == null) return NotFound();

        // try sending mail
        try
        {
            Email email = new()
            {
                ToName = user.Name,
                To = user.EmailAddress,
                Subject = "Email Services Using SendGrid",
                HtmlContent = EmailTemplates.HtmlEmailTemplate,
                PlainContent = EmailTemplates.HtmlEmailTemplate
            };

            var jobId = BackgroundJob.Enqueue<IEmailSenderService>(x => x.SendEmailAsync(email));
            
            // var result = await _emailSenderService.SendEmailAsync(email);
            // if (result) Console.WriteLine("Email sent successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        return Ok(user);
    }
}