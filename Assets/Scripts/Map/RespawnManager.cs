using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RespawnManager : MonoBehaviour
{
    public string SpawnStorageTagName = "SpawnStorage";
    public string RespawnPointName = "SpawnPoint";

    public GameObject respawnPrefab;

    private GameObject _respawnManager;
    private IEnumerable<Vector2> spawnPoints;

    private float radius = 50f;
    private float gridStep;

    private void Start( )
    {
        GameObject[] l = GameObject.FindGameObjectsWithTag(SpawnStorageTagName);
        if (l.Length == 0)
        {
            Debug.LogError($"There were no GameObjects with tag {SpawnStorageTagName} assigned self");
            _respawnManager = gameObject;
        }
        else if (l.Length >= 1)
        {
            _respawnManager = l.First();
            if (l.Length > 1)
                Debug.LogWarning($"{SpawnStorageTagName} had more than one element assigned ({l.Length})");
        }
        gridStep = radius / 2f;

        spawnPoints = GetGridPointsInCircle();
        InstantiateSpawnPoints();
    }

    private IEnumerable<Vector2> GetGridPointsInCircle( )
    {
        int i1 = (int)Mathf.Ceil(-radius / gridStep);
        int i2 = (int)Mathf.Floor(radius / gridStep);

        float radius2 = radius * radius;

        for (int i = i1; i <= i2; i++)
        {
            float x = i * gridStep;

            float localRadius = x;
            localRadius *= localRadius;
            localRadius = (float)Mathf.Sqrt(radius2 - localRadius);

            int j1 = (int)Mathf.Ceil(-localRadius / gridStep);
            int j2 = (int)Mathf.Floor(localRadius / gridStep);

            for (int j = j1; j <= j2; j++)
            {
                yield return new Vector2(x, j * gridStep);
            }
        }
    }

    private void InstantiateSpawnPoints( )
    {
        foreach (Vector2 sp in spawnPoints)
        {
            Vector3 pos = new Vector3(sp.x, 0f, sp.y);
            GameObject go = new GameObject(RespawnPointName);
            go.transform.parent = _respawnManager.transform;
            go.transform.position = pos;
        }
    }
}