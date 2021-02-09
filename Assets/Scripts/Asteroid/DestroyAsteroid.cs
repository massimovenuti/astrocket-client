using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAsteroid : MonoBehaviour
{
    
    public GameObject _Asteroid;
    public Transform _SpawningRemains;
    //public GameObject _InstanciateBullet;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider intruder)
    {
        //pseudo-code
        /*
        vie_asteroid--;
        if(vie_asteroid == 0)
        {
            Destruction();
            DropPowerUP();
        }
        */
        
        //Pour le moment
        if(intruder.tag == "Bullet")
        {
            Destruction();
            DropPowerUP();

            Instantiate(_Asteroid, _SpawningRemains.position, _SpawningRemains.rotation);

            //Que pour les gros astéroides :
            //Remains();
        }


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

    void Remains()
    {

        GameObject fragment = (GameObject)Instantiate(_Asteroid, _Asteroid.transform.position, _Asteroid.transform.rotation);
        fragment.name = "AsteroidTEMP";
        //donner des directions aux débris
        //10 est une valeur am odifier
        
    }

    void Destruction()
    {
        Destroy(gameObject);
    }

}
