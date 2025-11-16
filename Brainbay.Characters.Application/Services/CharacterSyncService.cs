using System.Collections.Immutable;
using Brainbay.Characters.Contracts;
using Brainbay.Characters.DataAccess;
using Brainbay.Characters.Integrations.RickAndMorty.Services;
using CharacterDto = Brainbay.Characters.DataAccess.Models.CharacterDto;
using GetCharactersRequest = Brainbay.Characters.Integrations.RickAndMorty.Models.GetCharactersRequest;

namespace Brainbay.Characters.Application.Services;

internal sealed class CharacterSyncService(
    IRickAndMortyApiClient apiClient,
    ICharacterBatchStore characterBatchStore)
    : ICharacterSyncService
{
    public async Task SyncCharactersAsync(CancellationToken cancellationToken = default)
    {
        await characterBatchStore.CleanupAsync();

        var filters = new Dictionary<string, string>
        {
            { "status", nameof(CharacterStatus.Alive) },
        };

        var request = new GetCharactersRequest(filters);
        
        do
        {
            var response = await apiClient.GetCharactersAsync(request);

            if (response is null)
            {
                return;
            }

            var characters = response.Characters
                .Select(x => new CharacterDto(
                    x.Id,
                    x.Name,
                    Enum.Parse<CharacterStatus>(x.Status, ignoreCase: true),
                    Enum.Parse<CharacterGender>(x.Gender, ignoreCase: true),
                    x.Created,
                    x.Image))
                .ToImmutableArray();
            
            await characterBatchStore.RegisterCharactersAsync(characters);

            request = response.NextPageRequest;
    
        } while (request is not null);
    }
}