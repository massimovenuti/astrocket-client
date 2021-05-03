namespace API.Auth
{
    [System.Serializable]
    public class UserRole : IRole, IName
    {
        public string Name { get => username; set { username = value; } }
        [UnityEngine.SerializeField]
        private string username;
        public int Role { get => role; set { role = value; } }
        [UnityEngine.SerializeField]
        private int role;
    }
}
