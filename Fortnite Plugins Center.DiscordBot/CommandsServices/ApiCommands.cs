using Discord.Commands;
using Fortnite_Plugins_Center.Shared.Models.Mongo;
using Fortnite_Plugins_Center.Shared.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Fortnite_Plugins_Center.Shared.Enums;
using System.IO;

namespace Fortnite_Plugins_Center.DiscordBot.CommandsServices
{
    public class ApiCommands : ModuleBase
    {
        KaedeService _kaedeService;

        [Command("signup")]
        public async Task signup()
        {
            _kaedeService = new KaedeService();

            var Infomation = await _kaedeService.GetUsersCreate(Context.User.Id.ToString(), Context.User.Username, Context.User.Discriminator, Context.User.GetAvatarUrl(Discord.ImageFormat.Png, 128));

            if (Infomation.Reponse == UserResponse.Success)
                await Context.Channel.SendMessageAsync("User was added to our database!");
            else if (Infomation.Reponse == UserResponse.AlreadExits)
                await Context.Channel.SendMessageAsync("Looks like your account already exits!");
            else if (Infomation.Reponse == UserResponse.Updated)
                await Context.Channel.SendMessageAsync("For some reason we got a Updated Response - Please report this to an member of our admin team");
            else if (Infomation.Reponse == UserResponse.Error)
                await Context.Channel.SendMessageAsync("An error has been found!");
        }

        [Command("addplugin")]
        public async Task addplugin()
        {
            var attachments = Context.Message.Attachments;

            RestClient client = new RestClient(new Uri(attachments.ElementAt(0).Url));
            RestRequest request = new RestRequest(Method.GET);

            byte[] buffer = client.DownloadData(request);

            string download = Encoding.UTF8.GetString(buffer);

            Console.WriteLine(download);

            _kaedeService = new KaedeService();

            var Infomation = await _kaedeService.GetUsersAddPlugin(Context.User.Id.ToString(), download);

            if (Infomation.Reponse == UserResponse.Success)
                await Context.Channel.SendMessageAsync("Plugin was added to your account!");
            else if (Infomation.Reponse == UserResponse.AlreadExits)
                await Context.Channel.SendMessageAsync("Looks like this already exits!");
            else if (Infomation.Reponse == UserResponse.Updated)
                await Context.Channel.SendMessageAsync("For some reason we got a Updated Response - Please report this to an member of our admin team");
            else if (Infomation.Reponse == UserResponse.Error)
                await Context.Channel.SendMessageAsync($"An error has been found, make sure you have used {Configuration.DiscordPrefix}signup to make a account!");
        }

        [Command("account")]
        public async Task account()
        {
            _kaedeService = new KaedeService();

            var data = await _kaedeService.GetUsersById(Context.User.Id.ToString());

            var fileText = JsonConvert.SerializeObject(data, Formatting.Indented);

            var filePath = Configuration.CurretnDirectory + $"{Context.User.Id}.json";

            if (!Directory.Exists(Configuration.CurretnDirectory))
                Directory.CreateDirectory(Configuration.CurretnDirectory);

            await File.WriteAllTextAsync(filePath, fileText);

            await Context.Channel.SendFileAsync(filePath);

            File.Delete(filePath);
        }
    }
}

