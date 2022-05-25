using Fortnite_Plugins_Center.Shared.Exceptions.Users;
using Fortnite_Plugins_Center.Shared.Models.Mongo;
using Fortnite_Plugins_Center.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fortnite_Plugins_Center.API.Controllers
{
    [ApiController]
    [Route("/api/users")]
    public class UserController : ControllerBase
    {
        private readonly IMongoService _mongoService;

        public UserController(IMongoService mongoService)
        {
            _mongoService = mongoService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Users>>> GetUsers()
            => await _mongoService.GetUsers();

        [HttpGet]
        [Route("createuser")]
        public async Task<ActionResult<Users>> GetNewUser(string id = "", string username = "", string discriminator = "", string pfp = "")
        {
            var discord = new DiscordInfomation
            {
                Id = ulong.Parse(id),
                UserName = username,
                Discriminator = discriminator,
                ProfilePic = pfp
            };

            var reponser = await _mongoService.CreateUser(discord);

            var response = new Users
            {
                Reponse = reponser,
                DiscordInfo = discord,
            };

            return response;
        }

        [HttpGet]
        [Route("id/{id}/addplugin")]
        public async Task<ActionResult<Users>> GetAddPlugin(string id, string content = "")
        {
            var date = DateTime.Now.ToUniversalTime().ToString("dd-MM-yyyy HH:mm:ss");

            if (string.IsNullOrEmpty(id))
                throw new InvalidQueryException("illegal id provided");

            var users = await _mongoService.GetUsers();
            var user = users.FirstOrDefault(x => x.DiscordInfo.Id == ulong.Parse(id));

            if (user == null)
                throw new UserNotFoundException(id.ToString());

            var newplug = JsonConvert.DeserializeObject<Content>(content);

            var plugin = new Plugin
            {
                AddedDate = date,
                Content = newplug,
                Likes = 0,
                DisLikes = 0,
            };

            var response = await _mongoService.AddPlugin(user, plugin);

            user.Reponse = response;
            user.Plugins.Add(plugin);

            return user;
        }

        [HttpGet]
        [Route("id/{MainID}/follow")]
        public async Task<ActionResult<Users>> GetFollow(string MainID, string id = "")
        {
            var date = DateTime.Now.ToUniversalTime().ToString("dd-MM-yyyy HH:mm:ss");

            if (string.IsNullOrEmpty(id))
                throw new InvalidQueryException("illegal id provided");

            var users = await _mongoService.GetUsers();
            var user = users.FirstOrDefault(x => x.DiscordInfo.Id == ulong.Parse(MainID));

            if (user == null)
                throw new UserNotFoundException(id.ToString());

            var OtherInfo = users.FirstOrDefault(x => x.DiscordInfo.Id == ulong.Parse(id)).DiscordInfo;

            var response = await _mongoService.AddFollower(user, OtherInfo);

            user.Reponse = response;
            user.Following.Add(OtherInfo);

            return user;
        } //beta test cba doing it

        [HttpGet]
        [Route("id/{id}")]
        public async Task<ActionResult<Users>> GetUserByID(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new InvalidQueryException("illegal id provided");

            var users = await _mongoService.GetUsers();
            var user = users.FirstOrDefault(x => x.DiscordInfo.Id == ulong.Parse(id));

            if (user == null)
                throw new UserNotFoundException(id.ToString());

            return user;
        }

    }
}
