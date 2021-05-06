namespace API
{
    [System.Serializable]
    public class GameServerToken : IToken
    {
        public string Token { get => serverToken; set { serverToken = value; } }
        [UnityEngine.SerializeField]
        private string serverToken;
    }
}