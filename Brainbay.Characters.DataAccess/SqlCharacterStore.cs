using System.Collections.Immutable;
using Brainbay.Characters.DataAccess.Models;
using Brainbay.Characters.DataAccess.Sql;
using Brainbay.Characters.Domain;
using Dapper;

namespace Brainbay.Characters.DataAccess;

internal sealed class SqlCharacterStore(IDbConnectionFactory connectionFactory, TimeProvider timeProvider)
    : ICharacterStore
{
    public async Task RegisterCharacterAsync(RegisterCharacterRequest request)
    {
        var connection = connectionFactory.CreateConnection();

        try
        {
            await connection.OpenAsync();

            var parameters = new
            {
                Name = string.Empty,
                Status = string.Empty,
                CreatedAt = timeProvider.GetUtcNow(),
            };

            await connection.ExecuteAsync(Queries.RegisterCharacter, param: parameters);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<GetCharactersResponse> GetCharactersAsync()
    {
        var connection = connectionFactory.CreateConnection();

        try
        {
            await connection.OpenAsync();

            var entities = await connection.QueryAsync<CharacterEntity>(Queries.GetCharacters);
            var characters = entities.Select(x => x.ToCharacter()).ToImmutableList();
            
            return new GetCharactersResponse(characters, DataSource.Database);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    private sealed record CharacterEntity(
        int Id,
        string Name,
        CharacterStatus Status,
        CharacterGender Gender,
        DateTime CreatedAt)
    {
        public Character ToCharacter() => new(Id, Name, Status, Gender, new DateTimeOffset(CreatedAt));
    }
}