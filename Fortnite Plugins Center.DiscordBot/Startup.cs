using Discord.Commands;
using Discord.WebSocket;
using Fortnite_Plugins_Center.DiscordBot.Services;
using Fortnite_Plugins_Center.Shared.Models.Mongo;
using Fortnite_Plugins_Center.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Fortnite_Plugins_Center.DiscordBot
{
    internal class Startup
    {
        public Startup(string[] args)
        {

        }

        public static async Task RunAsync(string[] args)
        {
            var startup = new Startup(args);
            await startup.RunAsync();
        }

        public async Task RunAsync()
        {
            var services = new ServiceCollection();
            ConfigurationServices(services);

            var provider = services.BuildServiceProvider();
            provider.GetRequiredService<CommandHandler>();

            await provider.GetRequiredService<StartupService>().StartAsync();
            await Task.Delay(-1);
        }

        public void ConfigurationServices(IServiceCollection service)
        {
            service.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = Discord.LogSeverity.Verbose,
                MessageCacheSize = 1000
            }))
            .AddSingleton(new CommandService(new CommandServiceConfig
            {
                LogLevel = Discord.LogSeverity.Verbose,
                DefaultRunMode = RunMode.Async,
                CaseSensitiveCommands = false
            }))
            .AddSingleton<CommandHandler>()
            .AddSingleton<StartupService>();
        }
    }
}
