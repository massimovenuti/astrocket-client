using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLifeTime : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 1);
    }


    //A delete
    /*
    void OnTriggerEnter(Collider intruder)
    {
        //A changer le nom avec le nom de l'astéroide instanciée
        if(intruder.tag == "Asteroid")
        {
            DestroyBullet();
            intruder.SendMessage("ShootingVector", gameObject.transform.forward);
        }
    }
    */

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }

}
