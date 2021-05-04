namespace API
{
    [System.Serializable]
    class ServerListItem : IName, IPort, IIPAdress, IPlayerCount
    {
        public string Name { get => name; set => name = value; }
        private string name;
        public string IP { get => ip; set => ip = value; }
        private string ip;
        public short Port { get => port; set => port = value; }
        private short port;
        public int PlayerCount { get => playersNB; set => playersNB = value; }
        private int playersNB;
    }
}
