using Brainbay.Characters.DataAccess.Models;
using Brainbay.Characters.DataAccess.Sql;
using Dapper;
using Z.Dapper.Plus;

namespace Brainbay.Characters.DataAccess;

public sealed class SqlCharacterBatchStore(IDbConnectionFactory connectionFactory) : ICharacterBatchStore
{
    public Task RegisterCharactersAsync(IReadOnlyCollection<CharacterDto> characters) =>
        connectionFactory.TryAsync(async connection =>
        {
            await connection
                .UseBulkOptions(x => x.DestinationTableName = "Characters")
                .BulkInsertAsync(characters);
        });

    public Task CleanupAsync() => connectionFactory.TryAsync(async connection =>
    {
        await connection.ExecuteAsync(Queries.CleanupCharacters);
    });
}