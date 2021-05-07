using System;
using System.Net;
using System.Net.Http;
using UnityEngine;


namespace API.MainServer
{
    class MainServerAPI
    {
        private readonly string MAIN_SERVER_API_URL = @"main.aw.alexandre-vogel.fr";
        private readonly HttpClient _httpClient;
        private ErrorMessage _message;

        public ErrorMessage ErrorMessage { get => _message; private set { _message = value; } }

        public MainServerAPI( )
        {
            // Default URL for Auth API call
            UriBuilder uri = new UriBuilder("https", MAIN_SERVER_API_URL);
            _httpClient = new HttpClient()
            {
                BaseAddress = uri.Uri
            };

            // Allow for HTTPS communication
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                | SecurityProtocolType.Tls11
                | SecurityProtocolType.Tls;
        }

        public bool PostAddNewGameServer(UserToken token, GameServer gs)
        {
            using (HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, $"GameServer"))
            {
                string s = JsonUtility.ToJson(gs);
                message.Headers.Add("user_token", token.Token);
                message.Content = new StringContent(s, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = _httpClient.SendAsync(message).Result;
                ErrorMessage = new ErrorMessage(APICallFunction.AddServer, responseMessage.StatusCode);
                if (responseMessage.IsSuccessStatusCode)
                    return true;
            }
            return false;
        }

        public bool DeleteServer(UserToken userToken, ServerName serverName)
        {
            using (HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Delete, $"GameServer"))
            {
                string s = JsonUtility.ToJson(serverName);
                message.Headers.Add("user_token", userToken.Token);
                message.Content = new StringContent(s, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = _httpClient.SendAsync(message).Result;
                ErrorMessage = new ErrorMessage(APICallFunction.DeleteServer, responseMessage.StatusCode);
                if (responseMessage.IsSuccessStatusCode)
                    return true;
            }
            return false;
        }

        public bool PutPlayerCount(ServerToken serverToken, SeverNameAndPlayerCount snpc)
        {
            using (HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Put, $"GameServer"))
            {
                string s = JsonUtility.ToJson(snpc);
                message.Headers.Add("server_token", serverToken.Token);
                message.Content = new StringContent(s, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = _httpClient.SendAsync(message).Result;
                ErrorMessage = new ErrorMessage(APICallFunction.UpdatePlayerCount, responseMessage.StatusCode);
                if (responseMessage.IsSuccessStatusCode)
                    return true;
            }
            return false;
        }

        public bool PostIsServerAlive(GameServerToken[] serverToken)
        {
            using (HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, $"alive"))
            {
                // Si deserialization direct ne marche pas 
                // string s = "[";
                // foreach (ServerToken tok in serverToken)
                //     s += JsonUtility.ToJson(tok) + ",";
                // if (s.EndsWith(","))
                //     s.Insert(s.Length - 1, " ");
                // s += "]";

                string s = JsonUtility.ToJson(serverToken);
                message.Content = new StringContent(s, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = _httpClient.SendAsync(message).Result;
                ErrorMessage = new ErrorMessage(APICallFunction.UpdatePlayerCount, responseMessage.StatusCode);
                if (responseMessage.IsSuccessStatusCode)
                    return true;
            }
            return false;
        }

        public ServerListItem[] GetServerList(UserToken token)
        {
            using (HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, $"list"))
            {
                message.Headers.Add("user_token", token.Token);
                HttpResponseMessage responseMessage = _httpClient.SendAsync(message).Result;
                ErrorMessage = new ErrorMessage(APICallFunction.None, responseMessage.StatusCode);
                if (responseMessage.IsSuccessStatusCode)
                {
                    return JsonHelper.FromJson<ServerListItem>(JsonHelper.fixJson(responseMessage.Content.ReadAsStringAsync().Result));
                }

            }
            return null;
        }
    }
}
