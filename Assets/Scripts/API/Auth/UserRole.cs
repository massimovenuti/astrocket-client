namespace API.Auth
{
    public class UserRole : IRole, IName
    {
        public string Name { get => name; set { name = value; } }
        [UnityEngine.SerializeField]
        private string name;
        public int Role { get => role; set { role = value; } }
        [UnityEngine.SerializeField]
        private int role;
    }
}
