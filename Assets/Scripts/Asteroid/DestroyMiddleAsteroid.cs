using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMiddleAsteroid : MonoBehaviour
{
    public GameObject _MiddleAsteroid;
    public GameObject _LittleAsteroid;

    private Rigidbody _rb;
    
    public Transform _SpawningLittleRemains;

    
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
        DropRemains();
    }

    void DropRemains()
    {
        Instantiate(_LittleAsteroid, _SpawningLittleRemains.position /*+ new Vector3(0, 0, 0)*/, _SpawningLittleRemains.rotation);
    }

    void HitByAsteroid()
    {
        Destroy(_MiddleAsteroid);
        print("Vaiseau -10 de vie");
    }

}
