using Brainbay.Characters.Integrations.RickAndMorty.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Brainbay.Characters.Integrations.RickAndMorty.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRickAndMortyClient(this IServiceCollection services)
    {
        return services.AddSingleton<IRickAndMortyApiClient, RickAndMortyApiClient>();
    }
}