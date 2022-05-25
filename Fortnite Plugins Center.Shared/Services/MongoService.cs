using Fortnite_Plugins_Center.Shared.Enums;
using Fortnite_Plugins_Center.Shared.Models.Mongo;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fortnite_Plugins_Center.Shared.Services
{
    public interface IMongoService
    {
        public void Ping();

        public Task InitDatabase();

        public Task<Data> GetData();

        public Task<List<Users>> GetUsers();

        public Task<UserResponse> CreateUser(DiscordInfomation info);

        public Task<UserResponse> AddPlugin(Users info, Plugin plugin);

        public Task<UserResponse> AddFollower(Users info, DiscordInfomation otherinfo);
    }

    public class MongoService : IMongoService
    {
        private readonly IMongoCollection<Data> _data;

        private readonly IMongoCollection<Users> _users;

        public MongoService(IMongoSettings mongoSettings)
        {
            var client = new MongoClient("mongodb://localhost:27017");

            var mongoDatabase = client.GetDatabase("Plugins");

            _data = mongoDatabase.GetCollection<Data>("data");

            _users = mongoDatabase.GetCollection<Users>("users");

            _ = InitDatabase();
        }

        public void Ping()
        {
        }

        public async Task InitDatabase()
        {
            if (await _data.CountDocumentsAsync(x => true) <= 0)
            {
                await _data.InsertOneAsync(new Data
                {
                    IsEnabled = true,
                    Version = "1.0.0.0"
                });
            }
        }

        public async Task<UserResponse> CreateUser(DiscordInfomation info)
        {
            var date = DateTime.Now.ToUniversalTime().ToString("dd-MM-yyyy HH:mm:ss");

            try
            {
                var users = await GetUsers();
                var user = users.FirstOrDefault(x => x.DiscordInfo.Id == info.Id);

                if (user == null)
                {
                    await _users.InsertOneAsync(new Users
                    {
                        CreatedDate = date,
                        LastUpdatedDate = date,
                        DiscordInfo = info,
                        Plugins = new List<Plugin>(),
                        Followers = new List<DiscordInfomation>(),
                        Following = new List<DiscordInfomation>(),
                    });
                    return UserResponse.Success;
                }
                else
                    return UserResponse.AlreadExits;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return UserResponse.Error;
            }
        }

        public async Task<UserResponse> AddPlugin(Users info, Plugin plugin)
        {
            var date = DateTime.Now.ToUniversalTime().ToString("dd-MM-yyyy HH:mm:ss");

            try
            {
                //i dunno what i did here but it wors :)
                var users = await GetUsers();
                var user = users.FirstOrDefault(x => x.DiscordInfo.Id == info.DiscordInfo.Id);

                List<Plugin> plugins = new List<Plugin>();

                foreach (var plug in user.Plugins)
                {
                    if (plug.Content != plugin.Content)
                    {
                        plugins.Add(plug);
                    }
                }

                plugins.Add(plugin);

                var filter = Builders<Users>.Filter.Eq(x => x.DiscordInfo.Id, info.DiscordInfo.Id);
                var update = Builders<Users>.Update.Set(x => x.Plugins, plugins).Set(x => x.LastUpdatedDate, date);

                await _users.UpdateManyAsync(filter, update);
                return UserResponse.Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return UserResponse.Error;
            }
        }

        public async Task<UserResponse> AddFollower(Users info, DiscordInfomation otherinfo)
        {
            var date = DateTime.Now.ToUniversalTime().ToString("dd-MM-yyyy HH:mm:ss");

            try
            {
                //i dunno what i did here but it wors :)
                var users = await GetUsers();
                var user = users.FirstOrDefault(x => x.DiscordInfo.Id == info.DiscordInfo.Id);

                List<DiscordInfomation> followers = new List<DiscordInfomation>();

                foreach (var friends in user.Following)
                    if (friends.Id != otherinfo.Id)
                        followers.Add(friends);

                followers.Add(otherinfo);

                var filter = Builders<Users>.Filter.Eq(x => x.DiscordInfo.Id, info.DiscordInfo.Id);
                var update = Builders<Users>.Update.Set(x => x.Following, followers).Set(x => x.LastUpdatedDate, date);

                await _users.UpdateManyAsync(filter, update);

                var filterO = Builders<Users>.Filter.Eq(x => x.DiscordInfo.Id, otherinfo.Id);
                var updateO = Builders<Users>.Update.Set(x => x.Followers, followers).Set(x => x.LastUpdatedDate, date);

                await _users.UpdateManyAsync(filterO, updateO);
                return UserResponse.Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return UserResponse.Error;
            }
        }

        public async Task<Data> GetData()
        {
            var data = await _data.FindAsync(x => true);
            return await data.FirstOrDefaultAsync();
        }

        public async Task<List<Users>> GetUsers()
        {
            var user = await _users.FindAsync(x => true);
            return user.ToList();
        }
    }
}
