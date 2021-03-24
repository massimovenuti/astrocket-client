using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AsteroidNetworkManager : NetworkManager
{
    [SerializeField] string _asteroidSpawnerStorageTagName = "AsteroidSpawnerStorage";

    [SerializeField] string _asteroidSpawnerName = "AsteroidSpawner";

    private GameObject _asteroidSpawner;
    private List<GameObject> _asteroidSpawnerList;

    private int _asteroidSpawnerAmount = 16;
    private int _mapRadiusLen = 160;
    private float _yAxis = 0f;

    private int _maxAsteroidCount = 50;
    private int _randomIndex = 0;

    private float precision = 0.3f; // 0 => very precise

    [Server]
    public override void OnStartServer( )
    {
        GameObject go = GameObject.FindGameObjectsWithTag(_asteroidSpawnerStorageTagName)[0];
        if (go == null)
            Debug.LogError($"There were no GameObjects with tag {_asteroidSpawnerStorageTagName} assigned self");
        else
            _asteroidSpawner = go;

        _asteroidSpawnerList = new List<GameObject>();

        InstantiateAsteroidSpawners();
        StartCoroutine(SpawnAsteroid());
    }

    [Server]
    public override void OnStopServer( )
    {
        StopAllCoroutines();

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Asteroid"))
        {
            NetworkServer.Destroy(go);
        }

        base.OnStopServer();
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
            Debug.Log(go.transform.rotation);
            _asteroidSpawnerList.Add(go);
        }
    }

    [Server]
    private IEnumerator SpawnAsteroid( )
    {
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        int asteroidCount = asteroids.Length;

        if (asteroidCount < _maxAsteroidCount)
        {
            int tmp = Random.Range(0, _asteroidSpawnerAmount);

            // random asteroid spawner (if the asteroid spawns two times at the same position, the second time will be transfered to another position)
            _randomIndex = (tmp == _randomIndex) ? (_randomIndex + 3) % _asteroidSpawnerAmount : tmp;

            Transform tf = _asteroidSpawnerList[_randomIndex].transform;
            Quaternion rot = new Quaternion(tf.rotation.x, tf.rotation.y + Random.Range(-precision, precision), tf.rotation.z, tf.rotation.w);

            GameObject go = Instantiate(spawnPrefabs.Find(prefab => prefab.tag == "Asteroid"), tf.position, rot);
            NetworkServer.Spawn(go);
        }
        else
        {
            Debug.Log("Max number of asteroids reached : " + asteroidCount);
        }

        yield return new WaitForSeconds(1f);

        StartCoroutine(SpawnAsteroid());
    }
}