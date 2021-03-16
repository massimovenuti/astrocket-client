using System;
using System.Net;
using UnityEngine;
using System.Linq;
using System.Net.Http;
using System.Collections.Generic;

namespace API.Stats
{
    public class StatsAPICall
    {
        private readonly Uri AUTH_API_URL = new Uri(@"https://auth.aw.alexandre-vogel.fr/");
        private readonly HttpClient _httpClient = new HttpClient();
        private ErrorMessage _errorMessage;

        public ErrorMessage ErrorMessage { get => _errorMessage; private set { _errorMessage = value; }}

        public StatsAPICall( )
        {
            // Default URL for Auth API call
            _httpClient.BaseAddress = AUTH_API_URL;

            // Allow for HTTPS communication
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                | SecurityProtocolType.Tls11
                | SecurityProtocolType.Tls;
        }

        public List<PlayerStats> GetAllStats()
        {
            HttpResponseMessage responseMessage;
            responseMessage = _httpClient.GetAsync("stats/").Result;
            ErrorMessage = new ErrorMessage(APICallFunction.FetchStats, responseMessage.StatusCode);

            if (responseMessage.StatusCode != HttpStatusCode.OK)
                return null;
            else
            {
                PlayerStats[] ps = JsonHelper.FromJson<PlayerStats>(responseMessage.Content.ReadAsStringAsync().Result);
                return ps.ToList();
            }
        }

        public PlayerStats GetUserStats(IName name)
            => GetUserStats(name.Name);
        public PlayerStats GetUserStats(string name)
        {
            HttpResponseMessage responseMessage = _httpClient.GetAsync($"stats/{name}").Result;
            ErrorMessage = new ErrorMessage(APICallFunction.FetchStats, responseMessage.StatusCode);
            if (responseMessage.StatusCode != HttpStatusCode.OK)
                return null;
            else
                return JsonUtility.FromJson<PlayerStats>(responseMessage.Content.ReadAsStringAsync().Result);
        }

        public bool PostModifyPlayerStats(IName name, IToken token)
            => PostModifyPlayerStats(name.Name, token.Token);
        public bool PostModifyPlayerStats(string name, string token)
        {
            using (HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, $"stats/{name}"))
            {
                message.Headers.Add("serverToken", token);
                HttpResponseMessage responseMessage = _httpClient.SendAsync(message).Result;
                ErrorMessage = new ErrorMessage(APICallFunction.UpdateStats, responseMessage.StatusCode);
                if (responseMessage.IsSuccessStatusCode)
                    return true;
            }
            return false;
        }

        public bool DeleteUserStats(IName name, IToken token)
            => DeleteUserStats(name.Name, token.Token);
        public bool DeleteUserStats(string name, string token)
        {
            using (HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Delete, $"stats/{name}"))
            {
                message.Headers.Add("serverToken", token);  
                HttpResponseMessage responseMessage = _httpClient.SendAsync(message).Result;
                ErrorMessage = new ErrorMessage(APICallFunction.None, responseMessage.StatusCode);
                if (responseMessage.IsSuccessStatusCode)
                    return true;
            }
            return false;
        }

        public List<PlayerStats> GetRannkingByScore(long offset = 1, int limit = 10)
            => GetRannking(OrderByData.nbPoints, offset, limit);
        public List<PlayerStats> GetRannking(OrderByData data, long offset = 1, int limit = 10)
        {
            HttpResponseMessage responseMessage;
            responseMessage = _httpClient.GetAsync($"stats{(data != OrderByData.maxPoints ? "/" + data.ToString() : "")}?offset={offset}&limit={limit}").Result;
            ErrorMessage = new ErrorMessage(APICallFunction.FetchStats, responseMessage.StatusCode);
            if (responseMessage.StatusCode != HttpStatusCode.OK)
                return null;
            else
            {
                PlayerStats[] ps = JsonHelper.FromJson<PlayerStats>(responseMessage.Content.ReadAsStringAsync().Result);
                return ps.ToList();
            }
        }
    }
}

