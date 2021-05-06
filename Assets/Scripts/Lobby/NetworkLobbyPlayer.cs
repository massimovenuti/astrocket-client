using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Mirror.Examples.NetworkRoom
{
    [AddComponentMenu("")]
    public class NetworkLobbyPlayer : NetworkRoomPlayer
    {
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

            updateLobbyStatus(GameObject.FindGameObjectsWithTag("Player").Length);

            if (isLocalPlayer)
            {
                CmdChangeReadyState(false);
                _readyBtn.interactable = true;
            }
        }

        private void OnDestroy( )
        {
            updateLobbyStatus(GameObject.FindGameObjectsWithTag("Player").Length);
        }


        private void updateLobbyStatus(int curPlayers)
        {
            int maxPlayers = GameObject.Find("RoomManager").GetComponent<AsteroidNetworkManager>().maxConnections;
            _statusField.text = curPlayers.ToString() + "/" + maxPlayers.ToString(); // TODO : ne pas hardcoder la valeur
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