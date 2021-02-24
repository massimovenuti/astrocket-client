using System;

namespace API.Auth
{
    [Serializable]
    public class UserLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
