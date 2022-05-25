using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fortnite_Plugins_Center.DiscordBot.Services
{
    internal class StartupService
    {
        private static IServiceProvider _prividor;
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;

        public StartupService(IServiceProvider provider, DiscordSocketClient discord, CommandService commands)
        {
            _prividor = provider;
            _discord = discord;
            _commands = commands;
        }

        public async Task StartAsync()
        {
            string token = Configuration.DiscordToken;
            if (string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine("Please enter in your token");
                return;
            }

            await _discord.LoginAsync(TokenType.Bot, token);
            await _discord.StartAsync();

            await _discord.SetStatusAsync(UserStatus.DoNotDisturb);
            await _discord.SetActivityAsync(new Game("Created by 0xkaede", ActivityType.Playing, ActivityProperties.None)).ConfigureAwait(false);
            await _commands.AddModulesAsync(Assembly.GetExecutingAssembly(), _prividor);
        }
    }
}
