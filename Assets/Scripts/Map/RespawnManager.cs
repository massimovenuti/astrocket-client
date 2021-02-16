using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RespawnManager : MonoBehaviour
{
    public string SpawnStorageTagName = "SpawnStorage";
    public string RespawnPointName = "SpawnPoint";

    public GameObject respawnPrefab;

    public LayerMask checkLayers;

    private GameObject _respawnManager;

    private IEnumerable<Vector2> spawnPoints;
    private List<GameObject> respawnPointsList;

    private float height = 1f;
    private float radius = 50f;
    private float gridStep;

    private float checkRadius;

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
        gridStep = radius;
        checkRadius = radius / 2f;
        respawnPointsList = new List<GameObject>();
        spawnPoints = GetGridPointsInCircle();
        InstantiateSpawnPoints();
        GenerateEnemiesToSpot();
    }

    private void Update( )
    {
        if (Input.GetKeyDown("k"))
            Debug.Log("Safe : "+ getSafeRespawnPoint());
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
        int i = 0;

        foreach (Vector2 sp in spawnPoints)
        {
            GameObject go = new GameObject(RespawnPointName + (++i));
            go.transform.parent = _respawnManager.transform;
            
            Vector3 pos = new Vector3(sp.x, height, sp.y);
            go.transform.position = pos;

            respawnPointsList.Add(go);
        }
    }

    public Vector3 getSafeRespawnPoint()
    {
        GameObject bestRespawnPoint = null;
        float maxDistance = 0f;
        List<GameObject> l = new List<GameObject>();

        foreach (GameObject sp in respawnPointsList)
        {
            Collider[] colliders = Physics.OverlapSphere(sp.transform.position, checkRadius, checkLayers);

            if (colliders.Length == 0)
            {
                l.Add(sp);
            }
            else
            {
                float minDistance = radius;

                foreach(Collider c in colliders)
                {
                    float d = Vector3.Distance(sp.transform.position, c.transform.position);
                    if(d < minDistance)
                    {
                        minDistance = d;
                    }
                }

                if(minDistance > maxDistance && minDistance < checkRadius + 1f)
                {
                    maxDistance = minDistance;
                    bestRespawnPoint = sp;
                }
            }

            if (l.Count != 0)
                break;
        }

        int len = l.Count;
        GameObject go = (len != 0) ? l[Random.Range(0, len)] : bestRespawnPoint;
        Debug.Log(go.name);

        return go.transform.position;
    }

    public void GenerateEnemiesToSpot()
    {
        int times = 20;

        for(int i = 0; i < times; i++)
        {
            Vector3 pos = new Vector3((Random.value - 0.5f) * radius * 2, 1f, (Random.value - 0.5f) * radius * 2);
            Instantiate(respawnPrefab, pos, Quaternion.identity);
        }
    }

    /*
    private void OnDrawGizmos( )
    {
        foreach(Vector2 sp in spawnPoints)
        {
            Vector3 pos = new Vector3(sp.x, height, sp.y);
            Gizmos.DrawWireSphere(pos, checkRadius);
        }
    }
    */
}