using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RespawnManager : MonoBehaviour
{
    [SerializeField] string RespawnPointStorageTagName = "RespawnPointStorage";

    [SerializeField] string RespawnPointName = "RespawnPoint";

    [SerializeField] float checkRadius = 25f;

    [SerializeField] LayerMask checkLayers;

    [SerializeField] GameObject enemyMockPrefab;

    private float _yAxis = 0f;

    private GameObject _respawnManager;

    private List<GameObject> _respawnPointsList;

    private readonly float _radius = 80f;

    private readonly float _gridStep = 50f;


    private void Awake( )
    {
        GameObject go = GameObject.FindGameObjectsWithTag(RespawnPointStorageTagName).First();
        if (go == null)
            Debug.LogError($"There were no GameObjects with tag {RespawnPointStorageTagName} assigned self");
        else
            _respawnManager = go;

        _respawnPointsList = new List<GameObject>();
    }

    private void Start( )
    {
        InstantiateRespawnPoints();
    }

    private void Update( )
    {
        if (Input.GetKeyDown("k"))
            Debug.Log("Safe : " + GetSafeRespawnPoint());
    }

    private IEnumerable<Vector2> GetGridPointsInCircle( )
    {
        int i1 = (int)Mathf.Ceil(-_radius / _gridStep);
        int i2 = (int)Mathf.Floor(_radius / _gridStep);

        float radius2 = _radius * _radius;

        for (int i = i1; i <= i2; i++)
        {
            float x = i * _gridStep;

            float localRadius = x;
            localRadius *= localRadius;
            localRadius = (float)Mathf.Sqrt(radius2 - localRadius);

            int j1 = (int)Mathf.Ceil(-localRadius / _gridStep);
            int j2 = (int)Mathf.Floor(localRadius / _gridStep);

            for (int j = j1; j <= j2; j++)
                yield return new Vector2(x, j * _gridStep);
        }
    }

    private void InstantiateRespawnPoints( )
    {
        int i = 0;

        foreach (Vector2 rsp in GetGridPointsInCircle())
        {
            GameObject go = new GameObject(RespawnPointName + (++i));
            go.transform.parent = _respawnManager.transform;

            Vector3 pos = new Vector3(rsp.x, _yAxis, rsp.y);
            go.transform.position = pos;

            _respawnPointsList.Add(go);
        }
    }

    public Vector3 GetSafeRespawnPoint( )
    {
        List<GameObject> l = new List<GameObject>();
        GameObject bestRespawnPoint = null;
        float maxDistance = 0f;
        int len = 0;

        foreach (GameObject rsp in _respawnPointsList)
        {
            Collider[] colliders = Physics.OverlapSphere(rsp.transform.position, checkRadius, checkLayers);

            if (colliders.Length == 0)
            {
                l.Add(rsp);
                len++;
            }
            else if (len == 0)
            {
                float minDistance = _radius;

                foreach (Collider c in colliders)
                {
                    // the distance is computed directly with the center of the collider and not the collider itself
                    // this implies the fact it may behave strangely with objects of different size like asteroids 
                    // we may compute it differently afterwards
                    float d = Vector3.Distance(rsp.transform.position, c.transform.position);
                    if (d < minDistance)
                        minDistance = d;

                }

                if (minDistance > maxDistance && minDistance < checkRadius + 1f)
                {
                    maxDistance = minDistance;
                    bestRespawnPoint = rsp;
                }
            }
        }

        GameObject go = (len != 0) ? l[Random.Range(0, len)] : bestRespawnPoint;
        Debug.Log(go.name);

        return go.transform.position;
    }

    private void OnDrawGizmos( )
    {
        if (!Application.isPlaying) return;

        foreach (GameObject rsp in _respawnPointsList)
            Gizmos.DrawWireSphere(rsp.transform.position, checkRadius);
    }
}