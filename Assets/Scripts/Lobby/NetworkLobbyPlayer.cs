using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class NetworkLobbyPlayer : NetworkRoomPlayer
{
    private Button _readyBtn;
    private AsteroidNetworkManager _roomManager;
    private LobbyScreenManager _lobbyScreen;

    [SerializeField]
    private Sprite _checkSprite, _crossSprite;
    private TMP_Text _playerReadyStatusText;

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

        _playerReadyStatusText = transform.Find("Canvas/ReadyStatus/TextStatus").GetComponent<TMP_Text>();
        _playerReadyStatusText.text = (readyToBegin) ? "Ready" : "Not Ready";
    }

    [ClientCallback]
    private void OnDestroy( )
    {
        _roomManager.roomPlayers--;
        if (_lobbyScreen != null)
        {
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

    public override void ReadyStateChanged(bool _, bool newReadyState)
    {
        base.ReadyStateChanged(_, newReadyState);

        TMP_Text readyStatusText = transform.Find("Canvas/ReadyStatus/TextStatus").GetComponent<TMP_Text>();
        Image readyStatusImage = transform.Find("Canvas/ReadyStatus/ImageStatus").GetComponent<Image>();

        if (newReadyState)
        {
            readyStatusText.text = "Ready";
            readyStatusImage.sprite = _checkSprite;
        }
        else
        {
            readyStatusText.text = "Not Ready";
            readyStatusImage.sprite = _crossSprite;
            _readyBtn.interactable = true;
        }
    }
}
