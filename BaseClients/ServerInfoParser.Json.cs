using System;
using Newtonsoft.Json;

namespace MineLib.Network.BaseClients
{
    public class Base64Converter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(byte[]);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // There is a dilemma: throw an exception and stop the app if data was corrupted or use it and decode it anyway.
            var text = (string) reader.Value;// + "=";

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
        public Sample[] Sample;
    }

    public struct ServerVersion
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("protocol")]
        public int Protocol;
    }

    public struct ServerInfo
    {
        [JsonProperty("description")]
        public string Description;

        [JsonProperty("players")]
        public Players Players;

        [JsonProperty("version")]
        public ServerVersion Version;

        [JsonProperty("favicon")]
        public byte[] Favicon;
    }

}
