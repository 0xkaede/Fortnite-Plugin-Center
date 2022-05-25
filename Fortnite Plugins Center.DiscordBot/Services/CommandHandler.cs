using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortnite_Plugins_Center.DiscordBot.Services
{
    internal class CommandHandler
    {
        private static IServiceProvider _prividor;
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;

        public CommandHandler(DiscordSocketClient discord, CommandService commands, IServiceProvider provider)
        {
            _prividor = provider;
            _discord = discord;
            _commands = commands;

            _discord.Ready += OnReady;
            _discord.MessageReceived += OnMessageRecived;
            _discord.JoinedGuild += OnJoinedGuild;
            _discord.LeftGuild += OnJLeftGuild;
        }

        private async Task OnJLeftGuild(SocketGuild Guild)
            => Console.WriteLine($"We Left {Guild.Name}");

        private async Task OnJoinedGuild(SocketGuild Guild)
            => Console.WriteLine($"We Joined {Guild.Name}");

        private async Task OnMessageRecived(SocketMessage arg)
        {
            var msg = arg as SocketUserMessage;

            var context = new SocketCommandContext(_discord, msg);

            int pos = 0;
            if (msg.HasStringPrefix(Configuration.DiscordPrefix, ref pos) || msg.HasMentionPrefix(_discord.CurrentUser, ref pos))
            {
                var usercommand = context.User.Username + "#" + context.User.Discriminator;
                var result = await _commands.ExecuteAsync(context, pos, _prividor);
                if (!result.IsSuccess)
                {
                    var reason = result.Error;

                    await context.Channel.SendMessageAsync($"The following error occured: \n {reason}");

                    Console.WriteLine($"The following error occured from an executed command from {usercommand}: {reason}");
                }
            }
        }

        private Task OnReady()
        {
            var TOKEN = Configuration.DiscordToken;
            Console.WriteLine($"Discord bot created in Discord.NET, by 0xkaede");
            Console.WriteLine($"Connected as username: {_discord.CurrentUser.Username}#{_discord.CurrentUser.Discriminator}");
            Console.WriteLine($"Connected as ID: {_discord.CurrentUser.Id}");
            Console.WriteLine($"Bot using Token that ends with: {TOKEN}");
            Console.WriteLine($"Bot is currently in {_discord.Guilds.Count} Guild");
            //foreach (var guild in _discord.Guilds)
            //{
            //    string[] guildinfo = { guild.Name };

            //    Console.WriteLine($"{guildinfo[0]}");
            //}
            return Task.CompletedTask;
        }
    }
}
