using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Bullet : NetworkBehaviour
{
    [SerializeField] float destroyAfter = 1;

    [SerializeField] Rigidbody rigidBody;

    public float force = 3000f;
    
    [SyncVar]
    public uint ownerId;

    [ServerCallback]
    public override void OnStartServer( )
    {
        Invoke(nameof(DestroySelf), destroyAfter);
    }

    private void Start( )
    {
        rigidBody.AddForce(transform.forward * force);
    }

    [Server]
    private void DestroySelf( )
    {
        NetworkServer.Destroy(gameObject);
    }
}
