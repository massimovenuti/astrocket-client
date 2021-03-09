namespace API
{
    public class TokenName
    {
        public string Token { get => token; set { token = value; } }
        [UnityEngine.SerializeField]
        private string token;
    }
}
