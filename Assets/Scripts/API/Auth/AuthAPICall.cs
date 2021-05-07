using UnityEngine;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Text;
using System;
using API.Stats;

namespace API.Auth
{
    public class AuthAPICall
    {
        private readonly string AUTH_API_URL = @"auth.aw.alexandre-vogel.fr";
        private readonly Dictionary<string, string> _endpoints;
        private readonly HttpClient _httpClient;
        private ErrorMessage _message;

        public ErrorMessage ErrorMessage { get => _message; private set { _message = value; } }

        public AuthAPICall( )
        {
            // Default URL for Auth API call
            UriBuilder uri = new UriBuilder("https", AUTH_API_URL);
            _httpClient = new HttpClient()
            {
                BaseAddress = uri.Uri
            };

            // Allow for HTTPS communication
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                | SecurityProtocolType.Tls11
                | SecurityProtocolType.Tls;

            // API endpoints
            _endpoints = new Dictionary<string, string>()
            {
                ["loginUser"] = "user/login",
                ["addUser"] = "user/add",
                ["checkUserToken"] = "user/check",
                ["checkServerToken"] = "server/check",
                ["banUser"] = "user/ban",
                ["removeUser"] = "user/remove",
                ["addServer"] = "server/add",
                ["removeServer"] = "server/remove",
                ["addAdmin"] = "user/admin",
                ["removeAdmin"] = "user/unadmin",
                ["unbanUser"] = "user/unban",
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
            string res = DoApiCall(JsonUtility.ToJson(ur), _endpoints["loginUser"], RequestType.Post, APICallFunction.Login);
            if (res == null)
                return null;
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
            string res = DoApiCall(JsonUtility.ToJson(ur), _endpoints["addUser"], RequestType.Post, APICallFunction.Register);
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

        public UserRole PostCheckUserToken(string token)
            => PostCheckUserToken(new UserToken() { Token = token });
        public UserRole PostCheckUserToken(UserToken ut)
        {
            string res = DoApiCall(JsonUtility.ToJson(ut), _endpoints["checkUserToken"], RequestType.Post);
            if (res == null)
                return null;
            else
            {
                UserRole token = JsonUtility.FromJson<UserRole>(res);
                if (token.Name == default)
                    return null;
                else
                    return token;
            }
        }

        public ServerName PostCheckServerToken(string token)
            => PostCheckServerToken(new ServerToken() { Token = token });
        public ServerName PostCheckServerToken(ServerToken ut)
        {
            string res = DoApiCall(JsonUtility.ToJson(ut), _endpoints["checkServerToken"], RequestType.Post);
            if (res == null)
                return null;
            else
            {
                ServerName token = (ServerName)JsonUtility.FromJson(res, typeof(ServerName));
                if (token.Name == default)
                    return null;
                else
                    return token;
            }
        }

        public bool PostBanUser(string username, string adminToken)
            => PostBanUser(new BanItem() { Name = username, Token = adminToken });
        public bool PostBanUser(BanItem bi)
        {
            string res = DoApiCall(JsonUtility.ToJson(bi), _endpoints["banUser"], RequestType.Post);
            if (res == null)
                return false;
            else
                return true;
        }

        public bool PostRemoveUser(string username, string adminToken)
            => PostRemoveUser(new BanItem() { Name = username, Token = adminToken });
        public bool PostRemoveUser(BanItem bi)
        {
            string res = DoApiCall(JsonUtility.ToJson(bi), _endpoints["removeUser"], RequestType.Post);
            if (res == null)
                return false;
            else
                return true;
        }

        public ServerToken PostAddServer(string name, string user_token)
            => PostAddServer(new ServerInfo() { Token = user_token, Name = name });
        public ServerToken PostAddServer(ServerInfo ut)
        {
            string res = DoApiCall(JsonUtility.ToJson(ut), _endpoints["addServer"], RequestType.Post);
            if (res == null)
                return null;
            else
            {
                ServerToken token = (ServerToken)JsonUtility.FromJson(res, typeof(ServerToken));
                if (token.Token == default)
                    return null;
                else
                    return token;
            }
        }

        public bool PostRemoveServer(string token)
            => PostRemoveServer(new UserToken() { Token = token });
        public bool PostRemoveServer(UserToken ut)
        {
            string res = DoApiCall(JsonUtility.ToJson(ut), _endpoints["removeServer"], RequestType.Post);
            if (res == null)
                return false;
            else
                return true;
        }

        public bool PostAddAdmin(string username, string token)
            => PostAddAdmin(new UserToken() { Name = username, Token = token });
        public bool PostAddAdmin(UserToken ut)
        {
            string res = DoApiCall(JsonUtility.ToJson(ut), _endpoints["addAdmin"], RequestType.Post);
            if (res == null)
                return false;
            else
                return true;
        }

        public bool PostRemoveAdmin(string username, string token)
            => PostRemoveAdmin(new UserToken() { Name = username, Token = token });
        public bool PostRemoveAdmin(UserToken ut)
        {
            string res = DoApiCall(JsonUtility.ToJson(ut), _endpoints["removeAdmin"], RequestType.Post);
            if (res == null)
                return false;
            else
                return true;
        }

        public bool PostUnbanUser(string username, string adminToken)
            => PostUnbanUser(new BanItem() { Name = username, Token = adminToken });
        public bool PostUnbanUser(BanItem bi)
        {
            string res = DoApiCall(JsonUtility.ToJson(bi), _endpoints["banUser"], RequestType.Post);
            if (res == null)
                return false;
            else
                return true;
        }

        private string DoApiCall(string json, string endpoint, RequestType rtype, APICallFunction ftype = APICallFunction.None)
        {
            HttpResponseMessage responseMessage;
            Debug.Log(_httpClient.BaseAddress + endpoint);
            if (rtype == RequestType.Get)
                responseMessage = _httpClient.GetAsync(endpoint).Result;
            else
                responseMessage = _httpClient.PostAsync(endpoint, new StringContent(json, Encoding.UTF8, "application/json")).Result;
            ErrorMessage = new ErrorMessage(ftype, responseMessage.StatusCode);
            if (responseMessage.StatusCode != HttpStatusCode.OK)
                return null;
            else
                return responseMessage.Content.ReadAsStringAsync().Result;
        }
    }
}
