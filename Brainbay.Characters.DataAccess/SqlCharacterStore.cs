using System.Collections.Immutable;
using Brainbay.Characters.DataAccess.Extensions;
using Brainbay.Characters.DataAccess.Sql;
using Brainbay.Characters.Contracts;
using Dapper;

namespace Brainbay.Characters.DataAccess;

internal sealed class SqlCharacterStore(IDbConnectionFactory connectionFactory)
    : ICharacterStore
{
    public Task<int> RegisterCharacterAsync(RegisterCharacterRequest request, DateTimeOffset now) =>
        connectionFactory.TryAsync(async connection =>
        {
            var parameters = new
            {
                Name = request.Name,
                Species = request.Species,
                Status = request.Status,
                Gender = request.Gender,
                CreatedAt = now,
                ImageUrl = request.ImageUrl.ToString(),
            };

            return await connection.QuerySingleAsync<int>(Queries.RegisterCharacter, param: parameters);
        });

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
