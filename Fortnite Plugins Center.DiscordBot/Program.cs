using System.Threading.Tasks;

namespace Fortnite_Plugins_Center.DiscordBot
{
    class Program
    {
        public static async Task Main(string[] args)
            => await Startup.RunAsync(args);
    }
}
