using Brainbay.Characters.Integrations.RickAndMorty.Models;

namespace Brainbay.Characters.Integrations.RickAndMorty.Services;

public interface IRickAndMortyApiClient
{
    Task<GetCharacterPageResponse?> GetCharactersAsync(GetCharactersRequest request);
}