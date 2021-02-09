using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAsteroid : MonoBehaviour
{
    
    public GameObject _Asteroid;
<<<<<<< HEAD

    private Rigidbody _rb;

    public Transform _SpawningRemains;

    //La taille sera attribué au spawn de l'astéroide (entre 3 et 1)
    public int _Size;

    //TEMP
    float _VectorX;
    float _VectorZ;
    Vector3 _direction;
    int _AsteroidVelocity = 3;


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.mass = 15;
        _SpawningRemains = _Asteroid.transform.Find("Remains");

        //J'initialise une direction arbitraire à l'asteroide
        _VectorX = Random.Range(-1f, 1f);
        _VectorZ = Random.Range(-1f, 1f);
        _direction = new Vector3(_VectorX, 0, _VectorZ);
        _rb.velocity = _direction.normalized * _AsteroidVelocity;
    }

    /// <summary>
    /// Se déclenche lorsque l'astéroide entre en collision avec un élément
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {     
        if (collision.gameObject.tag == "Bullet")
        {
            Destroy(collision.gameObject);
            DestructionAsteroid();
        }       

        if (collision.gameObject.tag == "Player")
        {
            HitByAsteroid();
        }
    }

    /// <summary>
    /// Fractionne l'astéroïde ou le détruit
    /// </summary>
    void DestructionAsteroid()
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

    /// <summary>
    /// Créé deux débris d'astéroïdes et leur ajoute un comportement
    /// </summary>
    void DropRemains(Vector3 vec, Vector3 origin)
    {
        //Faut-il le placer comme attribut de classe ?
        float RemainSpeed = 1.5f;

        float Angle1 = Random.Range(Mathf.PI / 16, Mathf.PI / 8);
        float Angle2 = Random.Range(Mathf.PI / 16, Mathf.PI / 8);


        GameObject remain1 = (GameObject)Instantiate(_Asteroid, _SpawningRemains.position, _SpawningRemains.rotation);
        GameObject remain2 = (GameObject)Instantiate(_Asteroid, _SpawningRemains.position, _SpawningRemains.rotation);

        remain1.name = "Remain1";
        remain2.name = "Remain2";

        float PointSpawn1_X = Mathf.Cos(Mathf.PI / 2) * vec.x - Mathf.Sin(Mathf.PI / 2) * vec.z;
        float PointSpawn1_Z = Mathf.Sin(Mathf.PI / 2) * vec.x + Mathf.Cos(Mathf.PI / 2) * vec.z;
        float PointSpawn2_X = Mathf.Cos(-Mathf.PI / 2) * vec.x - Mathf.Sin(-Mathf.PI / 2) * vec.z;
        float PointSpawn2_Z = Mathf.Sin(-Mathf.PI / 2) * vec.x + Mathf.Cos(-Mathf.PI / 2) * vec.z;


        Vector3 PointSpawn1 = new Vector3 (PointSpawn1_X, 0, PointSpawn1_Z).normalized * (remain1.transform.localScale.x / 4) + origin;
        Vector3 PointSpawn2 = new Vector3(PointSpawn2_X, 0, PointSpawn2_Z).normalized * (remain1.transform.localScale.x / 4) + origin;

        remain1.transform.position = PointSpawn1;
        remain2.transform.position = PointSpawn2;

        remain1.transform.localScale = new Vector3(remain1.transform.localScale.x / 2, remain1.transform.localScale.y / 2, remain1.transform.localScale.z / 2);
        remain2.transform.localScale = new Vector3(remain2.transform.localScale.x / 2, remain2.transform.localScale.y / 2, remain2.transform.localScale.z / 2);

        remain1.GetComponent<Rigidbody>().mass /= 2;
        remain2.GetComponent<Rigidbody>().mass /= 2;

        float Velocity1_X = Mathf.Cos(Angle1) * vec.x - Mathf.Sin(Angle1) * vec.z;
        float Velocity1_Z = Mathf.Sin(Angle1) * vec.x + Mathf.Cos(Angle1) * vec.z;
        float Velocity2_X = Mathf.Cos(-Angle2) * vec.x - Mathf.Sin(-Angle2) * vec.z;
        float Velocity2_Z = Mathf.Sin(-Angle2) * vec.x + Mathf.Cos(-Angle2) * vec.z;


        remain1.GetComponent<Rigidbody>().velocity = (new Vector3(Velocity1_X, 0, Velocity1_Z) * RemainSpeed);
        remain2.GetComponent<Rigidbody>().velocity = (new Vector3(Velocity2_X, 0, Velocity2_Z) * RemainSpeed);
    }


    //Amené à être modifié selon les choix sur les power up
    /// <summary>
    /// Laisse tomber les power-up
    /// </summary>
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

    /// <summary>
    /// Inflige des dégats au vaisseau
    /// </summary>
    void HitByAsteroid( )
    {
        Destroy(_Asteroid);
        print("Vaiseau -10 de vie");
    }

}
