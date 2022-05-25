using Fortnite_Plugins_Center.Shared.Enums;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using B = MongoDB.Bson.Serialization.Attributes.BsonElementAttribute;

namespace Fortnite_Plugins_Center.Shared.Models.Mongo
{
    [BsonIgnoreExtraElements]
    public class Users
    {
        [B("Response")] public UserResponse Reponse { get; set; }

        [B("CreatedDate")] public string CreatedDate { get; set; }

        [B("LastUpdatedDate")] public string LastUpdatedDate { get; set; }

        [B("Following")] public List<DiscordInfomation> Following { get; set; } = new List<DiscordInfomation>();

        [B("Followers")] public List<DiscordInfomation> Followers { get; set; } = new List<DiscordInfomation>();

        [B("DiscordInfo")] public DiscordInfomation DiscordInfo { get; set; }

        [B("Plugins")] public List<Plugin> Plugins { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class DiscordInfomation
    {
        [B("ID")] public ulong Id { get; set; }

        [B("UserName")] public string UserName { get; set; }

        [B("Discriminator")] public string Discriminator { get; set; }

        [B("ProfilePicture")] public string ProfilePic { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Plugin
    {
        [B("AddedDate")] public string AddedDate { get; set; }

        [B("Likes")] public int Likes { get; set; }

        [B("Dislikes")] public int DisLikes { get; set; }

        [B("Name")] public string Name { get; set; }

        [B("Content")] public Content Content { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Content
    {
        [B("Name")] public string Name { get; set; }

        [B("Icon")] public string Icon { get; set; }

        [B("SwapIcon")] public string SwapIcon { get; set; }

        [B("Type")] public int Type { get; set; }

        [B("Messages")] public List<Messages> Messages { get; set; }

        [B("isDownloadLink")] public bool isDownloadLink { get; set; }

        [B("DownloadLink")] public string DownloadLink { get; set; }

        [B("PluginMain")] public PluginMain PluginMain { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class PluginMain
    {
        [B("Name")] public string Name { get; set; }

        [B("Icon")] public string Icon { get; set; }

        [B("Swapicon")] public string Swapicon { get; set; }

        [B("Type")] public string Type { get; set; }

        [B("Messages")] public List<Messages> Message { get; set; }

        [B("Assets")] public List<Assets> Assets { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Assets
    {
        [B("CompressionType")] public string CompressionType { get; set; }

        [B("AssetPath")] public string AssetPath { get; set; }

        [B("AssetUcas")] public string AssetUcas { get; set; }

        [B("Swaps")] public List<Swaps> Swaps { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Swaps
    {
        [B("type")] public string type { get; set; }

        [B("search")] public string search { get; set; }

        [B("replace")] public string replace { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class Messages
    {
        [B("Title")] public string Title { get; set; }

        [B("Message")] public string Message { get; set; }

        [B("Type")] public MessageInfo Type { get; set; } = MessageInfo.Information;
    }
}
