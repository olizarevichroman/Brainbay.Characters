using Brainbay.Characters.Contracts;

namespace Brainbay.Characters.DataAccess;

internal sealed class CharacterManager(ICharacterStore store, TimeProvider timeProvider) : ICharacterManager
{
    public Task<GetCharactersResponse> GetCharactersAsync(GetCharactersRequest request)
    {
        return store.GetCharactersAsync(request);
    }

    public async Task<Character> RegisterCharacterAsync(RegisterCharacterRequest request)
    {
        var now = timeProvider.GetUtcNow();
        var id = await store.RegisterCharacterAsync(request, now);

        return new Character(
            id,
            request.Name,
            request.Species,
            request.Status,
            request.Gender,
            now,
            request.ImageUrl);
    }
}
