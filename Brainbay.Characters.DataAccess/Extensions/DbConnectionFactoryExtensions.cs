using System.Data.Common;

namespace Brainbay.Characters.DataAccess.Extensions;

public static class DbConnectionFactoryExtensions
{
    public static async Task<T> TryAsync<T>(
        this IDbConnectionFactory connectionFactory,
        Func<DbConnection, Task<T>> func)
    {
        var connection = connectionFactory.CreateConnection();

        try
        {
            await connection.OpenAsync();

            return await func(connection);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }
}
