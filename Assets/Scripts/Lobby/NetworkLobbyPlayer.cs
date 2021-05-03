using UnityEngine;
using UnityEngine.UI;

namespace Mirror.Examples.NetworkRoom
{
    [AddComponentMenu("")]
    public class NetworkLobbyPlayer : NetworkRoomPlayer
    {
        public override void OnStartClient( )
        {
            // Debug.LogFormat(LogType.Log, "OnStartClient {0}", SceneManager.GetActiveScene().path);

            base.OnStartClient();

            Button leaveBtn = GameObject.Find("LeaveButton").GetComponent<Button>();
            leaveBtn.onClick.AddListener(exitLobby);
        }

        public override void OnClientEnterRoom( )
        {
            // Debug.LogFormat(LogType.Log, "OnClientEnterRoom {0}", SceneManager.GetActiveScene().path);
        }

        public override void OnClientExitRoom( )
        {
            // Debug.LogFormat(LogType.Log, "OnClientExitRoom {0}", SceneManager.GetActiveScene().path);
        }

        /*public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
        {
            // Debug.LogFormat(LogType.Log, "ReadyStateChanged {0}", newReadyState);
        }*/

        private void exitLobby()
        {
            GameObject.Find("RoomManager").GetComponent<AsteroidNetworkManager>().StopClient();
        }
    }
}