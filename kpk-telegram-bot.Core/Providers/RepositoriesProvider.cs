using kpk_telegram_bot.Common.Contracts.Providers;
using kpk_telegram_bot.Common.Contracts.Repositories;
using kpk_telegram_bot.Repositories.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace kpk_telegram_bot.Core.Providers;

public class RepositoriesProvider : IDependencyProvider
{
    public void Register(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IItemTypeRepository, ItemTypeRepository>();
        services.AddScoped<IItemPropertyTypeRepository, ItemPropertyTypeRepository>();
        services.AddScoped<IItemPropertyRepository, ItemPropertyRepository>();
    }
}