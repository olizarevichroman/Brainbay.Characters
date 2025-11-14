using Brainbay.Characters.Application.Services;
using Brainbay.Characters.Integrations.RickAndMorty.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Brainbay.Characters.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddRickAndMortyClient();

        return services.AddSingleton<ICharacterSyncService, CharacterSyncService>();
    }
}