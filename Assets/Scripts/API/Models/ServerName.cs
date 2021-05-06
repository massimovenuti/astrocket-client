namespace API
{
    [System.Serializable]
    public class ServerName : IName
    {
        public string Name { get => name; set { name = value; } }
        [UnityEngine.SerializeField]
        private string name;
    }
}
