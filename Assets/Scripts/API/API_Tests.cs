using UnityEngine;
using API.Auth;
using API.Stats;

namespace API
{
    class API_Tests : MonoBehaviour
    {
        private void Awake( )
        {
            Debug.Log(JsonUtility.ToJson(new UserToken() { Token = "65455645"}));
            Debug.Log(JsonUtility.ToJson(new BanItem() { Token = "65455645", Name = "testuser"}));
            Debug.Log(JsonUtility.ToJson(new UserLogin() { Name = "testuser", Password = "testpass"}));
            Debug.Log(JsonUtility.ToJson(new UserRegister() { Name = "testuser", Password = "testpass", Email = "test@example.com" }));
        }

        public void AuthApiTest()
        {

        }
    }
}
