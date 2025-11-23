using Brainbay.Characters.Contracts;

namespace Brainbay.Characters.DataAccess;

internal interface ICharacterStore
{
    Task<int> RegisterCharacterAsync(RegisterCharacterRequest request, DateTimeOffset now);

    Task<GetCharactersResponse> GetCharactersAsync(GetCharactersRequest request);
}
