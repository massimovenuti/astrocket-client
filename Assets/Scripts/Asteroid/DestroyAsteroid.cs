using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAsteroid : MonoBehaviour
{
    
    public GameObject _Asteroid;

    //La taille sera attribué au spawn de l'astéroide
    public int _Size;

    private Rigidbody _rb;

    public Transform _SpawningRemains;

    //TEMP
    public float _VectorX;
    public float _VectorZ;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        
        //get le _SpawningRemains

        //J'initialise une direction arbitraire à l'asteroide
        _VectorX = Random.Range(-1f, 1f);
        _VectorZ = Random.Range(-1f, 1f);
    }

    void FixedUpdate()
    {
        Vector3 _direction = new Vector3(_VectorX, 0, _VectorZ);
        _rb.AddForce(_direction);
    }


    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Bullet")
        {

            Destroy(collision.gameObject);

            DestructionAsteroid(-collision.impulse);

        }

        if (collision.gameObject.tag == "Player")
        {
            //désoriente les axes du vaisseaux légèrement
            HitByAsteroid();
        }

    }


    void DestructionAsteroid(Vector3 impactPoint)
    {
        Destroy(_Asteroid);
        _Size--;

        if(_Size >= 1)
        {
            DropRemains(impactPoint);
        }
        else
        {
            DropPowerUP();
        }

        
    }

    void DropRemains(Vector3 vec)
    {
                
        float angle1 = Random.Range(0f, 1f) * 6.283185f;
        float angle2 = Random.Range(0f, 1f) * 6.283185f;
                                                                                            
        GameObject remain1 = (GameObject)Instantiate(_Asteroid, _SpawningRemains.position, _SpawningRemains.rotation);
        remain1.name = "Remain1";
        
        remain1.transform.localScale = new Vector3(remain1.transform.localScale.x / 2, remain1.transform.localScale.y / 2, remain1.transform.localScale.z / 2);
        remain1.GetComponent<Rigidbody>().AddForce((vec + new Vector3(10.0f * Mathf.Sin(angle1), 0, 10.0f * Mathf.Cos(angle1))) * 10);

        
        GameObject remain2 = (GameObject)Instantiate(_Asteroid, _SpawningRemains.position, _SpawningRemains.rotation);
        remain2.name = "Remain2";
        remain2.transform.localScale = new Vector3(remain2.transform.localScale.x/2, remain2.transform.localScale.y /2, remain2.transform.localScale.z/ 2);
        remain2.GetComponent<Rigidbody>().AddForce((vec + new Vector3(10.0f * Mathf.Sin(angle2), 0, 10.0f * Mathf.Cos(angle2))) * 10);
        
    }


    //amené a etre modifier selon les choix sur les power up
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

    void HitByAsteroid( )
    {
        Destroy(_Asteroid);
        print("Vaiseau -10 de vie");
    }

}
