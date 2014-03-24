using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace MineLib.Network
{
    public enum YggdrasilStatus
    {
        Error,
        Success,
        WrongPassword,
        Blocked,
        AccountMigrated,
        InvalidToken,
        NotFound,
        UnsupportedMediaType
    }

    public static partial class Yggdrasil
    {
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

                string json =
                    JsonConvert.SerializeObject(new JsonLogin
                    {
                        Agent = Agent.Minecraft,
                        Username = username,
                        Password = password
                    });

   
                var response = JsonConvert.DeserializeObject<Response>(
                    wClient.UploadString("https://authserver.mojang.com/authenticate", json));

                return new YggdrasilAnswer {Status = YggdrasilStatus.Success, Response = response};
            }
            catch (WebException e)
            {
                return HandleWebException(e);
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

                var json =
                    JsonConvert.SerializeObject(new JsonRefreshSession
                    {
                        AccessToken = accessToken,
                        ClientToken = clientToken,
                    });

                var response = JsonConvert.DeserializeObject<Response>(
                    wClient.UploadString("https://authserver.mojang.com/refresh", json));

                return new YggdrasilAnswer { Status = YggdrasilStatus.Success, Response =  response };
            }
            catch (WebException e)
            {
                return HandleWebException(e);
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

                var json = JsonConvert.SerializeObject(new JsonVerifySession {AccessToken = accessToken});

                var response = wClient.UploadString("https://authserver.mojang.com/validate", json);

                return string.IsNullOrEmpty(response);
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

                var json =
                    JsonConvert.SerializeObject(new JsonLogout
                    {
                        Username = username,
                        Password = password
                    });

                var response =  wClient.UploadString("https://authserver.mojang.com/signout", json);

                return string.IsNullOrEmpty(response);
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

                var json =
                    JsonConvert.SerializeObject(new JsonInvalidate
                    {
                        AccessToken = accessToken,
                        ClientToken = clientToken
                    });

                var response = wClient.UploadString("https://authserver.mojang.com/signout", json);

                return string.IsNullOrEmpty(response);
            }
            catch (WebException)
            {
                return false;
            }
        }

        /// <summary>
        ///     Both server and client need to make a request to sessionserver.mojang.com if the server is in online-mode.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="selectedProfile"></param>
        /// <param name="serverHash"></param>
        /// <returns></returns>
        public static bool ClientAuth(string accessToken, string selectedProfile, string serverHash)
        {
            try
            {
                var wClient = new WebClient();
                wClient.Headers.Add("Content-Type: application/json");

                var json =
                    JsonConvert.SerializeObject(new JsonClientAuth
                    {
                        AccessToken = accessToken,
                        SelectedProfile = selectedProfile,
                        ServerID = serverHash
                    });

                var response = wClient.UploadString("https://sessionserver.mojang.com/session/minecraft/join", json);

                return string.IsNullOrEmpty(response);
            }
            catch (WebException)
            {
                return false;
            }
        }

        private static YggdrasilAnswer HandleWebException(WebException e)
        {
            if (e.Status != WebExceptionStatus.ProtocolError)
                return new YggdrasilAnswer { Status = YggdrasilStatus.Error };

            using (var response = (HttpWebResponse)e.Response)
            {
                if (response.StatusCode != HttpStatusCode.Forbidden)
                    return new YggdrasilAnswer { Status = YggdrasilStatus.Blocked };

                if (response.StatusCode == HttpStatusCode.UnsupportedMediaType)
                    return new YggdrasilAnswer { Status = YggdrasilStatus.UnsupportedMediaType };

                if (response.StatusCode == HttpStatusCode.NotFound)
                    return new YggdrasilAnswer { Status = YggdrasilStatus.NotFound };

                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    string result = sr.ReadToEnd();

                    if (!response.ContentType.Contains("application/json"))
                        return new YggdrasilAnswer {Status = YggdrasilStatus.Error};

                    var error = JsonConvert.DeserializeObject<Error>(result);

                    if (error.ErrorDescription != ErrorType.ForbiddenOperationException)
                        return new YggdrasilAnswer {Status = YggdrasilStatus.Error};

                    if (error.Cause != null && error.Cause.Contains("UserMigratedException"))
                        return new YggdrasilAnswer {Status = YggdrasilStatus.AccountMigrated};

                    if (error.ErrorMessage.Contains("Invalid token"))
                        return new YggdrasilAnswer { Status = YggdrasilStatus.InvalidToken };

                    return new YggdrasilAnswer {Status = YggdrasilStatus.WrongPassword};
                }

            }

        }

    }
}