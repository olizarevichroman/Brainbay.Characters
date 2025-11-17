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
            .Skip(request.Skip)
            .Take(request.Take)
            .ToList();

        return new GetCharactersResponse(page, cacheResponse.DataSource, cacheResponse.TotalCount);
    }

    private async Task<GetCharactersResponse> ExtractCachedCharactersAsync()
    {
        var dataSource = DataSource.Cache;

        var response = await memoryCache.GetOrCreateAsync(CharactersCacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheOptions.MaxCacheDuration;

            var getAllRequest = new GetCharactersRequest(skip: 0, take: int.MaxValue);

            var response = await fallbackStore.GetCharactersAsync(getAllRequest);
            dataSource = response.DataSource;

            return response;
        });
        
        return new GetCharactersResponse(response!.Characters, dataSource, response.TotalCount);
    }
}