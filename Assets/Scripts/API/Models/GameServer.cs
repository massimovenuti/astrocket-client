namespace API
{
    [System.Serializable]
    class GameServer : IName, IIPAdress, IPort
    {
        public string Name { get => name; set => name = value; }
        private string name;
        public string IP { get => ip; set => ip = value; }
        private string ip;
        public short Port { get => port; set => port = value; }
        private short port;
    }

}
