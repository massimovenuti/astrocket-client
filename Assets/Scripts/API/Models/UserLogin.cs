namespace API
{
    [System.Serializable]
    public class UserLogin : IName, IPassword
    {
        public string Name { get => username; set { username = value; } }
        [UnityEngine.SerializeField]
        private string username;
        public string Password { get => password; set { password = value; } }
        [UnityEngine.SerializeField]
        private string password;
    }
}
