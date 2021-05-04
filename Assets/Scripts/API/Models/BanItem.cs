namespace API
{
    [System.Serializable]
    public class BanItem : IToken, IName
    {
        public string Name { get => username; set { username = value; } }
        [UnityEngine.SerializeField]
        private string username;

        public string Token { get => token; set { token = value; } }
        [UnityEngine.SerializeField]
        private string token;
    }
}
