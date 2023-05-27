using Hangfire;
using Hangfire.Storage.SQLite;
using HangfireAndSendGrid;
using HangfireAndSendGrid.Contracts;
using HangfireAndSendGrid.Models;
using HangfireAndSendGrid.Services;
using HangfireBasicAuthenticationFilter;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureOptions<DatabaseOptionSetup>();
builder.Services.ConfigureOptions<EmailSettingSetup>();

builder.Services.AddTransient<IEmailSenderService, EmailSenderService>();
builder.Services.AddTransient<IServiceManagement, ServiceManagement>();


// add dbcontext
builder.Services.AddDbContext<HangfireSendGridDbContext>((serviceProvider, dbContextBuilder) =>
{
    var databaseOptions = serviceProvider.GetService<IOptions<DatabaseOption>>()!.Value;

    dbContextBuilder.UseSqlServer(
        databaseOptions.ConnectionString,
        sqlActionOptions =>
        {
            sqlActionOptions.EnableRetryOnFailure(
                databaseOptions.MaxRetryCount,
                TimeSpan.FromSeconds(databaseOptions.MaxRetryDelay),
                databaseOptions.ErrorNumbersToAdd
            );

            sqlActionOptions.CommandTimeout(databaseOptions.CommandTimeout);
        }
    );
});

// add hangfire
builder.Services.AddHangfire(
    opt =>
        opt.UseSQLiteStorage(
            builder.Configuration.GetValue<string>("SqliteDb:SqliteDbName")
        )
);
builder.Services.AddHangfireServer();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHangfireDashboard();
app.MapHangfireDashboard("/hanfire", new DashboardOptions
{
    DashboardTitle = "SendGrid Email Sender & Hangfire",
    Authorization = new[]
    {
        new HangfireCustomBasicAuthenticationFilter
        {
            Pass = "mattie",
            User = "Euler"
        }
    }
});

app.UseAuthorization();

app.MapControllers();


#pragma warning disable CS0618
// send news letter mail in every 2 minutes interval
RecurringJob.AddOrUpdate<IServiceManagement>(x => x.SendBatchMail(), "0 */2 * ? * *");
#pragma warning restore CS0618

app.Run();