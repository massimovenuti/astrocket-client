using System;

namespace API.Auth
{
    [Serializable]
    public class UserRegister : UserLogin
    {
        public string Email { get; set; }
    }
}
