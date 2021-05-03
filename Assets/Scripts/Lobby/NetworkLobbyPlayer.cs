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
        private Button _readyBtn;

        public override void OnStartClient( )
        {
            base.OnStartClient();

            Button leaveBtn = GameObject.Find("LeaveButton").GetComponent<Button>();
            leaveBtn.onClick.AddListener(exitLobby); // Bind button click to exitLobby()

            _statusField = GameObject.Find("RoomStatus").GetComponent<TMP_Text>();
            _readyBtn = GameObject.Find("ReadyButton").GetComponent<Button>();
            _readyBtn.onClick.AddListener(setPlayerReady);

            updateLobbyStatus();
        }

        public override void OnClientEnterRoom( )
        {
            _playersNum++;
            updateLobbyStatus();
        }

        public override void OnClientExitRoom( )
        {
            _playersNum--;
            updateLobbyStatus();
        }

        private void updateLobbyStatus()
        {
            _statusField.text = _playersNum.ToString() + "/8"; // TODO : ne pas hardcoder la valeur
        }

        private void setPlayerReady()
        {
            CmdChangeReadyState(true);
            _readyBtn.interactable = false;
        }

        private void exitLobby()
        {
            GameObject.Find("RoomManager").GetComponent<AsteroidNetworkManager>().StopClient();
        }
    }
}