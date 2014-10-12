using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MineLib.Network.Modern.BaseClients
{
    public class Base64Converter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(byte[]);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var text = (string) reader.Value;

            // Cut non-used data
            text = text.Replace("data:image/png;base64,", "");

            return Convert.FromBase64String(text);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var bytes = (byte[])value;
            writer.WriteValue(Convert.ToBase64String(bytes));
        }
    }

    public struct Version
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("protocol")]
        public int Protocol;
    }

    public struct Sample
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("id")]
        public string ID;
    }

    public struct Players
    {
        [JsonProperty("max")]
        public int Max;

        [JsonProperty("online")]
        public int Online;

        [JsonProperty("sample")]
        public List<Sample> Sample;
    }

    public struct ModList
    {
        [JsonProperty("modid")]
        public string ModID;

        [JsonProperty("version")]
        public string Version;
    }

    public struct ModInfo
    {
        [JsonProperty("type")]
        public string Type;

        [JsonProperty("modList")]
        public List<ModList> ModList;
    }


    public struct ServerInfo
    {
        [JsonProperty("version")]
        public Version Version;

        [JsonProperty("players")]
        public Players Players;

        [JsonProperty("description")]
        public string Description;

        [JsonProperty("favicon")]
        public byte[] Favicon;

        [JsonProperty("modinfo")]
        public ModInfo? ModInfo;
    }
}
