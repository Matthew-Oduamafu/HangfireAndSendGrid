﻿using Microsoft.Extensions.Options;

namespace HangfireAndSendGrid.Models;

public class DatabaseOptionSetup : IConfigureOptions<DatabaseOption>
{
    private readonly IConfiguration _configuration;

    public DatabaseOptionSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(DatabaseOption options)
    {
        options.ConnectionString = _configuration.GetConnectionString("Default")!;
        _configuration.GetSection(nameof(DatabaseOption)).Bind(options);
    }
}