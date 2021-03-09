using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using UnityEngine;

namespace API.Stats
{
    // use [Serializable] to turn a class into structure JSON
    public class StatsAPICall
    {
        private readonly Uri AUTH_API_URL = new(@"https://auth.aw.alexandre-vogel.fr/");
        private readonly HttpClient _httpClient = new();

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

            if (responseMessage.StatusCode != HttpStatusCode.OK)
                return null;
            else
                return JsonUtility.FromJson<PlayerStats>(responseMessage.Content.ReadAsStringAsync().Result);
        }

        public bool PostModifyPlayerStats(IName name, IToken token)
            => PostModifyPlayerStats(name.Name, token.Token);
        public bool PostModifyPlayerStats(string name, string token)
        {
            using (HttpRequestMessage message = new(HttpMethod.Post, $"stats/{name}"))
            {
                message.Headers.Add("serverToken", token);
                HttpResponseMessage responseMessage = _httpClient.SendAsync(message).Result;
                if (responseMessage.IsSuccessStatusCode)
                    return true;
            }
            return false;
        }

        public bool DeleteUserStats(IName name, IToken token)
            => DeleteUserStats(name.Name, token.Token);
        public bool DeleteUserStats(string name, string token)
        {
            using (HttpRequestMessage message = new(HttpMethod.Delete, $"stats/{name}"))
            {
                message.Headers.Add("serverToken", token);
                HttpResponseMessage responseMessage = _httpClient.SendAsync(message).Result;
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

