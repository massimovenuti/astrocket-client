namespace API
{
    [System.Serializable]
    class ServerListItem : IName, IPort, IIPAdress, IPlayerCount
    {
        public string Name { get => name; set => name = value; }
        [UnityEngine.SerializeField]
        private string name;
        public string IP { get => address; set => address = value; }
        [UnityEngine.SerializeField]
        private string address;
        public short Port { get => port; set => port = value; }
        [UnityEngine.SerializeField]
        private short port;
        public int PlayerCount { get => playersNB; set => playersNB = value; }
        [UnityEngine.SerializeField]
        private int playersNB;
    }
}
