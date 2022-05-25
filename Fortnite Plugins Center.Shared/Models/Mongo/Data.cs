using MongoDB.Bson.Serialization.Attributes;
using B = MongoDB.Bson.Serialization.Attributes.BsonElementAttribute;

namespace Fortnite_Plugins_Center.Shared.Models.Mongo
{
    [BsonIgnoreExtraElements]
    public class Data
    {
        [B("version")] public string Version { get; set; }

        [B("isEnabled")] public bool IsEnabled { get; set; } = true;
    }
}
