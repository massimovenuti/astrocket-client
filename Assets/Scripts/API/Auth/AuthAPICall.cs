using UnityEngine;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Text;
using System;

namespace API.Auth
{
    public class AuthAPICall
    {
        private readonly Uri AUTH_API_URL = new Uri(@"https://auth.aw.alexandre-vogel.fr");
        private readonly Dictionary<string, string> _endpoints;
        private readonly HttpClient _httpClient = new HttpClient();

        public AuthAPICall( )
        {
            // Default URL for Auth API call
            _httpClient.BaseAddress = AUTH_API_URL;

            // Allow for HTTPS communication
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                | SecurityProtocolType.Tls11
                | SecurityProtocolType.Tls;

            // API endpoints
            _endpoints = new Dictionary<string, string>()
            {
                ["loginUser"] = "/user/login",
                ["addUser"] = "/user/add",
                ["checkUserToken"] = "/user/check",
                ["checkServerToken"] = "/server/check",
                ["banUser"] = "/user/ban",
                ["removeUser"] = "/user/remove",
                ["addServer"] = "/server/add",
                ["removeServer"] = "/server/remove",
                ["addAdmin"] = "/user/admin",
                ["removeAdmin"] = "/user/unadmin",
                ["unbanUser"] = "/user/unban",
            };
        }

        /// <summary>
        /// Post user login information and asks a new token
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>null if the user doesn't exist or credentials are invalid</returns>
        public UserToken PostLoginUser(string username, string password)
            => PostLoginUser(new UserLogin() { Name = username, Password = password });
        public UserToken PostLoginUser(UserLogin ur)
        {
            string res = DoApiCall(JsonUtility.ToJson(ur), _endpoints["logUser"], RequestType.Post);
            if (res == null)
                return new UserToken() { Token = null };
            else
            {
                UserToken token = (UserToken)JsonUtility.FromJson(res, typeof(UserToken));
                if (token.Token == default)
                    return null;
                else
                    return token;
            }
        }


        public UserToken PostAddUser(string username, string password, string email)
            => PostAddUser(new UserRegister() { Name = username, Password = password, Email = email });
        public UserToken PostAddUser(UserRegister ur)
        {
            string res = DoApiCall(JsonUtility.ToJson(ur), _endpoints["regUser"], RequestType.Post);
            if (res == null)
                return new UserToken() { Token = null };
            else
            {
                UserToken token = (UserToken)JsonUtility.FromJson(res, typeof(UserToken));
                if (token.Token == default)
                    return null;
                else
                    return token;
            }
        }

        public UserToken PostCheckUserToken(string token)
            => PostCheckUserToken(new UserToken() { Token = token });
        public UserToken PostCheckUserToken(UserToken ut)
        {
            string res = DoApiCall(JsonUtility.ToJson(ut), _endpoints["checkUserToken"], RequestType.Post);
            if (res == null)
                return new UserToken() { Token = null };
            else
            {
                UserToken token = (UserToken)JsonUtility.FromJson(res, typeof(UserToken));
                if (token.Token == default)
                    return null;
                else
                    return token;
            }
        }

        public UserToken PostCheckServerToken(string token)
            => PostCheckServerToken(new UserToken() { Token = token });
        public UserToken PostCheckServerToken(UserToken ut)
        {
            string res = DoApiCall(JsonUtility.ToJson(ut), _endpoints["checkServerToken"], RequestType.Post);
            if (res == null)
                return new UserToken() { Token = null };
            else
            {
                UserToken token = (UserToken)JsonUtility.FromJson(res, typeof(UserToken));
                if (token.Token == default)
                    return null;
                else
                    return token;
            }
        }

        public bool PostAddAdmin(string username, string password, string email)
            => PostAddAdmin(new UserRegister() { Name = username, Password = password, Email = email });
        public bool PostAddAdmin(UserRegister ur)
        {
            string res = DoApiCall(JsonUtility.ToJson(ur), _endpoints["regUser"], RequestType.Post);
            if (res == null)
                return false;
            else
            {
                UserToken token = (UserToken)JsonUtility.FromJson(res, typeof(UserToken));
                if (token.Token == default)
                    return false;
                else
                    return true;
            }
        }

        public void PostBanUser(string username, string adminToken)
            => PostBanUser(new BanItem() { Name = username, Token = adminToken });
        public void PostBanUser(BanItem bi)
        {
            string res = DoApiCall(JsonUtility.ToJson(bi), _endpoints["userTokCheck"], RequestType.Post);
            if (res == null)
                Debug.LogWarning($"Invalid username {bi.Name} or admin token {bi.Token}");
            else
                Debug.Log($"The user {bi.Name} was banned successfully");
        }

        public void PostRemoveUser(string username, string adminToken)
            => PostRemoveUser(new BanItem() { Name = username, Token = adminToken });
        public void PostRemoveUser(BanItem bi)
        {
            string res = DoApiCall(JsonUtility.ToJson(bi), _endpoints["userTokCheck"], RequestType.Post);
            if (res == null)
                Debug.LogWarning($"Invalid username {bi.Name} or admin token {bi.Token}");
            else
                Debug.Log($"The user {bi.Name} was removed successfully");
        }



        public ServerToken PostAddServerToList(string name, string user_token)
            => PostAddServerToList(new ServerInfo() { Token = user_token, Name = name});
        public ServerToken PostAddServerToList(ServerInfo ut)
        {
            string res = DoApiCall(JsonUtility.ToJson(ut), _endpoints["srvTokCheck"], RequestType.Post);
            if (res == null)
                return new ServerToken() { Token = null };
            else
            {
                ServerToken token = (ServerToken)JsonUtility.FromJson(res, typeof(ServerToken));
                if (token.Token == default)
                    return null;
                else
                    return token;
            }
        }

        public UserToken PostRemoveServerFromList(string token)
            => PostRemoveServerFromList(new UserToken() { Token = token });
        public UserToken PostRemoveServerFromList(UserToken ut)
        {
            string res = DoApiCall(JsonUtility.ToJson(ut), _endpoints["srvTokCheck"], RequestType.Post);
            if (res == null)
                return new UserToken() { Token = null };
            else
            {
                UserToken token = (UserToken)JsonUtility.FromJson(res, typeof(UserToken));
                if (token.Token == default)
                    return null;
                else
                    return token;
            }
        }

        private string DoApiCall(string json, string called, RequestType type)
        {
            HttpResponseMessage responseMessage;
            if (type == RequestType.Get)
                responseMessage = _httpClient.GetAsync(called).Result;
            else
                responseMessage = _httpClient.PostAsync(called, new StringContent(json, Encoding.UTF8, "application/json")).Result;

            if (responseMessage.StatusCode != HttpStatusCode.OK)
                return null;
            else
                return responseMessage.Content.ReadAsStringAsync().Result;
        }
    }
}
