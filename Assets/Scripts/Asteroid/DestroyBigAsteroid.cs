using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBigAsteroid : MonoBehaviour
{
    
    public GameObject _BigAsteroid;
    public GameObject _MiddleAsteroid;
    
    private Rigidbody _rb;
    public Transform _SpawningRemains;


    public GameObject _SpawningZone;


    public float _VectorX;
    public float _VectorZ;

    //a supp
    public float _Vector;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        /*
        _VectorX = Random.Range(-1f, 1f);
        _VectorZ = Random.Range(-1f, 1f);
        */
        _Vector = -1;
    }

    // FixedUpdate is called multiple time per frame
    private void FixedUpdate()
    {  
        
        //Vector3 _direction = new Vector3(_VectorX, 0, _VectorZ);
        Vector3 _direction = new Vector3(_Vector, 0, 0);
        _rb.AddForce(_direction);
    }


    void OnTriggerEnter(Collider intruder)
    {
        if(intruder.tag == "Bullet")
        {
            DestructionBigAsteroid();
        }

        if(intruder.tag == "Player")
        {
            HitByAsteroid();
        }
    }

    void DestructionBigAsteroid()
    {
        Destroy(_BigAsteroid);
        //EvaluatePosition();
    }


/*
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
*/

    void HitByAsteroid()
    {
        Destroy(_BigAsteroid);
        print("Vaiseau -10 de vie");
    }


    //sendMessage de BulletLifeTime
    // Quand le bullet touche l'asteroide
    void ShootingVector(Vector3 vec)
    {
        
        print("DestroyBigAsteroid " + vec);

        float xmin = _SpawningZone.GetComponent<SphereCollider>().bounds.min.x;
        float xmax = _SpawningZone.GetComponent<SphereCollider>().bounds.max.x;

        float zmin = _SpawningZone.GetComponent<SphereCollider>().bounds.min.z;
        float zmax = _SpawningZone.GetComponent<SphereCollider>().bounds.max.z;

        float x = Random.Range(xmin, xmax);
        float z = Random.Range(zmin, zmax);

        DropRemains(new Vector3(x, 2.9f, z) ,vec);

    }

    void DropRemains(Vector3 SpawnPoint, Vector3 vec)
    {
        
        GameObject remain1 = (GameObject)Instantiate(_MiddleAsteroid, SpawnPoint, _SpawningRemains.rotation);
        remain1.GetComponent<Rigidbody>().AddForce(vec + new Vector3(-45,0,45));
        //Instantiate(_MiddleAsteroid, SpawnPoint, _SpawningRemains.rotation);
        GameObject remain2 = (GameObject)Instantiate(_MiddleAsteroid, SpawnPoint, _SpawningRemains.rotation);
        remain2.GetComponent<Rigidbody>().AddForce(vec + new Vector3(-45,0,-45));
        
    }

}
