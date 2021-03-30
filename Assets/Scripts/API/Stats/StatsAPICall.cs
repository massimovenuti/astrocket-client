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
        private readonly string STATS_API_URL = @"stats.aw.alexandre-vogel.fr";
        private readonly HttpClient _httpClient;
        private ErrorMessage _message;

        public ErrorMessage ErrorMessage { get => _message; private set { _message = value; } }

        public StatsAPICall( )
        {
            // Default URL for Auth API call
            UriBuilder uri = new UriBuilder("https", STATS_API_URL, 3000);
            _httpClient = new HttpClient()
            {
                BaseAddress = uri.Uri
            };

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

            Debug.Log(responseMessage.Content.ReadAsStringAsync().Result);

            if (responseMessage.StatusCode != HttpStatusCode.OK)
                return null;
            else
            {
                PlayerStats[] ps = JsonHelper.FromJson<PlayerStats>(JsonHelper.fixJson(responseMessage.Content.ReadAsStringAsync().Result));
                return ps.ToList();
            }
        }

        public PlayerStats GetUserStats(IName name)
            => GetUserStats(name.Name);
        public PlayerStats GetUserStats(string name)
        {
            HttpResponseMessage responseMessage = _httpClient.GetAsync($"stats/{name}").Result;
            ErrorMessage = new ErrorMessage(APICallFunction.FetchStats, responseMessage.StatusCode);
            Debug.Log(responseMessage.Content.ReadAsStringAsync().Result);
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
            => GetRannking(OrderByData.nbPoints);
        public List<PlayerStats> GetRannking(OrderByData data, long offset = 1, int limit = 10)
        {
            HttpResponseMessage responseMessage;
            Debug.Log(_httpClient.BaseAddress + $"stats/ranking{(data != OrderByData.maxPoints ? "/" + data.ToString() : "")}?offset={offset}&limit={limit}");
            responseMessage = _httpClient.GetAsync($"stats{(data != OrderByData.maxPoints ? "/" + data.ToString() : "")}?offset={offset}&limit={limit}").Result;
            ErrorMessage = new ErrorMessage(APICallFunction.FetchStats, responseMessage.StatusCode);
            Debug.Log(responseMessage.StatusCode);
            if (responseMessage.StatusCode != HttpStatusCode.OK)
                return null;
            else
            {
                PlayerStats[] ps = JsonHelper.FromJson<PlayerStats>(JsonHelper.fixJson(responseMessage.Content.ReadAsStringAsync().Result));
                return ps.ToList();
            }
        }
    }
}

