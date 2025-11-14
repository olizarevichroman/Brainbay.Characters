using Brainbay.Characters.DataAccess.Models;
using Brainbay.Characters.DataAccess.Options;
using Brainbay.Characters.Domain;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Brainbay.Characters.DataAccess;

internal sealed class InMemoryCharacterStore(
    ICharacterStore fallbackStore,
    IMemoryCache memoryCache,
    IOptions<CharacterCacheOptions> cacheOptions)
    : ICharacterStore
{
    private CharacterCacheOptions CacheOptions => cacheOptions.Value;

    public Task RegisterCharacterAsync(RegisterCharacterRequest request)
    {
        // TODO mark stale data, if a character was successfully registered
        throw new NotImplementedException();
    }

    public async Task<GetCharactersResponse> GetCharactersAsync()
    {
        // TODO check for characters in memory and check whether data is marked as stale

        return await fallbackStore.GetCharactersAsync();
        
        // TODO 
    }
}