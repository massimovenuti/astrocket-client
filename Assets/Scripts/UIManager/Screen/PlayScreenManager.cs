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

       /* Button serveurButton = GameObject.Find("ServerButton").GetComponent<Button>();
        serveurButton.onClick.AddListener(runGame);*/

        _servContainer = GameObject.Find("PlayUI").gameObject;

        getServerList();
    }

    private void getServerList()
    {
        MainServerAPI mainApi = new MainServerAPI();
        API.ServerListItem[] serverListItems = mainApi.GetServerList(SharedInfo.userToken); // TODO
        Debug.LogWarning(mainApi.ErrorMessage);

        foreach (API.ServerListItem server in serverListItems)
        {
            Debug.Log(server.IP);
            GameObject servBtn = Instantiate(serverButtonPrefab, _servContainer.transform);
            PlayButtonManager pbm = servBtn.GetComponent<PlayButtonManager>();
            pbm.ip = server.IP;
            pbm.port = server.Port;
            servBtn.transform.Find("name").GetComponent<TMPro.TMP_Text>().text = server.Name;
            servBtn.transform.Find("numPlayer").GetComponent<TMPro.TMP_Text>().text = $"{server.PlayerCount}/4";
        }
    }

    void runGame( )
    {
        GameObject.Find("RoomManager").GetComponent<AsteroidNetworkManager>().StartClient();
    }
}
