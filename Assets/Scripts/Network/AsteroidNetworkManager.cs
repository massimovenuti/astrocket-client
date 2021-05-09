using API;
using API.Auth;
using API.Stats;
using API.MainServer;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AsteroidNetworkManager : NetworkRoomManager
{
    [SerializeField] string _asteroidSpawnerStorageTagName = "AsteroidSpawnerStorage";

    [SerializeField] string _asteroidSpawnerName = "AsteroidSpawner";

    private GameObject _asteroidSpawner;
    private List<GameObject> _asteroidSpawnerList;

    private int _asteroidSpawnerAmount = 16;
    private int _mapRadiusLen = 160;
    private float _yAxis = 0f;

    private int _randomIndex = 0;

    private float precision = 20; // variation de la précision en degré

    public string playerToken;
    private string serveurToken;
    private string serveurName;

    private List<(Transform, bool)> _roomPlayerSpawnsList;

    private Tuple<bool, Color>[] playersColor = { 
        new Tuple<bool, Color>(true, Color.red), 
        new Tuple<bool, Color>(true, Color.blue), 
        new Tuple<bool, Color>(true, Color.green), 
        new Tuple<bool, Color>(true, Color.yellow),
        new Tuple<bool, Color>(true, Color.cyan),
        new Tuple<bool, Color>(true, Color.magenta),
        new Tuple<bool, Color>(true, Color.white),
        new Tuple<bool, Color>(true, Color.grey),
    };

    [SerializeField]
    private GameObject _roomTimerPrefab;
    
    [SerializeField]
    private GameObject _gameTimerPrefab;

    private RoomTimer _roomTimer;
    private GameTimer _gameTimer;

    public int roomPlayers = 0;

    /*
    public override void Awake( )
    {
        base.Awake();
        List<string> args = Environment.GetCommandLineArgs().ToList();

        name = args[1];
        GetComponent<IgnoranceTransport>().CommunicationPort = Int32.Parse(args[2]);
        serveurToken = args[3];
    }
*/

    public override void OnRoomServerSceneChanged(string sceneName)
    {
        //start game
        if (sceneName == GameplayScene)
        {
            GameObject go = Instantiate(_gameTimerPrefab);
            _gameTimer = go.GetComponent<GameTimer>();
            NetworkServer.Spawn(go);
            SpawnAsteroids();

        }
        else if (sceneName == RoomScene)
        {
            GameObject go = Instantiate(_roomTimerPrefab);
            _roomTimer = go.GetComponent<RoomTimer>();
            NetworkServer.Spawn(go);
            InitRoomSpawners();
            if (roomSlots.Count >= minPlayers)
            {
                _roomTimer.StartTimer();
            }
        }
    }

    [Server]
    public override void OnStopServer( )
    {
        StopAllCoroutines();

        if (IsSceneActive(GameplayScene))
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Asteroid"))
            {
                NetworkServer.Destroy(go);
            }
        }

        base.OnStopServer();
    }

    public struct PlayerToken : NetworkMessage
    {
        public string token;
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        conn.Send<PlayerToken>(new PlayerToken { token = this.playerToken });
    }

    public override void OnStartServer( )
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<PlayerToken>(OnCreatePlayer, false);
    }

    public override void OnRoomServerPlayersReady( )
    {
        _roomTimer.SetTimer(6); // TODO : ne pas hardcoder la valeur
    }

    void OnCreatePlayer(NetworkConnection conn, PlayerToken token)
    {
        AuthAPICall api = new AuthAPICall();
        UserRole userInfo = api.PostCheckUserToken(token.token);
        if (userInfo == null)
        {
            conn.Disconnect();
        }

        /*        MainServerAPI mainApi = new MainServerAPI();
        mainApi.PutPlayerCount(new ServerToken { Token = serveurToken }, new SeverNameAndPlayerCount { Name = serveurName, PlayerCount = numPlayers });*/


        if (IsSceneActive(RoomScene))
        {
            // increment the index before adding the player, so first player starts at 1
            clientIndex++;

            if (roomSlots.Count == maxConnections)
            { 
                conn.Disconnect();
            }
            else if (roomSlots.Count == (minPlayers - 1))
            {
                _roomTimer.StartTimer();
            }

            allPlayersReady = false;

            int spawnNum = GetRoomPlayerSpawn();
            Transform spawnTrans = _roomPlayerSpawnsList[spawnNum].Item1;
            GameObject player = Instantiate(roomPlayerPrefab.gameObject, spawnTrans.position, spawnTrans.rotation);
            player.GetComponent<PlayerInfo>().playerName = userInfo.Name;
            player.GetComponent<PlayerInfo>().color = GetPlayerColor();
            player.GetComponent<RoomPlayer>().spawnNum = spawnNum;
            NetworkServer.AddPlayerForConnection(conn, player);
        }
    }

    private int GetRoomPlayerSpawn()
    {
        for (int i = 0; i < _roomPlayerSpawnsList.Count; i++)
        {
            if (_roomPlayerSpawnsList[i].Item2 == true)
            {
                _roomPlayerSpawnsList[i] = (_roomPlayerSpawnsList[i].Item1, false);
                return i;
            }
        }
        return -1;
    }

    private void FreeRoomSpawn(int spawn)
    {
        if (spawn >= _roomPlayerSpawnsList.Count)
        {
            return;
        }
        _roomPlayerSpawnsList[spawn] = (_roomPlayerSpawnsList[spawn].Item1, true);
    }

    private Color GetPlayerColor()
    {
        for (int i = 0; i < playersColor.Length; i++)
        {
            if (playersColor[i].Item1)
            {
                playersColor[i] = new Tuple<bool, Color>(false, playersColor[i].Item2);
                return playersColor[i].Item2;
            }
        }

        return Color.white;
    }

    private void FreePlayerColor(Color playerColor)
    {
        for (int i = 0; i < playersColor.Length; i++)
        {
            if (playersColor[i].Item2 == playerColor)
            {
                playersColor[i] = new Tuple<bool, Color>(true, playersColor[i].Item2);
                break;
            }
        }
    }

    public override void OnRoomStopClient( )
    {
        roomPlayers = 0;
    }

    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnection conn, GameObject roomPlayer, GameObject gamePlayer)
    {
        gamePlayer.GetComponent<PlayerInfo>().color = roomPlayer.GetComponent<PlayerInfo>().color;
        gamePlayer.GetComponent<PlayerInfo>().playerName = roomPlayer.GetComponent<PlayerInfo>().playerName;
        gamePlayer.GetComponent<PlayerScore>().rank = (ushort)(roomPlayer.GetComponent<NetworkRoomPlayer>().index + 1);
        return true;
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        NetworkIdentity[] clientObjects = new NetworkIdentity[conn.clientOwnedObjects.Count];
        conn.clientOwnedObjects.CopyTo(clientObjects);
        
        foreach (NetworkIdentity co in clientObjects)
        {
            GameObject go = NetworkIdentity.spawned[co.netId].gameObject;
            if (go.tag == "RoomPlayer")
            {
                FreePlayerColor(go.GetComponent<PlayerInfo>().color);
                FreeRoomSpawn(go.GetComponent<RoomPlayer>().spawnNum);
                break;
            }
        }

        base.OnServerDisconnect(conn);

        /*        MainServerAPI mainApi = new MainServerAPI();
        mainApi.PutPlayerCount(new ServerToken { Token = serveurToken }, new SeverNameAndPlayerCount { Name = serveurName, PlayerCount = numPlayers });*/

        if (IsSceneActive(GameplayScene) && (numPlayers < minPlayers))
        {
            _gameTimer.SetTimer(0);
        }
        else if (IsSceneActive(RoomScene) && (roomSlots.Count < minPlayers))
        {
            _roomTimer.StopTimer();
        }
    }

    public void StopGame()
    {
        StopAllCoroutines();
        /*
        StatsAPICall api = new StatsAPICall();

        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");

        PlayerStats[] updateStats = new PlayerStats[allPlayers.Length];

        PlayerInfo info;
        PlayerScore score;

        for (int i = 0; i < allPlayers.Length; i++)
        {
            info = allPlayers[i].GetComponent<PlayerInfo>();
            score = allPlayers[i].GetComponent<PlayerScore>();

            updateStats[i] = new PlayerStats()
            {
                username = info.name,
                nbPoints = score.nbPoints,
                nbKills = score.nbKills,
                nbAsteroids = score.nbAsteroids,
                nbDeaths = score.nbDeaths,
                nbPowerUps = score.nbPowerUps,
                nbGames = 1,
                nbWins = 0,
                maxKills = score.nbKills,
                maxPoints = score.nbPoints,
                maxPowerUps = score.nbPowerUps,
                maxDeaths = score.nbDeaths,
            };
        }

        updateStats.Where(e => e.nbPoints == updateStats.Max(f => f.nbPoints)).First().nbWins = 1;

        foreach (PlayerStats newStats in updateStats)
        {
            api.PostModifyPlayerStats(newStats.username, serveurToken, updateStats);
        }
        */
        
        ServerChangeScene(RoomScene);
    }

    public void StartGame()
    {
        ServerChangeScene(GameplayScene);
    }

    private void SpawnAsteroids()
    {
        GameObject go = GameObject.FindGameObjectsWithTag(_asteroidSpawnerStorageTagName)[0];
        if (go == null)
            Debug.LogError($"There were no GameObjects with tag {_asteroidSpawnerStorageTagName} assigned self");
        else
            _asteroidSpawner = go;

        _asteroidSpawnerList = new List<GameObject>();

        InstantiateAsteroidSpawners();
        StartCoroutine(SpawnAsteroid());

        // NetworkServer.Spawn(Instantiate(_timeManagerPrefab));
    }

    [Server]
    private void InstantiateAsteroidSpawners( )
    {
        float angleStep = 360f / _asteroidSpawnerAmount;
        Vector3 pos = new Vector3(0, _yAxis, 0);

        for (int i = 0; i < _asteroidSpawnerAmount; i++)
        {
            float theta = i * angleStep;
            pos.x = _mapRadiusLen * Mathf.Cos(theta * Mathf.Deg2Rad);
            pos.z = _mapRadiusLen * Mathf.Sin(theta * Mathf.Deg2Rad);

            GameObject go = new GameObject(_asteroidSpawnerName + (i + 1));
            go.transform.parent = _asteroidSpawner.transform;
            go.transform.position = pos;
            go.transform.rotation = Quaternion.LookRotation((Vector3.zero - go.transform.position).normalized);
            _asteroidSpawnerList.Add(go);
        }
    }

    [Server]
    private void InitRoomSpawners()
    {
        if (_roomPlayerSpawnsList == null)
        {
            _roomPlayerSpawnsList = new List<(Transform, bool)>();
            for (int i = 0; i < maxConnections; i++)
            {
                _roomPlayerSpawnsList.Add((null, true));
            }
        }
        Transform camTransform = Camera.main.transform;
        Transform spawnsTrans = GameObject.Find("SpawnPoints").transform;
        for (int i = 0; i < maxConnections; i++)
        {
            Transform t = spawnsTrans.GetChild(i);
            Vector3 direction = (camTransform.position - t.position).normalized;

            //create the rotation we need to be in to look at the target
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            //rotate us over time according to speed until we are in the required rotation
            t.rotation = lookRotation;

            //t.rotation = Quaternion.LookRotation(( - transform.position).normalized);
            _roomPlayerSpawnsList[i] = (t, _roomPlayerSpawnsList[i].Item2);
        }
    }

    [Server]
    private IEnumerator SpawnAsteroid( )
    {
        int tmp = UnityEngine.Random.Range(0, _asteroidSpawnerAmount);

        // random asteroid spawner (if the asteroid spawns two times at the same position, the second time will be transfered to another position)
        _randomIndex = (tmp == _randomIndex) ? (_randomIndex + 3) % _asteroidSpawnerAmount : tmp;

        Transform tf = _asteroidSpawnerList[_randomIndex].transform;
        Quaternion rot = new Quaternion(tf.rotation.x, tf.rotation.y, tf.rotation.z, tf.rotation.w);
        rot *= Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(-precision, precision));

        GameObject go = Instantiate(spawnPrefabs.Find(prefab => prefab.tag == "Asteroid"), tf.position, rot);
        NetworkServer.Spawn(go);

        yield return new WaitForSeconds(1f);

        StartCoroutine(SpawnAsteroid());
    }
}