using System;
using UnityEngine;
using System.Collections.Generic;
using System.Net.Http;

namespace API.Auth
{
    public class AuthAPICall
    {
        private const string AUTH_API_URL = @"https://auth.aw.alexandre-vogel.fr";
        private readonly Dictionary<string, string> _urls;
        private HttpClient _httpClient = new HttpClient();
        
        public AuthAPICall()
        {
            _urls = new Dictionary<string, string>()
            {
                ["logUser"] = "/user/login",
                ["regUser"] = "/user/add",
                ["addServer"] = "/server/add",
                ["userTokCheck"] = "/user/check",
                ["srvTokCheck"] = "/server/check",
                ["banUser"] = "/user/remove",
                ["rmvServer"] = "/server/remove",
            };
        }

        public UserToken PostUserLoginInfo(string username, string password)
            => PostUserLoginInfo(new UserLogin { Password = password, Username = username });

        public UserToken PostUserLoginInfo(UserLogin ul)
        {
            using(StringContent sc = new StringContent(JsonUtility.ToJson(ul)))
            {
                HttpResponseMessage msg = _httpClient.PostAsync(AUTH_API_URL + _urls["logUser"], sc).Result;

                if(msg.IsSuccessStatusCode)
                {

                }
            }

            return null;
        }



    }
}
