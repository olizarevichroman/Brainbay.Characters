using Brainbay.Characters.Application.Extensions;
using Brainbay.Characters.Application.Services;
using Brainbay.Characters.DataAccess;
using Brainbay.Characters.DataAccess.Extensions;
using Brainbay.Characters.DataAccess.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddDataAccessServices();
builder.Services.AddHttpClient();

builder.Services.Configure<MySqlOptions>(builder.Configuration.GetSection("DataSources:MySql"));

builder.Services.AddSingleton<IDbConnectionFactory>(provider =>
{
    var options = provider.GetRequiredService<IOptions<MySqlOptions>>().Value;

    return new MySqlConnectionFactory(options);
});

var host = builder.Build();

using var scope = host.Services.CreateScope();
var syncService = scope.ServiceProvider.GetRequiredService<ICharacterSyncService>();

await syncService.SyncCharactersAsync();