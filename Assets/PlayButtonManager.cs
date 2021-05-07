using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class PlayButtonManager : MonoBehaviour
{
    public string ip;
    public int port;

    public string name;
    public int numPlayers;
    public int maxPlayers;

    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(runGame);
    }

    void runGame( )
    {
        AsteroidNetworkManager go = GameObject.Find("RoomManager").GetComponent<AsteroidNetworkManager>();
        go.networkAddress = ip;
        go.GetComponent<IgnoranceTransport>().CommunicationPort = port;
        go.StartClient();
    }
}
