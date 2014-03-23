using Newtonsoft.Json;

namespace MineLib.Network.BaseClients
{
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
        public string Favicon;
    }

}
