using Brainbay.Characters.DataAccess.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MySql;

namespace Brainbay.Characters.Tests;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private static readonly MySqlOptions MySqlOptions = new()
    {
        Server = "localhost",
        Database = "brainbay",
        UserId = "root",
        Password = "password"
    };

    private readonly MySqlContainer _mySqlContainer = new MySqlBuilder()
        .WithImage("mysql:8.0")
        .WithDatabase(MySqlOptions.Database)
        .WithUsername(MySqlOptions.UserId)
        .WithPassword(MySqlOptions.Password)
        .WithCleanUp(true)
        .Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        
        builder.ConfigureServices(services =>
        {
            services.Configure<MySqlOptions>(options =>
            {
                options.Server = _mySqlContainer.Hostname;
                options.Database = MySqlOptions.Database;
                options.Port = _mySqlContainer.GetMappedPublicPort();
            });
        });
    }

    public async ValueTask InitializeAsync()
    {
        await _mySqlContainer.StartAsync();
    }

    public override async ValueTask DisposeAsync()
    {
        await _mySqlContainer.StopAsync();
    }
}