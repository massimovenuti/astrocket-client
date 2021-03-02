using UnityEngine;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Text;

namespace API.Auth
{
    public class AuthAPICall
    {
        private const string AUTH_API_URL = @"https://auth.aw.alexandre-vogel.fr";
        private readonly Dictionary<string, string> _urls;
        private readonly HttpClient _httpClient = new HttpClient();

        public AuthAPICall( )
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

        public Token PostUserLoginInfo(string username, string password)
            => PostUserLoginInfo(new UserLogin() { Username = username, Password = password });
        public Token PostUserLoginInfo(UserLogin ur)
        {
            string res = DoApiCall(JsonUtility.ToJson(ur), _urls["logUser"], RequestType.Post);
            if (res == null)
                return new Token() { Tok = null };
            else
            {
                Token token = (Token)JsonUtility.FromJson(res, typeof(Token));
                if (token.Tok == default)
                    return null;
                else
                    return token;
            }
        }

        public Token PostUserRegisterInfo(string username, string password, string email)
            => PostUserRegisterInfo(new UserRegister() { Username = username, Password = password, Email = email });
        public Token PostUserRegisterInfo(UserRegister ur)
        {
            string res = DoApiCall(JsonUtility.ToJson(ur), _urls["regUser"], RequestType.Post);
            if (res == null)
                return new Token() { Tok = null };
            else
            {
                Token token = (Token)JsonUtility.FromJson(res, typeof(Token));
                if (token.Tok == default)
                    return null;
                else
                    return token;
            }
        }

        public Token PostUserTokenVerification(string token)
            => PostUserTokenVerification(new Token() { Tok = token });
        public Token PostUserTokenVerification(Token ut)
        {
            string res = DoApiCall(JsonUtility.ToJson(ut), _urls["userTokCheck"], RequestType.Post);
            if (res == null)
                return new Token() { Tok = null };
            else
            {
                Token token = (Token)JsonUtility.FromJson(res, typeof(Token));
                if (token.Tok == default)
                    return null;
                else
                    return token;
            }
        }

        public void PostBanUser(string username, string adminToken)
            => PostBanUser(new BanItem() { Username = username, AdminToken = adminToken });
        public void PostBanUser(BanItem bi)
        {
            string res = DoApiCall(JsonUtility.ToJson(bi), _urls["userTokCheck"], RequestType.Post);
            if (res == null)
                Debug.LogWarning($"Invalid username {bi.Username} or admin token {bi.AdminToken}");
            else
                Debug.Log($"The user {bi.Username} was banned successfully");
        }

        public Token PostServerTokenVerification(string token)
            => PostServerTokenVerification(new Token() { Tok = token });
        public Token PostServerTokenVerification(Token ut)
        {
            string res = DoApiCall(JsonUtility.ToJson(ut), _urls["srvTokCheck"], RequestType.Post);
            if (res == null)
                return new Token() { Tok = null };
            else
            {
                Token token = (Token)JsonUtility.FromJson(res, typeof(Token));
                if (token.Tok == default)
                    return null;
                else
                    return token;
            }
        }

        private string DoApiCall(string json, string called, RequestType type)
        {
            HttpResponseMessage responseMessage;
            if (type == RequestType.Get)
                responseMessage = _httpClient.GetAsync(AUTH_API_URL + called).Result;
            else
                responseMessage = _httpClient.PostAsync(AUTH_API_URL + called, new StringContent(json, Encoding.UTF8, "application/json")).Result;

            if (responseMessage.StatusCode != HttpStatusCode.OK)
                return null;
            else
                return responseMessage.Content.ReadAsStringAsync().Result;
        }

        public Token TestAddUser()
        {
            string username = "testusername";
            string password = "testpassword";
            string email = @"test.email@example.com";

            return PostUserRegisterInfo(username, password, email);
        }
    }
}
