using Brainbay.Characters.Contracts;

namespace Brainbay.Characters.DataAccess;

internal interface ICharacterStore
{
    Task RegisterCharacterAsync(RegisterCharacterRequest request);

    Task<GetCharactersResponse> GetCharactersAsync(GetCharactersRequest request);
}