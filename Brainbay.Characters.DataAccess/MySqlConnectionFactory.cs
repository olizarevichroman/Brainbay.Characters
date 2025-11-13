using System.Data.Common;
using MySql.Data.MySqlClient;

namespace Brainbay.Characters.DataAccess;

public sealed class MySqlConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public DbConnection CreateConnection()
    {
        return new MySqlConnection(connectionString);
    }
}