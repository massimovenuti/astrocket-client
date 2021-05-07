using UnityEngine;
using UnityEngine.UI;
using API.MainServer;
using System.Collections.Generic;
using System;

public class PlayScreenManager : ScreenManager
{
    private GameObject _servContainer; 
    public GameObject serverButtonPrefab;

    public override void Start( )
    {
#if UNITY_ANDROID
        Screen.orientation = ScreenOrientation.Landscape;
#endif
        base.Start();

        Button serveurButton = GameObject.Find("ServerButton").GetComponent<Button>();
        serveurButton.onClick.AddListener(runGame);

        _servContainer = GameObject.Find("PlayUI").gameObject;
    }

    private void getServerList()
    {
        MainServerAPI mainApi = new MainServerAPI();
        API.ServerListItem[] serverListItems = mainApi.GetServerList(); // TODO

        foreach (API.ServerListItem server in serverListItems)
        {
            GameObject servBtn = Instantiate(serverButtonPrefab);
            PlayButtonManager pbm = servBtn.GetComponent<PlayButtonManager>();
            pbm.ip = server.IP;
            pbm.port = server.Port;
            pbm.name = server.Name;
            pbm.numPlayers = server.PlayerCount;
            pbm.maxPlayers = 4; // TODO
            servBtn.transform.parent = _servContainer.transform;
        }
    }

    void runGame( )
    {
        GameObject.Find("RoomManager").GetComponent<AsteroidNetworkManager>().StartClient();
    }
}
