namespace Fortnite_Plugins_Center.Shared.Models.Mongo
{
    public interface IMongoSettings
    {
        public string Hostname { get; set; }

        public int Port { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        public string Database { get; set; }

        public CollectionSettings Collections { get; set; }
    }

    public class MongoSettings : IMongoSettings
    {
        public string Hostname { get; set; }

        public int Port { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        public string Database { get; set; }

        public CollectionSettings Collections { get; set; }
    }

    public class CollectionSettings
    {
        public string Data { get; set; }

        public string Items { get; set; }
    }
}
