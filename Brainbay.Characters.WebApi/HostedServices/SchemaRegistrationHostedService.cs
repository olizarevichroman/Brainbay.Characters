using Brainbay.Characters.DataAccess;
using Brainbay.Characters.DataAccess.Sql;
using Dapper;

namespace Brainbay.Characters.WebApi.HostedServices;

internal sealed class SchemaRegistrationHostedService(
    IDbConnectionFactory connectionFactory,
    ILogger<SchemaRegistrationHostedService> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var connection = connectionFactory.CreateConnection();

            try
            {
                await connection.OpenAsync(stoppingToken);

                foreach (var migration in Migrations.List)
                {
                    logger.LogInformation("Migration '{MigrationName}' started.", migration.Name);

                    await connection.ExecuteAsync(migration.Query, stoppingToken);

                    logger.LogInformation("Migration '{MigrationName}' succeeded.", migration.Name);
                }

                return;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured during migration.");
            }
            finally
            {
                await connection.CloseAsync();
            }
            
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}