using System;

namespace API.Auth
{
    [Serializable]
    public class BanItem
    {
        public string AdminToken { get; set; }
        public string Username { get; set; }
    }
}
