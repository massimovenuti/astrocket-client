namespace API.Auth
{
    [System.Serializable]
    public class UserRegister : IName,IPassword, IEmail 
    {
        public string Name { get => username; set { username = value; } }
        [UnityEngine.SerializeField]
        private string username;
        public string Password { get => password; set { password = value; } }
        [UnityEngine.SerializeField]
        private string password;
        public string Email { get => email; set { email = value; } }
        [UnityEngine.SerializeField]
        private string email;
    }
}
