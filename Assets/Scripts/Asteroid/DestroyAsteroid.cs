using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAsteroid : MonoBehaviour
{
    
    public GameObject _AsteroidTEMP;
    public Transform _SpawningRemains;

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
            DestructionAsteroid();
        }

    }


    void DestructionAsteroid()
    {
        Destroy(_AsteroidTEMP);
        DropRemains();
    }


    void DropRemains()
    {
        Instantiate(_AsteroidTEMP, _SpawningRemains.position + new Vector3(0, 0, 0), _SpawningRemains.rotation);

    }


    //amené a etre modifier selon les choix sur les power up
    void DropPowerUP()
    {
       int dropRate = Random.Range(1,100);

        if (dropRate <= 70)
        {
            if (dropRate <= 50)
            {
                print("Power-up : Regen vie");
            }
            if (dropRate > 50)
            {
                print("Power-up : Shield");
            }
        }
        else
        {
            print("Pas de Power-up");
        }
    }



    void Destruction()
    {
        Destroy(gameObject);
    }

}
