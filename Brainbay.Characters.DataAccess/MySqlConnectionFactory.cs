using System.Data.Common;
using Brainbay.Characters.DataAccess.Options;
using MySql.Data.MySqlClient;

namespace Brainbay.Characters.DataAccess;

public sealed class MySqlConnectionFactory(MySqlOptions options) : IDbConnectionFactory
{
    private readonly string _connectionString = new MySqlConnectionStringBuilder
    {
        Server = options.Server,
        Port = options.Port,
        Database = options.Database,
        UserID = options.UserId,
        Password = options.Password,
    }.ConnectionString;

    public DbConnection CreateConnection() => new MySqlConnection(_connectionString);

    public async Task TryAsync(Func<DbConnection, Task> action)
    {
        var connection = CreateConnection();

        try
        {
            await connection.OpenAsync();
            await action(connection);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }
}