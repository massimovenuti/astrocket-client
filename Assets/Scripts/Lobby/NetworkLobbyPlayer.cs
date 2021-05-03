using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Mirror.Examples.NetworkRoom
{
    [AddComponentMenu("")]
    public class NetworkLobbyPlayer : NetworkRoomPlayer
    {
        private int _playersNum = 0;
        private TMP_Text _statusField;

        public override void OnStartClient( )
        {
            base.OnStartClient();

            Button leaveBtn = GameObject.Find("LeaveButton").GetComponent<Button>();
            leaveBtn.onClick.AddListener(exitLobby); // Bind button click to exitLobby()

            _statusField = GameObject.Find("RoomStatus").GetComponent<TMP_Text>();
            updateLobbyStatus();
        }

        public override void OnClientEnterRoom( )
        {
            _playersNum++;
            
        }

        public override void OnClientExitRoom( )
        {
            _playersNum--;
        }

        private void updateLobbyStatus()
        {
            _statusField.text = _playersNum.ToString() + "/8"; // TODO : ne pas hardcoder la valeur
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