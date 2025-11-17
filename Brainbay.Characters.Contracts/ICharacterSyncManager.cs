namespace Brainbay.Characters.Contracts;

public interface ICharacterSyncManager
{
    Task RegisterCharactersAsync(IReadOnlyCollection<Character> characters);

    Task CleanupAsync();
}