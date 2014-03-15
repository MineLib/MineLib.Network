using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace MineLib.Network
{
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

        [JsonProperty("availableProfiles")]
        public AvailableProfiles[] AvailableProfiles { get; set; }

        [JsonProperty("selectedProfile")]
        public SelectedProfile Profile { get; set; }
    }

    public enum YggdrasilStatus
    {
        Error,
        Success,
        WrongPassword,
        Blocked,
        AccountMigrated,
        NotPremium,
        InvalidToken
    }

    public struct YggdrasilAnswer
    {
        public YggdrasilStatus Status;

        public Response Response;
    }

    public static class Yggdrasil
    {
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

        private struct JsonVerifyName
        {
            [JsonProperty("accessToken")]
            public string AccessToken { get; set; }

            [JsonProperty("selectedProfile")]
            public string SelectedProfile { get; set; }

            [JsonProperty("serverId")]
            public string ServerID { get; set; }
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

        /// <summary>
        ///     Authenticates a user using his password.
        /// </summary>
        /// <param name="username">Login</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        public static YggdrasilAnswer Login(string username, string password)
        {
            try
            {
                var wClient = new WebClient();
                wClient.Headers.Add("Content-Type: application/json");

                string json_request =
                    JsonConvert.SerializeObject(new JsonLogin
                    {
                        Agent = Agent.Minecraft,
                        Username = username,
                        Password = password
                    });

                var response = JsonConvert.DeserializeObject<Response>(
                        wClient.UploadString("https://authserver.mojang.com/authenticate", json_request));

                return new YggdrasilAnswer {Status = YggdrasilStatus.Success, Response = response};
            }
            catch (WebException e)
            {
                if (e.Status != WebExceptionStatus.ProtocolError)
                    return new YggdrasilAnswer {Status = YggdrasilStatus.Error};

                var response = (HttpWebResponse) e.Response;
                if ((int) response.StatusCode != 403)
                    return new YggdrasilAnswer {Status = YggdrasilStatus.Blocked};

                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    string result = sr.ReadToEnd();
                    return result.Contains("UserMigratedException")
                        ? new YggdrasilAnswer {Status = YggdrasilStatus.AccountMigrated}
                        : new YggdrasilAnswer {Status = YggdrasilStatus.WrongPassword};
                }
            }
        }

        /// <summary>
        ///     Refreshes a valid accessToken.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="clientToken"></param>
        /// <returns></returns>
        public static YggdrasilAnswer RefreshSession(string accessToken, string clientToken)
        {
            try
            {
                var wClient = new WebClient();
                wClient.Headers.Add("Content-Type: application/json");

                var json_request =
                    JsonConvert.SerializeObject(new JsonRefreshSession
                    {
                        AccessToken = accessToken,
                        ClientToken = clientToken,
                    });

                var response = JsonConvert.DeserializeObject<Response>(
                    wClient.UploadString("https://authserver.mojang.com/refresh", json_request));

                return new YggdrasilAnswer { Status = YggdrasilStatus.Success, Response =  response };
            }
            catch (WebException e)
            {
                if (e.Status != WebExceptionStatus.ProtocolError)
                    return new YggdrasilAnswer { Status = YggdrasilStatus.Error };

                var response = (HttpWebResponse) e.Response;
                if ((int) response.StatusCode != 403)
                    return new YggdrasilAnswer { Status = YggdrasilStatus.Blocked };

                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    string result = sr.ReadToEnd();
                    return result.Contains("ForbiddenOperationException")
                        ? new YggdrasilAnswer {Status = YggdrasilStatus.InvalidToken}
                        : new YggdrasilAnswer {Status = YggdrasilStatus.Error};
                }
            }
        }

        /// <summary>
        ///     Allows to login to a premium Minecraft account using the Yggdrasil authentication scheme.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="selectedProfile"></param>
        /// <param name="serverHash"></param>
        /// <returns></returns>
        public static bool VerifyName(string accessToken, string selectedProfile, string serverHash)
        {
            try
            {
                var wClient = new WebClient();
                wClient.Headers.Add("Content-Type: application/json");

                var json_request =
                    JsonConvert.SerializeObject(new JsonVerifyName
                    {
                        AccessToken = accessToken,
                        SelectedProfile = selectedProfile,
                        ServerID = serverHash
                    });

                var response = JsonConvert.DeserializeObject<Response>(
                    wClient.UploadString("https://sessionserver.mojang.com/session/minecraft/join", json_request));

                return true;
            }
            catch (WebException)
            {
                return false;
            }
        }

        /// <summary>
        ///     Checks if an accessToken is a valid session token with a currently-active session.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static bool VerifySession(string accessToken)
        {
            try
            {
                var wClient = new WebClient();
                wClient.Headers.Add("Content-Type: application/json");

                var json_request =
                    JsonConvert.SerializeObject(new JsonVerifySession {AccessToken = accessToken});

                var response = JsonConvert.DeserializeObject<Response>(
                    wClient.UploadString("https://authserver.mojang.com/validate", json_request));

                return true;
            }
            catch (WebException)
            {
                return false;
            }
        }

        /// <summary>
        ///     Invalidates accessTokens using an account's username and password
        /// </summary>
        /// <param name="username">Login</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        public static bool Logout(string username, string password)
        {
            try
            {
                var wClient = new WebClient();
                wClient.Headers.Add("Content-Type: application/json");

                var json_request =
                    JsonConvert.SerializeObject(new JsonLogout
                    {
                        Username = username,
                        Password = password
                    });

                var response = JsonConvert.DeserializeObject<Response>(
                    wClient.UploadString("https://authserver.mojang.com/signout", json_request));

                return true;
            }
            catch (WebException)
            {
                return false;
            }
        }

        /// <summary>
        ///     Invalidates accessTokens using a client/access token pair.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="clientToken"></param>
        /// <returns></returns>
        public static bool Invalidate(string accessToken, string clientToken)
        {
            try
            {
                var wClient = new WebClient();
                wClient.Headers.Add("Content-Type: application/json");

                var json_request =
                    JsonConvert.SerializeObject(new JsonInvalidate
                    {
                        AccessToken = accessToken,
                        ClientToken = clientToken
                    });

                var response = JsonConvert.DeserializeObject<Response>(
                    wClient.UploadString("https://authserver.mojang.com/signout", json_request));

                return true;
            }
            catch (WebException)
            {
                return false;
            }
        }
    }
}