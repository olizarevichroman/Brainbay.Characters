using Brainbay.Characters.Contracts;

namespace Brainbay.Characters.DataAccess;

internal sealed class CharacterManager(ICharacterStore store) : ICharacterManager
{
    public Task<GetCharactersResponse> GetCharactersAsync(GetCharactersRequest request)
    {
        return store.GetCharactersAsync(request);
    }

    public Task RegisterCharacterAsync(RegisterCharacterRequest request)
    {
        return store.RegisterCharacterAsync(request);
    }
}