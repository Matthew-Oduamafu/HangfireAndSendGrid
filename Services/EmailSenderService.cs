﻿using System.Net;
using HangfireAndSendGrid.Contracts;
using HangfireAndSendGrid.Models;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace HangfireAndSendGrid.Services;

public class EmailSenderService : IEmailSenderService
{
    private readonly EmailSetting _emailSetting;

    public EmailSenderService(IOptions<EmailSetting> options)
    {
        _emailSetting = options.Value;
    }

    public async Task<bool> SendEmailAsync(Email email)
    {
        var client = new SendGridClient(_emailSetting.ApiKey);

        var to = new EmailAddress {Name = email.ToName, Email = email.To};
        var from = new EmailAddress {Name = _emailSetting.FromName, Email = _emailSetting.FromAddress};

        var msg = MailHelper.CreateSingleEmail(from, to, email.Subject,
            email.PlainContent, email.HtmlContent);
        var response = await client.SendEmailAsync(msg);

        if (response.IsSuccessStatusCode != true)
        {
            Console.WriteLine("Email wasn't sent");
            Console.WriteLine($"status code {response.StatusCode}");
            Console.WriteLine(response.ToString());
        }

        return response.StatusCode is HttpStatusCode.Accepted or HttpStatusCode.OK;
    }
}