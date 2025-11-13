using Brainbay.Characters.DataAccess.Models;

namespace Brainbay.Characters.DataAccess;

public interface ICharacterStore
{
    Task RegisterCharacterAsync(RegisterCharacterRequest request);

    Task<GetCharactersResponse> GetCharactersAsync();
}