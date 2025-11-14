using Brainbay.Characters.DataAccess;
using Brainbay.Characters.DataAccess.Options;
using Brainbay.Characters.WebApi.HostedServices;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddControllers();
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
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

await app.RunAsync();