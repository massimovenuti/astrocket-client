using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Bullet : NetworkBehaviour
{
    public float destroyAfter = 1;
    public Rigidbody rigidBody;
    public float force = 3000f;
    public NetworkIdentity ownerIdentity;

    public override void OnStartServer( )
    {
        Invoke(nameof(DestroySelf), destroyAfter);
    }

    void Start( )
    {
        rigidBody.AddForce(transform.forward * force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        DestroySelf();
    }

    [Server]
    void DestroySelf( )
    {
        NetworkServer.Destroy(gameObject);
    }
}
