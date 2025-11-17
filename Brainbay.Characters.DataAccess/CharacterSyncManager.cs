using System.Collections.Immutable;
using Brainbay.Characters.Contracts;
using Brainbay.Characters.DataAccess.Extensions;

namespace Brainbay.Characters.DataAccess;

internal sealed class CharacterSyncManager(ICharacterBatchStore batchStore) : ICharacterSyncManager
{
    public Task RegisterCharactersAsync(IReadOnlyCollection<Character> characters)
    {
        var dtoCollection = characters
            .Select(x => x.ToDto())
            .ToImmutableList();
        
        return batchStore.RegisterCharactersAsync(dtoCollection);
    }

    public Task CleanupAsync() => batchStore.CleanupAsync();
}