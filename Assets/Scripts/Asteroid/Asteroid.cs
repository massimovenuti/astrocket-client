using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Mirror;

public class Asteroid : NetworkBehaviour
{
    [SerializeField] string _asteroidStorageTagName = "AsteroidStorage";

    [SerializeField] float _asteroidForce = 10000f;

    [SerializeField] int _maxSize = 3;

    private GameObject _asteroidStorage;

    private bool _inMapBounds = false;

    private int _barrierVelocitySensitivity = 6;

    [SyncVar]
    private int _size;

    private void Awake( )
    {
        _size = _maxSize;
    }

    public void Start( )
    {
        GameObject go = GameObject.FindGameObjectsWithTag(_asteroidStorageTagName).First();
        if (go == null)
            Debug.LogError($"There were no GameObjects with tag {_asteroidStorageTagName} assigned self");
        else
            _asteroidStorage = go;

        this.transform.parent = _asteroidStorage.transform;

        GetComponent<Rigidbody>().mass = 10 * _size;

        GetComponent<Rigidbody>().AddForce(transform.forward * _asteroidForce);
    }

    [ServerCallback]
    public void Update( )
    {
        if (!_inMapBounds && GetComponent<Rigidbody>().velocity.magnitude < _barrierVelocitySensitivity)
        {
            NetworkServer.Destroy(gameObject);
        }
    }

    [ServerCallback]
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SmoothBorder"))
        {
            this._inMapBounds = false;
        }

        if (other.CompareTag("AbsoluteBorder"))
        {
            NetworkServer.Destroy(gameObject);
        }
    }

    [ServerCallback]
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SmoothBorder"))
        {
            this._inMapBounds = true;
        }

        if (other.CompareTag("Player"))
        {
            NetworkServer.Destroy(gameObject);
        }
    }

    public int GetSize( )
    {
        return _size;
    }

    public void SetSize(int size)
    {
        _size = size;
    }

    public bool IsInMapBounds()
    {
        return _inMapBounds;
    }

    public void SetInMapBounds(bool _inMapBounds)
    {
        this._inMapBounds = _inMapBounds;
    }
}
