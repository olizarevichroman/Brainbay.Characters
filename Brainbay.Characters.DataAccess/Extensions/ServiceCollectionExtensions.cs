using Microsoft.Extensions.DependencyInjection;

namespace Brainbay.Characters.DataAccess.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataAccessServices(this IServiceCollection services)
    {
        services.AddTransient<ICharacterStore, SqlCharacterStore>();
        services.AddTransient<ICharacterBatchStore, SqlCharacterBatchStore>();

        return services;
    }
}