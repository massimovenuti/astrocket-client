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

    //faire une var pour la vitesse

    //TEMP
    public float _VectorX;
    public float _VectorZ;

    Vector3 _direction;

    public int index;

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
        //_direction = new Vector3(_VectorX, 0, _VectorZ);
        _direction = new Vector3(-1, 0, 1);
        _rb.AddForce(_direction*50);
    }


    private void OnCollisionEnter(Collision collision)
    {

        
        if (collision.gameObject.tag == "Bullet")
        {

            Destroy(collision.gameObject);

            DestructionAsteroid(_direction);

        }
        

        if (collision.gameObject.tag == "Player")
        {
            //désoriente les axes du vaisseaux légèrement
            HitByAsteroid();
        }

    }


    void DestructionAsteroid(Vector3 impactPoint)
    {
        Vector3 vector = _Asteroid.GetComponent<Rigidbody>().velocity;

        Vector3 origin = _Asteroid.transform.localPosition;

        Destroy(_Asteroid);

        _Size--;

        if(_Size >= 1)
        {
            DropRemains(vector, origin);
        }
        else
        {
            DropPowerUP();
        }

        
    }

    void DropRemains(Vector3 vec, Vector3 origin)
    {
        /*
        float angle1 = Random.Range(0f, 1f) * Mathf.PI;
        float angle2 = Random.Range(0f, 1f) * Mathf.PI;
        */

        float Angle = Vector3.Angle(vec, new Vector3(1, 0, 0));

        
      
                                                                                            
        GameObject remain1 = (GameObject)Instantiate(_Asteroid, _SpawningRemains.position, _SpawningRemains.rotation);
        remain1.name = "Remain1";
                                                                                                               
        Vector3 PointSpawn1 = origin + (new Vector3(- Mathf.Sin(-Angle) * (-remain1.transform.localScale.z / 2), 0, Mathf.Cos(-Angle) * (-remain1.transform.localScale.z / 2)));
        
        remain1.transform.position = PointSpawn1;
        remain1.transform.localScale = new Vector3(remain1.transform.localScale.x / 2, remain1.transform.localScale.y / 2, remain1.transform.localScale.z / 2);
        remain1.GetComponent<Rigidbody>().mass /= 2;

        remain1.GetComponent<Rigidbody>().AddForce(new Vector3(Mathf.Cos(Mathf.PI/6) * vec.x - Mathf.Sin(Mathf.PI/6) * vec.z, 0, Mathf.Sin(Mathf.PI/6)* vec.x + Mathf.Cos(Mathf.PI/6) * vec.z)*0);

       
        
        GameObject remain2 = (GameObject)Instantiate(_Asteroid, _SpawningRemains.position, _SpawningRemains.rotation);
        remain2.name = "Remain2";
                                                        
        Vector3 PointSpawn2 = origin + (new Vector3(-Mathf.Sin(-Angle) * (remain2.transform.localScale.z) / 2, 0, Mathf.Cos(-Angle) * (remain2.transform.localScale.z / 2)));
        remain2.transform.position = PointSpawn2;
        remain2.transform.localScale = new Vector3(remain2.transform.localScale.x/2, remain2.transform.localScale.y /2, remain2.transform.localScale.z/ 2);
        remain2.GetComponent<Rigidbody>().mass /= 2;

        remain2.GetComponent<Rigidbody>().AddForce(new Vector3(Mathf.Cos(-Mathf.PI / 6) * vec.x - Mathf.Sin(-Mathf.PI / 6) * vec.z, 0, Mathf.Sin(-Mathf.PI / 6) * vec.x + Mathf.Cos(-Mathf.PI / 6) * vec.z)*0);

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
