using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBigAsteroid : MonoBehaviour
{
    
    public GameObject _BigAsteroid;
    public GameObject _MiddleAsteroid;
    
    private Rigidbody _rb;
    public Transform _SpawningRemains;


    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // FixedUpdate is called multiple time per frame
    private void FixedUpdate()
    {  

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
        DropRemains();
    }

    void DropRemains()
    {
        Instantiate(_MiddleAsteroid, _SpawningRemains.position /*+ new Vector3(0, 0, 0)*/, _SpawningRemains.rotation);
    }

    void HitByAsteroid()
    {
        Destroy(_BigAsteroid);
        print("Vaiseau -10 de vie");
    }

}
