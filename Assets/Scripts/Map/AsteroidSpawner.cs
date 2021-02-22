using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AsteroidSpawner : MonoBehaviour
{
    public string AsteroidSpawnerStorageTagName = "AsteroidSpawnerStorage";
    public string AsteroidSpawnerName = "AsteroidSpawner";

    private GameObject _asteroidSpawner;
    private List<GameObject> _asteroidSpawnerList;

    private int _asteroidSpawnerAmount = 8;
    private int _mapRadiusLen = 160;
    private float _yAxis = 2.9f;

    private void Awake()
    {
        GameObject go = GameObject.FindGameObjectsWithTag(AsteroidSpawnerStorageTagName).First();
        if (go == null)
            Debug.LogError($"There were no GameObjects with tag {AsteroidSpawnerStorageTagName} assigned self");
        else
            _asteroidSpawner = go;

        _asteroidSpawnerList = new List<GameObject>();
    }

    private void Start()
    {
        InstantiateAsteroidSpawners();
    }

    private void Update()
    {
        
    }

    private void InstantiateAsteroidSpawners()
    {
        float angleStep = 360f / _asteroidSpawnerAmount;
        Vector3 pos = new Vector3(0, _yAxis, 0);
        Debug.Log("angle : " + angleStep);
        
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
}
