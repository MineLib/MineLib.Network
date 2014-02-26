using System;
using System.IO;
using System.Net;

namespace MineLib.Network
{
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

    public static class Yggdrasil
    {
        /// <summary>
        /// Allows to login to a premium Minecraft account using the Yggdrasil authentication scheme.
        /// </summary>
        /// <param name="username">Login</param>
        /// <param name="password">Password</param>
        /// <param name="accessToken">Will contain the access token returned by Minecraft.net, if the login is successful</param>
        /// <param name="clientToken"></param>
        /// <param name="uuid">Will contain the player's UUID, needed for multiplayer</param>
        /// <returns>Returns the status of the login (Success, Failure, etc.)</returns>
        public static YggdrasilStatus LoginAuthServer(ref string username, string password, ref string accessToken,
            ref string clientToken, ref string uuid)
        {
            try
            {
                WebClient wClient = new WebClient();
                wClient.Headers.Add("Content-Type: application/json");
                string json_request = "{\"agent\": { \"name\": \"Minecraft\", \"version\": 1 }, \"username\": \"" +
                                      username + "\", \"password\": \"" + password + "\" }";
                string result = wClient.UploadString("https://authserver.mojang.com/authenticate", json_request);

                if (result.Contains("availableProfiles\":[]}"))
                {
                    return YggdrasilStatus.NotPremium;
                }

                {
                    string[] temp = result.Split(new string[] { "accessToken\":\"" }, StringSplitOptions.RemoveEmptyEntries);
                    if (temp.Length >= 2) { accessToken = temp[1].Split('"')[0]; }
                    temp = result.Split(new string[] { "clientToken\":\"" }, StringSplitOptions.RemoveEmptyEntries);
                    if (temp.Length >= 2) { clientToken = temp[1].Split('"')[0]; }
                    temp = result.Split(new string[] { "name\":\"" }, StringSplitOptions.RemoveEmptyEntries);
                    if (temp.Length >= 2) { username = temp[1].Split('"')[0]; }
                    temp = result.Split(new string[] { "availableProfiles\":[{\"id\":\"" }, StringSplitOptions.RemoveEmptyEntries);
                    if (temp.Length >= 2) { uuid = temp[1].Split('"')[0]; }
                }

                return YggdrasilStatus.Success;
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse response = (HttpWebResponse)e.Response;
                    if ((int)response.StatusCode == 403)
                    {
                        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                        {
                            string result = sr.ReadToEnd();
                            if (result.Contains("UserMigratedException"))
                            {
                                return YggdrasilStatus.AccountMigrated;
                            }
                            return YggdrasilStatus.WrongPassword;
                        }
                    }
                    return YggdrasilStatus.Blocked;
                }
                return YggdrasilStatus.Error;
            }
        }

        public static YggdrasilStatus RefreshSession(ref string accesstoken, ref string clienttoken)
        {
            try
            {
                WebClient wClient = new WebClient();
                wClient.Headers.Add("Content-Type: application/json");
                string json = "{\"accessToken\": \"" + accesstoken + "\",\"clientToken\": \"" + clienttoken + "\"}";
                string result = wClient.UploadString("https://authserver.mojang.com/refresh", json);

                {
                    string[] temp = result.Split(new string[] { "accessToken\":\"" }, StringSplitOptions.RemoveEmptyEntries);
                    if (temp.Length >= 2) { accesstoken = temp[1].Split('"')[0]; }
                    temp = result.Split(new string[] { "clientToken\":\"" }, StringSplitOptions.RemoveEmptyEntries);
                    if (temp.Length >= 2) { clienttoken = temp[1].Split('"')[0]; }
                }

                return YggdrasilStatus.Success;
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse response = (HttpWebResponse)e.Response;
                    if ((int)response.StatusCode == 403)
                    {
                        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                        {
                            string result = sr.ReadToEnd();
                            if (result.Contains("ForbiddenOperationException"))
                            {
                                return YggdrasilStatus.InvalidToken;
                            }
                            return YggdrasilStatus.Error;
                        }
                    }
                    return YggdrasilStatus.Blocked;
                }
                return YggdrasilStatus.Error;
            }

        }

        public static bool VerifyName(string accessToken, string selectedProfile, string serverHash)
        {
            try
            {
                WebClient wClient = new WebClient();
                wClient.Headers.Add("Content-Type: application/json");
                string json = "{\"accessToken\": \"" + accessToken + "\",\"selectedProfile\": \"" + selectedProfile +
                              "\",\"serverId\": \"" + serverHash + "\"}";
                string result = wClient.UploadString("https://sessionserver.mojang.com/session/minecraft/join", json);

                {
                    // dunno what to do, can't find answer
                }

                return true;
            }
            catch (WebException) { return false; }
        }

        public static bool VerifySession(string accessToken)
        {
            try
            {
                WebClient wClient = new WebClient();
                wClient.Headers.Add("Content-Type: application/json");

                string json = "{\"accessToken\": \"" + accessToken + "\"}";
                string result = wClient.UploadString("https://authserver.mojang.com/validate", json);

                string[] temp = result.Split(new string[] { "accessToken\":\"" },
                    StringSplitOptions.RemoveEmptyEntries);
                if (temp.Length >= 2)
                    accessToken = temp[1].Split('"')[0];


                return true;
            }
            catch (WebException) { return false; }
        }

    }
}