namespace API.Auth
{
    [System.Serializable]
    public class ServerToken : IToken
    {
        public string Token { get => token; set { token = value; } }
        [UnityEngine.SerializeField]
        private string token;
    }
}
