using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Mirror;

public class Asteroid : NetworkBehaviour
{
    public string AsteroidStorageTagName = "AsteroidStorage";
    public float _asteroidForce = 10000f;
    public bool inMapBounds = false;

    private int _size = 1;
    private GameObject _asteroidStorage;
    private Rigidbody _rb;

    public void Start( )
    {
        GameObject go = GameObject.FindGameObjectsWithTag(AsteroidStorageTagName).First();
        if (go == null)
            Debug.LogError($"There were no GameObjects with tag {AsteroidStorageTagName} assigned self");
        else
            _asteroidStorage = go;

        this.transform.parent = _asteroidStorage.transform;
        _rb = GetComponent<Rigidbody>();
        _rb.mass = 15;
        _rb.AddForce(transform.forward * _asteroidForce);
        //rb.AddTorque(transform.up * 10000); // DO NOT TOUCH
    }

    public int GetSize( )
    {
        return _size;
    }

    public void DecreaseSize( )
    {
        _size -= 1;
    }
}
