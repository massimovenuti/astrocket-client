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
    }

    void DestructionBigAsteroid()
    {
        Destroy(_BigAsteroid);
        DropRemains(2);
        DropPowerUP();
    }

    void DropRemains(int remains)
    {
        int i;
        for(i=0; i<remains; i++)
        {
            Instantiate(_MiddleAsteroid, _SpawningRemains.position + new Vector3(i*3, 0, i*3), _SpawningRemains.rotation);
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
