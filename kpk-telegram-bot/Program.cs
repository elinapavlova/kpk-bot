using kpk_telegram_bot.Common.Contracts.Services;
using kpk_telegram_bot.Common.Database;
using kpk_telegram_bot.Common.Extensions;
using kpk_telegram_bot.Common.Helpers;
using kpk_telegram_bot.Core.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace kpk_telegram_bot;

internal static class Program
{
    private static IConfiguration _configuration;
    private static IBotService? _botService;

    public static async Task Main(string[] args)
    {
        var services = new ServiceCollection();
        Configure(services);
        
        await using var provider = services.BuildServiceProvider();
        await provider.Migrate();
        
        _botService = provider.GetRequiredService<IBotService>();
        await _botService.Work();
    }
    
    public static IHostBuilder CreateHostBuilder(string[] args)
        => Host.CreateDefaultBuilder(args)
            .ConfigureServices(Configure)
            .UseSerilog();

    private static async Task Migrate(this IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<KpkTelegramBotContext>();
        await context.Database.MigrateAsync();
    }
    
    private static void Configure(this IServiceCollection services)
    {
        _configuration = ConfigurationHelper.Build();
        var connection = _configuration.GetConnectionString("KpkTelegramBotDatabase");
        
        services.AddDbContext<KpkTelegramBotContext>(options =>
            options.UseSqlite(connection, x =>
            {
                x.MigrationsAssembly("kpk-telegram-bot");
                x.MigrationsHistoryTable("kpk_telegram_bot_Migrations_History", "public");
            }));
        
        var projectFolderPath = Path.GetDirectoryName(AppContext.BaseDirectory);
        var logPath = Path.Join(
            projectFolderPath,
            "logs",
            "log_info_.log");

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(_configuration)
            .WriteTo.File(
                logPath,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7)
            .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
            .CreateLogger();
        
        services.Register<RepositoriesProvider>();
        services.Register<ServicesProvider>();
        services.Register<HandlersProvider>();
        services.Register<CommandsProvider>();
        services.Register<OptionsProvider>(_configuration);
        services.Register<HttpClientsProvider>();
        services.Register<LoggerProvider>();
    }
}