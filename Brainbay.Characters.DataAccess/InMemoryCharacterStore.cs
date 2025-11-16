using Brainbay.Characters.DataAccess.Options;
using Brainbay.Characters.Contracts;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Brainbay.Characters.DataAccess;

internal sealed class InMemoryCharacterStore(
    ICharacterStore fallbackStore,
    IMemoryCache memoryCache,
    IOptions<CharacterCacheOptions> cacheOptions)
    : ICharacterStore
{
    private const string CharactersCacheKey = "characters";

    private CharacterCacheOptions CacheOptions => cacheOptions.Value;

    public async Task RegisterCharacterAsync(RegisterCharacterRequest request)
    {
        await fallbackStore.RegisterCharacterAsync(request);
        
        memoryCache.Remove(CharactersCacheKey);
    }

    public async Task<GetCharactersResponse> GetCharactersAsync(GetCharactersRequest request)
    {
        var cacheResponse = await ExtractCachedCharactersAsync();

        var page = cacheResponse.Characters
            .Where(x => x.Id >= request.LatestId.GetValueOrDefault())
            .Take(request.PageSize)
            .ToList();

        return new GetCharactersResponse(page, cacheResponse.DataSource);
    }

    private async Task<GetCharactersResponse> ExtractCachedCharactersAsync()
    {
        var dataSource = DataSource.Cache;

        var characters = await memoryCache.GetOrCreateAsync(CharactersCacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheOptions.MaxCacheDuration;

            var getAllRequest = new GetCharactersRequest(pageSize: int.MaxValue, latestId: null);

            var response = await fallbackStore.GetCharactersAsync(getAllRequest);
            dataSource = response.DataSource;

            return response.Characters;
        }) ?? [];
        
        return new GetCharactersResponse(characters, dataSource);
    }
}