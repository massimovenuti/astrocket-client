﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Ammo : NetworkBehaviour
{
    [SerializeField] float destroyAfter = 0.5f;

    private Rigidbody rigidBody;

    [SerializeField] float _speed = 100f;
    
    [SyncVar]
    public uint ownerId;

    public override void OnStartServer( )
    {
        Invoke(nameof(DestroySelf), destroyAfter);
    }

    private void Start( )
    {
        GetComponent<Rigidbody>().velocity = transform.forward * _speed;
        Color c = NetworkIdentity.spawned[ownerId].GetComponent<PlayerSetup>().playerColor;
        c += new Color(0.4f, 0.4f, 0.4f);

        if (this.name.Contains("Bullet"))
        {
            GetComponent<Renderer>().material.SetColor("_Color", c);
            GetComponent<Renderer>().material.SetColor("_EmissionColor", c);
        } else if (this.name.Contains("Rocket")) {
            Renderer[] renders = GetComponentsInChildren<Renderer>();
            Debug.LogWarning("r color");
            foreach (Renderer r in renders)
            {
                foreach (Material m in r.materials)
                {
                    if (m.name.Contains("accent"))
                    {
                        m.SetColor("_Color", c);
                    }
                }
            }
        }
    }

    [Server]
    private void DestroySelf( )
    {
        NetworkServer.Destroy(gameObject);
    }
}
