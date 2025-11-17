using System.Collections.Immutable;
using Brainbay.Characters.Contracts;
using Brainbay.Characters.Integrations.RickAndMorty.Services;
using GetCharactersRequest = Brainbay.Characters.Integrations.RickAndMorty.Models.GetCharactersRequest;

namespace Brainbay.Characters.Application.Services;

internal sealed class CharacterSyncService(
    IRickAndMortyApiClient apiClient,
    ICharacterSyncManager syncManager)
    : ICharacterSyncService
{
    public async Task SyncCharactersAsync()
    {
        await syncManager.CleanupAsync();

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
                .Select(x => new Character(
                    x.Id,
                    x.Name,
                    x.Species,
                    Enum.Parse<CharacterStatus>(x.Status, ignoreCase: true),
                    Enum.Parse<CharacterGender>(x.Gender, ignoreCase: true),
                    x.Created,
                    new Uri(x.Image)))
                .ToImmutableArray();
            
            await syncManager.RegisterCharactersAsync(characters);

            request = response.NextPageRequest;
    
        } while (request is not null);
    }
}