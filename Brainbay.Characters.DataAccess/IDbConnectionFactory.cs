using System.Data.Common;

namespace Brainbay.Characters.DataAccess;

public interface IDbConnectionFactory
{
    DbConnection CreateConnection();

    Task TryAsync(Func<DbConnection, Task> action);
}