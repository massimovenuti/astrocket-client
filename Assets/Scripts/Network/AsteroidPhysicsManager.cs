using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AsteroidPhysicsManager : NetworkBehaviour
{
    public string AsteroidStorageTagName = "AsteroidStorage";
    public float _asteroidForce = 10000f;

    private GameObject _asteroidStorage;

    public void Start( )
    {
        GameObject go = GameObject.FindGameObjectsWithTag(AsteroidStorageTagName)[0];
        if (go == null)
            Debug.LogError($"There were no GameObjects with tag {AsteroidStorageTagName} assigned self");
        else
            _asteroidStorage = go;

        this.transform.parent = _asteroidStorage.transform;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * _asteroidForce);
        //rb.AddTorque(transform.up * 10000);
    }
}
