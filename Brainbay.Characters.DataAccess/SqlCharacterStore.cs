using System.Collections.Immutable;
using Brainbay.Characters.DataAccess.Sql;
using Brainbay.Characters.Contracts;
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
                Name = request.Name,
                Status = request.Status,
                Gender = request.Gender,
                CreatedAt = timeProvider.GetUtcNow(),
            };

            await connection.ExecuteAsync(Queries.RegisterCharacter, param: parameters);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<GetCharactersResponse> GetCharactersAsync(GetCharactersRequest request)
    {
        var connection = connectionFactory.CreateConnection();

        try
        {
            await connection.OpenAsync();

            var parameters = new
            {
                Take = request.PageSize,
                LatestId = request.LatestId.GetValueOrDefault(),
            };

            var entities = await connection.QueryAsync<CharacterEntity>(Queries.GetCharacters, param: parameters);
            var characters = entities.Select(x => x.ToCharacter()).ToImmutableList();
            
            return new GetCharactersResponse(characters, DataSource.Database);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    private sealed record CharacterEntity(
        ulong Id,
        string Name,
        byte Status,
        byte Gender,
        DateTime CreatedAt,
        string ImageUrl)
    {
        public Character ToCharacter() => new(
            (int)Id,
            Name,
            (CharacterStatus)Status,
            (CharacterGender)Gender,
            new DateTimeOffset(CreatedAt),
            new Uri(ImageUrl));
    }
}