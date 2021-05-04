namespace API
{
    [System.Serializable]
    class SeverNameAndPlayerCount : IName, IPlayerCount
    {
        public string Name { get => name; set => name = value; }
        public string name;
        public int PlayerCount { get => playersNB; set => playersNB = value; }
        private int playersNB;
    }
}
