using Brainbay.Characters.Domain;

namespace Brainbay.Characters.DataAccess;

public interface ICharacterStore
{
    Task RegisterCharacterAsync(RegisterCharacterRequest request);

    Task<GetCharactersResponse> GetCharactersAsync();
}