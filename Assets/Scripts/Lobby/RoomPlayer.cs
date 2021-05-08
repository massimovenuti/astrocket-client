using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class RoomPlayer : NetworkRoomPlayer
{
    private Button _readyBtn, _leaveBtn;
    private AsteroidNetworkManager _roomManager;
    private LobbyScreenManager _lobbyScreen;

    public Sprite _checkSprite, _crossSprite;
    
    private TMP_Text _playerReadyStatusText;

    public override void OnStartClient( )
    {
        base.OnStartClient();

        _leaveBtn = GameObject.Find("LeaveButton").GetComponent<Button>();
        _readyBtn = GameObject.Find("ReadyButton").GetComponent<Button>();
        _lobbyScreen = GameObject.Find("LobbyScreen").GetComponent<LobbyScreenManager>();
        _roomManager = GameObject.Find("RoomManager").GetComponent<AsteroidNetworkManager>();
        _playerReadyStatusText = transform.Find("Canvas/ReadyStatus/TextStatus").GetComponent<TMP_Text>();

        if (isLocalPlayer)
        {
            _leaveBtn.onClick.AddListener(ExitLobby); // Bind button click to exitLobby()
            _readyBtn.onClick.AddListener(SetPlayerReady);
        }

        _roomManager.roomPlayers++;
        _lobbyScreen.setPlayers(_roomManager.roomPlayers);
        _playerReadyStatusText.text = GetComponent<PlayerInfo>().playerName;
    }

    [ClientCallback]
    public override void OnGUI( )
    {
        if (!NetworkManager.IsSceneActive(_roomManager.RoomScene))
        {
            return;
        }
        _leaveBtn = GameObject.Find("LeaveButton").GetComponent<Button>();
        _readyBtn = GameObject.Find("ReadyButton").GetComponent<Button>();
        _lobbyScreen = GameObject.Find("LobbyScreen").GetComponent<LobbyScreenManager>();
        if (isLocalPlayer)
        {
            _leaveBtn.onClick.AddListener(ExitLobby); // Bind button click to exitLobby()
            _readyBtn.onClick.AddListener(SetPlayerReady);
        }
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

    private void SetPlayerReady()
    {
        CmdChangeReadyState(true);
        _readyBtn.interactable = false;
    }

    private void ExitLobby()
    {
        GameObject.Find("RoomManager").GetComponent<AsteroidNetworkManager>().StopClient();
    }

    public override void ReadyStateChanged(bool _, bool newReadyState)
    {
        TMP_Text readyStatusText = transform.Find("Canvas/ReadyStatus/TextStatus").GetComponent<TMP_Text>();
        Image readyStatusImage = transform.Find("Canvas/ReadyStatus/ImageStatus").GetComponent<Image>();

        if (newReadyState)
        {
            readyStatusImage.sprite = _checkSprite;
        }
        else
        {
            readyStatusImage.sprite = _crossSprite;
        }
    }
}
