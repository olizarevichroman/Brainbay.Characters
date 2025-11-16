using Brainbay.Characters.Contracts;
using Brainbay.Characters.DataAccess.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Brainbay.Characters.DataAccess.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataAccessServices(this IServiceCollection services)
    {
        services.AddSingleton<SqlCharacterStore>();

        services.AddSingleton<ICharacterStore>(provider =>
        {
            var sqlStore = provider.GetRequiredService<SqlCharacterStore>();
            var memoryCache = provider.GetRequiredService<IMemoryCache>();
            var cacheOptions = provider.GetRequiredService<IOptions<CharacterCacheOptions>>();
            
            return new InMemoryCharacterStore(sqlStore, memoryCache, cacheOptions);
        });

        services.AddTransient<ICharacterBatchStore, SqlCharacterBatchStore>();
        services.AddTransient<ICharacterManager, CharacterManager>();

        return services;
    }
}