using System.Collections.Generic;
using Newtonsoft.Json;

namespace MineLib.Network
{
    public partial class Yggdrasil
    {
        public enum ErrorType
        {
            ForbiddenOperationException,
            IllegalArgumentException,
            UnsupportedMediaType
        }

        #region Response

        public struct AvailableProfiles
        {
            [JsonProperty("id")]
            public string ID { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("legacy")]
            public bool Legacy { get; set; }
        }

        public struct SelectedProfile
        {
            [JsonProperty("id")]
            public string ID { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("legacy")]
            public bool Legacy { get; set; }
        }

        public struct Response
        {
            [JsonProperty("accessToken")]
            public string AccessToken { get; set; }

            [JsonProperty("clientToken")]
            public string ClientToken { get; set; }

            [JsonProperty("selectedProfile")]
            public SelectedProfile Profile { get; set; }

            [JsonProperty("availableProfiles")]
            public List<AvailableProfiles> AvailableProfiles { get; set; }
        }

        public struct YggdrasilAnswer
        {
            public YggdrasilStatus Status;

            public Response Response;
        }

        public struct Error
        {
            [JsonProperty("error")]
            public ErrorType ErrorDescription { get; set; }

            [JsonProperty("errorMessage")]
            public string ErrorMessage { get; set; }

            [JsonProperty("cause")]
            public string Cause { get; set; }
        }

        #endregion Response

        #region Requests
        
        private struct Agent
        {
            public static Agent Minecraft
            {
                get { return new Agent { Name = "Minecraft", Version = 1 }; }
            }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("version")]
            public int Version { get; set; }
        }

        private struct JsonLogin
        {
            [JsonProperty("agent")]
            public Agent Agent { get; set; }

            [JsonProperty("username")]
            public string Username { get; set; }

            [JsonProperty("password")]
            public string Password { get; set; }
        }

        private struct JsonRefreshSession
        {
            [JsonProperty("accessToken")]
            public string AccessToken { get; set; }

            [JsonProperty("clientToken")]
            public string ClientToken { get; set; }
        }

        private struct JsonVerifySession
        {
            [JsonProperty("accessToken")]
            public string AccessToken { get; set; }
        }

        private struct JsonLogout
        {
            [JsonProperty("username")]
            public string Username { get; set; }

            [JsonProperty("password")]
            public string Password { get; set; }
        }

        private struct JsonInvalidate
        {
            [JsonProperty("accessToken")]
            public string AccessToken { get; set; }

            [JsonProperty("clientToken")]
            public string ClientToken { get; set; }
        }

        private struct JsonClientAuth
        {
            [JsonProperty("accessToken")]
            public string AccessToken { get; set; }

            [JsonProperty("selectedProfile")]
            public string SelectedProfile { get; set; }

            [JsonProperty("serverId")]
            public string ServerID { get; set; }
        }

        #endregion Requests
    }
}
