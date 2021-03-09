namespace API.Auth
{
    [System.Serializable]
    public class ServerInfo : IToken, IName
    {
        public string Name { get => name; set { name = value; } }
        [UnityEngine.SerializeField]
        private string name;

        public string Token { get => user_token; set { user_token = value; } }
        [UnityEngine.SerializeField]
        private string user_token;
    }
}
