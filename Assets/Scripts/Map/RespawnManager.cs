﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RespawnManager : MonoBehaviour
{
    public string RespawnPointStorageTagName = "RespawnPointStorage";
    public string RespawnPointName = "RespawnPoint";

    public LayerMask checkLayers;
    public float checkRadius = 25f;
    public float yAxis = 1f;

    private List<GameObject> respawnPointsList;

    private GameObject _respawnManager;
    public GameObject _enemyMockPrefab;

    private float _radius = 50f;
    private float _gridStep = 50f;


    private void Awake( )
    {
        GameObject go = GameObject.FindGameObjectsWithTag(RespawnPointStorageTagName).First();
        if (go == null)
        {
            Debug.LogError($"There were no GameObjects with tag {RespawnPointStorageTagName} assigned self");
        }
        else
        {
            _respawnManager = go;
        }

        respawnPointsList = new List<GameObject>();
        checkRadius = _radius / 2f;
    }

    private void Start( )
    {
        InstantiateRespawnPoints();
        GenerateEnemiesToSpot();
    }

    private void Update( )
    {
        if (Input.GetKeyDown("k"))
            Debug.Log("Safe : "+ getSafeRespawnPoint());
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
            {
                yield return new Vector2(x, j * _gridStep);
            }
        }
    }

    private void InstantiateRespawnPoints( )
    {
        int i = 0;
        IEnumerable<Vector2> respawnPoints = GetGridPointsInCircle();

        foreach (Vector2 rsp in respawnPoints)
        {
            GameObject go = new GameObject(RespawnPointName + (++i));
            go.transform.parent = _respawnManager.transform;
            
            Vector3 pos = new Vector3(rsp.x, yAxis, rsp.y);
            go.transform.position = pos;

            respawnPointsList.Add(go);
        }
    }

    public Vector3 getSafeRespawnPoint()
    {
        List<GameObject> l = new List<GameObject>();
        GameObject bestRespawnPoint = null;
        float maxDistance = 0f;
        int len = 0;

        foreach (GameObject rsp in respawnPointsList)
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

                foreach(Collider c in colliders)
                {
                    // the distance is computed directly with the center of the collider and not the collider itself
                    // this implies the fact it may behave strangely with objects of different size like asteroids 
                    // we may compute it differently afterwards
                    float d = Vector3.Distance(rsp.transform.position, c.transform.position);
                    if(d < minDistance)
                    {
                        minDistance = d;
                    }
                }

                if(minDistance > maxDistance && minDistance < checkRadius + 1f)
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

    public void GenerateEnemiesToSpot()
    {
        int enemiesToSpawn = 20;

        for(int i = 0; i < enemiesToSpawn; i++)
        {
            Vector3 pos = new Vector3((Random.value - 0.5f) * _radius * 2, 1f, (Random.value - 0.5f) * _radius * 2);
            Instantiate(_enemyMockPrefab, pos, Quaternion.identity);
        }
    }

    private void OnDrawGizmos( )
    {
        if (!Application.isPlaying) return;

        foreach (GameObject rsp in respawnPointsList)
        {
            Gizmos.DrawWireSphere(rsp.transform.position, checkRadius);
        }
    }
}