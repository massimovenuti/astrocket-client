namespace API.Auth
{
    [System.Serializable]
    public class UserToken : IToken, IName
    {
        public string Token { get => token; set { token = value; } }
        [UnityEngine.SerializeField]
        private string token;
        public string Name { get => username; set { username = value; } }
        [UnityEngine.SerializeField]
        private string username;
    }
}
