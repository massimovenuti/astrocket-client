using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class NetworkLobbyPlayer : NetworkRoomPlayer
{
    private Button _readyBtn;
    private bool _firstRoom = true;
    private AsteroidNetworkManager _roomManager;
    private LobbyScreenManager _lobbyScreen;

    public override void OnStartClient( )
    {
        base.OnStartClient();

        Button leaveBtn = GameObject.Find("LeaveButton").GetComponent<Button>();
        leaveBtn.onClick.AddListener(exitLobby); // Bind button click to exitLobby()

        _readyBtn = GameObject.Find("ReadyButton").GetComponent<Button>();
        _readyBtn.onClick.AddListener(setPlayerReady);

        _lobbyScreen = GameObject.Find("LobbyScreen").GetComponent<LobbyScreenManager>();
        _roomManager = GameObject.Find("RoomManager").GetComponent<AsteroidNetworkManager>();

        _roomManager.roomPlayers++;
        _lobbyScreen.setPlayers(_roomManager.roomPlayers);

        if (isLocalPlayer)
        {
            CmdChangeReadyState(false);
            _readyBtn.interactable = true;
        }
    }

    private void OnDestroy( )
    {
        if (_lobbyScreen != null)
        {
            _roomManager.roomPlayers--;
            _lobbyScreen.setPlayers(_roomManager.roomPlayers);
        }
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
