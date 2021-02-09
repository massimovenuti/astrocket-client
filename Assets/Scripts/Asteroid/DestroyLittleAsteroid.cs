using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyLittleAsteroid : MonoBehaviour
{
   
    public GameObject _LittleAsteroid;

    private Rigidbody _rb;
   
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
            DestructionLittleAsteroid();
        }
    }


    void DestructionLittleAsteroid()
    {
        Destroy(_LittleAsteroid);
        DropPowerUP();
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
