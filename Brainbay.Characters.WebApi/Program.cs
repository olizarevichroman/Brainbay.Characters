using System.Text.Json.Serialization;
using Brainbay.Characters.DataAccess;
using Brainbay.Characters.DataAccess.Extensions;
using Brainbay.Characters.DataAccess.Options;
using Brainbay.Characters.WebApi.Blazor;
using Brainbay.Characters.WebApi.HostedServices;
using Microsoft.Extensions.Options;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddDataAccessServices();

builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddMudServices();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.Configure<CharacterCacheOptions>(builder.Configuration.GetSection("Options:Characters"));
builder.Services.Configure<MySqlOptions>(builder.Configuration.GetSection("DataSources:MySql"));

builder.Services.AddSingleton<IDbConnectionFactory>(provider =>
{
    var options = provider.GetRequiredService<IOptions<MySqlOptions>>().Value;

    return new MySqlConnectionFactory(options);
});

builder.Services.AddHostedService<SchemaRegistrationHostedService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.EnableTryItOutByDefault());
}

app.UseStaticFiles();

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .DisableAntiforgery();

await app.RunAsync();