using Brainbay.Characters.DataAccess.Models;

namespace Brainbay.Characters.DataAccess;

public interface ICharacterBatchStore
{
    Task RegisterCharactersAsync(IReadOnlyCollection<CharacterDto> characters);

    Task CleanupAsync();
}