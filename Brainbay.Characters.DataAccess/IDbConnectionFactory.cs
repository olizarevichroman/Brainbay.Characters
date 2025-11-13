using System.Data.Common;

namespace Brainbay.Characters.DataAccess;

public interface IDbConnectionFactory
{
    DbConnection CreateConnection();
}