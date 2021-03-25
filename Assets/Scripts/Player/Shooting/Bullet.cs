using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Bullet : NetworkBehaviour
{
    [SerializeField] float destroyAfter = 0.5f;

    [SerializeField] Rigidbody rigidBody;

    [SerializeField] float _speed = 100f;
    
    [SyncVar]
    public uint ownerId;

    [ServerCallback]
    public override void OnStartServer( )
    {
        Invoke(nameof(DestroySelf), destroyAfter);
    }

    private void Start( )
    {
        GetComponent<Rigidbody>().velocity = transform.forward * _speed;
    }

    [Server]
    private void DestroySelf( )
    {
        NetworkServer.Destroy(gameObject);
    }
}
