using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AsteroidSpawner : MonoBehaviour
{
    public string AsteroidSpawnerStorageTagName = "AsteroidSpawnerStorage";
    public string AsteroidStorageTagName = "AsteroidStorage";
    public string AsteroidSpawnerName = "AsteroidSpawner";

    public GameObject asteroidPrefab;

    private GameObject _asteroidStorage;
    private GameObject _asteroidSpawner;
    private List<GameObject> _asteroidSpawnerList;

    private int _asteroidSpawnerAmount = 8;
    private int _mapRadiusLen = 175;
    private float _yAxis = 2.9f;

    private float _asteroidVelocity = 10f;
    private int _barrierVelocitySensitivity = 6;

    private int _maxAsteroidCount = 50;
    private int _randomIndex = 0;

    private void Awake()
    {
        GameObject go = GameObject.FindGameObjectsWithTag(AsteroidSpawnerStorageTagName).First();
        if (go == null)
            Debug.LogError($"There were no GameObjects with tag {AsteroidSpawnerStorageTagName} assigned self");
        else
            _asteroidSpawner = go;

        go = GameObject.FindGameObjectsWithTag(AsteroidStorageTagName).First();
        if (go == null)
            Debug.LogError($"There were no GameObjects with tag {AsteroidStorageTagName} assigned self");
        else
            _asteroidStorage = go;

        _asteroidSpawnerList = new List<GameObject>();
    }

    private void Start()
    {
        InstantiateAsteroidSpawners();

        // invoke every 2 seconds starting from time.deltaTime = 0f
        InvokeRepeating("SpawnAsteroid", 0f, 2f);
    }

    private void InstantiateAsteroidSpawners()
    {
        float angleStep = 360f / _asteroidSpawnerAmount;
        Vector3 pos = new Vector3(0, _yAxis, 0);
        
        for (int i = 0; i < _asteroidSpawnerAmount; i++)
        {
            float theta = i * angleStep;
            pos.x = _mapRadiusLen * Mathf.Cos(theta * Mathf.Deg2Rad);
            pos.z = _mapRadiusLen * Mathf.Sin(theta * Mathf.Deg2Rad);

            GameObject go = new GameObject(AsteroidSpawnerName + (i+1));
            go.transform.parent = _asteroidSpawner.transform;
            go.transform.position = pos;
            _asteroidSpawnerList.Add(go);
        }
    }

    private void SpawnAsteroid()
    {
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        int asteroidCount = asteroids.Length;

        if (asteroidCount < _maxAsteroidCount)
        {
            int tmp = Random.Range(0, _asteroidSpawnerAmount);

            // random asteroid spawner (if the asteroid spawns two times at the same position, the second time will be transfered to another position)
            _randomIndex = (tmp == _randomIndex) ? (_randomIndex + 3) % _asteroidSpawnerAmount : tmp;

            Transform tf = _asteroidSpawnerList[_randomIndex].transform;
            Vector3 dir = -tf.position.normalized;

            // random angle towards center of the map
            dir += new Vector3(Random.Range(-0.2f, 0.2f), 0, Random.Range(-0.2f, 0.2f));

            GameObject go = Instantiate(asteroidPrefab, tf.position, tf.rotation);
            go.transform.parent = _asteroidStorage.transform;
            Rigidbody rb = go.GetComponent<Rigidbody>();
            rb.velocity = dir * _asteroidVelocity;
        }
        else
        {
            Debug.Log("Max number of asteroids reached : " + asteroidCount);
        }

        CheckAsteroidPosition(asteroids);
    }

    private void CheckAsteroidPosition(GameObject[] asteroids)
    {
        foreach (GameObject asteroid in asteroids)
        {
            bool inMapBounds = asteroid.GetComponent<DestroyAsteroid>().inMapBounds;
            float speed = asteroid.GetComponent<Rigidbody>().velocity.magnitude;

            if (!inMapBounds && speed < _barrierVelocitySensitivity)
                Destroy(asteroid);
        }
    }
}
