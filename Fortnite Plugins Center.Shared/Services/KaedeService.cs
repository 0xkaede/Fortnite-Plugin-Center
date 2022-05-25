using Fortnite_Plugins_Center.Shared.Models.Mongo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fortnite_Plugins_Center.Shared.Services
{
    public interface IKaedeService
    {
        public Task<Data> GetData();

        public Task<List<Users>> GetUsers();

        public Task<Users> GetUsersById(string id);

        public Task<Users> GetUsersCreate(string id, string username, string discriminator, string pfp);

        public Task<Users> GetUsersAddPlugin(string id, string content);
    }

    public class KaedeService : IKaedeService
    {
        internal static class Endpoints
        {
            public static Uri Base = new Uri("http://localhost:443/api/");

            public static Uri Data = new Uri(Base, "data");
            public static Uri Users = new Uri(Base, "users");

            public static Uri UsersById(string id) => new Uri(Base, $"users/id/{id}");
            public static Uri UsersCreate(string id, string username, string discriminator, string pfp) => new Uri(Base, $"users/createuser?id={ulong.Parse(id)}&username={username}&discriminator={discriminator}&pfp={pfp}");
            public static Uri UsersAddPlugin(string id, string content) => new Uri(Base, $"users/id/{id}/addplugin?content={content}");
        }

        public static async Task<T> GetData<T>(Uri endpoint)
        {
            //logger
            return JsonConvert.DeserializeObject<T>(await new HttpClient().GetStringAsync(endpoint));
        }

        public async Task<Data> GetData()
            => await GetData<Data>(Endpoints.Data);

        public async Task<List<Users>> GetUsers()
            => await GetData<List<Users>>(Endpoints.Users);

        public async Task<Users> GetUsersById(string id)
            => await GetData<Users>(Endpoints.UsersById(id));

        public async Task<Users> GetUsersCreate(string id, string username, string discriminator, string pfp)
            => await GetData<Users>(Endpoints.UsersCreate(id, username, discriminator, pfp));

        public async Task<Users> GetUsersAddPlugin(string id, string content)
            => await GetData<Users>(Endpoints.UsersAddPlugin(id, content));
    }
}
