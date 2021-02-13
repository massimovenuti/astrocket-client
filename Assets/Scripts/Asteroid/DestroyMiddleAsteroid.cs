using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMiddleAsteroid : MonoBehaviour
{
    public GameObject _MiddleAsteroid;
    public GameObject _LittleAsteroid;

    private Rigidbody _rb;
    
    public Transform _SpawningLittleRemains;

    public GameObject _SpawningZone;

    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

   
    void FixedUpdate()
    {

    }


    void OnTriggerEnter(Collider intruder)
    {
        if(intruder.tag == "Bullet")
        {
            DestructionMiddleAsteroid();
        }

        if(intruder.tag == "Player")
        {
            HitByAsteroid();
        }

    }


    void DestructionMiddleAsteroid()
    {
        Destroy(_MiddleAsteroid);
        EvaluatePosition();
    }


    void EvaluatePosition()
    {
        float xmin = _SpawningZone.GetComponent<SphereCollider>().bounds.min.x;
        float xmax = _SpawningZone.GetComponent<SphereCollider>().bounds.max.x;

        float zmin = _SpawningZone.GetComponent<SphereCollider>().bounds.min.z;
        float zmax = _SpawningZone.GetComponent<SphereCollider>().bounds.max.z;

        float x = Random.Range(xmin, xmax);
        float z = Random.Range(zmin, zmax);

        DropRemains(new Vector3(x, 2.9f, z));

    }

    void DropRemains(Vector3 SpawnPoint) // + ,Vector3 vec
    {
        Instantiate(_LittleAsteroid, SpawnPoint, _SpawningLittleRemains.rotation);
        Instantiate(_LittleAsteroid, SpawnPoint, _SpawningLittleRemains.rotation);
    }

    void HitByAsteroid()
    {
        Destroy(_MiddleAsteroid);
        print("Vaiseau -10 de vie");
    }

}
