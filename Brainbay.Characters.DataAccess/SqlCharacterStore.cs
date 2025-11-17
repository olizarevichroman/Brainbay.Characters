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
                Species = request.Species,
                Status = request.Status,
                Gender = request.Gender,
                CreatedAt = timeProvider.GetUtcNow(),
                ImageUrl = request.ImageUrl.ToString(),
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
                Skip = request.Skip,
                Take = request.Take,
            };

            var reader = await connection.QueryMultipleAsync(Queries.GetCharacters, param: parameters);

            var characterEntities = await reader.ReadAsync<CharacterEntity>();
            var totalCount = await reader.ReadSingleAsync<int>();

            var characters = characterEntities.Select(x => x.ToCharacter()).ToImmutableList();
            
            return new GetCharactersResponse(characters, DataSource.Database, totalCount);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    private sealed record CharacterEntity(
        ulong Id,
        string Name,
        string Species,
        byte Status,
        byte Gender,
        DateTime CreatedAt,
        string ImageUrl)
    {
        public Character ToCharacter() => new(
            (int)Id,
            Name,
            Species,
            (CharacterStatus)Status,
            (CharacterGender)Gender,
            new DateTimeOffset(CreatedAt),
            new Uri(ImageUrl));
    }
}