using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayScreenManager : ScreenManager
{
    public override void Start( )
    {
        base.Start();

        Button serveurButton = GameObject.Find("ServerButton").GetComponent<Button>();
        serveurButton.onClick.AddListener(runGame);
    }

    void runGame( )
    {
        GameObject.Find("RoomManager").GetComponent<AsteroidNetworkManager>().StartClient();
    }
}
