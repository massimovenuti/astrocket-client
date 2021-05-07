using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LobbyScreenManager : MonoBehaviour
{
    private TMP_Text _statusField;

    void Start()
    {
        _statusField = GameObject.Find("RoomStatus").GetComponent<TMP_Text>();
        setPlayers(GameObject.Find("RoomManager").GetComponent<AsteroidNetworkManager>().roomPlayers);

#if UNITY_ANDROID
        Screen.orientation = ScreenOrientation.Landscape;
#endif
    }

    public void setPlayers(int players)
    {
        _statusField.text = players.ToString() + "/8";
    }

}
