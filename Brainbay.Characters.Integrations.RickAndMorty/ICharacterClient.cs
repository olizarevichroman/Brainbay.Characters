using Brainbay.Characters.Integrations.RickAndMorty.Models;

namespace Brainbay.Characters.Integrations.RickAndMorty;

public interface ICharacterClient
{
    Task<GetCharacterPageResponse> GetCharactersAsync(GetCharactersRequest request);
}