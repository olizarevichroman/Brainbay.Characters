using Brainbay.Characters.DataAccess.Models;

namespace Brainbay.Characters.DataAccess;

internal interface ICharacterBatchStore
{
    Task RegisterCharactersAsync(IReadOnlyCollection<CharacterDto> characters);

    Task CleanupAsync();
}