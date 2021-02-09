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
    }


    void DestructionMiddleAsteroid()
    {
        Destroy(_MiddleAsteroid);
        DropRemains(2);
        DropPowerUP();
    }

    void DropRemains(int remains)
    {
        int i;
        for(i=0; i<remains; i++)
        {
            Instantiate(_LittleAsteroid, _SpawningLittleRemains.position + new Vector3(i*2, 0, i*2), _SpawningLittleRemains.rotation);
        }

    }

    void DropPowerUP()
    {
       int dropRate = Random.Range(1,100);

        if (dropRate <= 20)
        {
            if (dropRate <= 10)
            {
                print("Power-up : Regen vie");
            }
            if (dropRate > 10)
            {
                print("Power-up : Shield");
            }
        }
        else
        {
            print("Pas de Power-up");
        }
    }
}
