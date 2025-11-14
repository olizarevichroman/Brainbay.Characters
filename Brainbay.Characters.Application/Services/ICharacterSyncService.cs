namespace Brainbay.Characters.Application.Services;

public interface ICharacterSyncService
{
    Task SyncCharactersAsync(CancellationToken cancellationToken = default);
}