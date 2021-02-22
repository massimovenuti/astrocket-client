using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DestroyAsteroid : MonoBehaviour
{
    public string AsteroidStorageTagName = "AsteroidStorage";

    public Transform spawningRemains;
    public int _Size; //La taille sera attribué au spawn de l'astéroide (entre 3 et 1)

    private GameObject _asteroidStorage;
    private Rigidbody _rb;

    private void Awake()
    {
        GameObject go = GameObject.FindGameObjectsWithTag(AsteroidStorageTagName).First();
        if (go == null)
            Debug.LogError($"There were no GameObjects with tag {AsteroidStorageTagName} assigned self");
        else
            _asteroidStorage = go;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.mass = 15;
        spawningRemains = this.transform.Find("Remains");
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
    private void DestructionAsteroid()
    {
        Vector3 vector = this.GetComponent<Rigidbody>().velocity;
        Vector3 origin = this.transform.localPosition;

        Destroy(this.gameObject);

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
    private void DropRemains(Vector3 vec, Vector3 origin)
    {
        //Faut-il le placer comme attribut de classe ?
        float RemainSpeed = 1.5f;

        float Angle1 = Random.Range(Mathf.PI / 16, Mathf.PI / 8);
        float Angle2 = Random.Range(Mathf.PI / 16, Mathf.PI / 8);

        GameObject remain1 = (GameObject)Instantiate(this.gameObject, spawningRemains.position, spawningRemains.rotation);
        GameObject remain2 = (GameObject)Instantiate(this.gameObject, spawningRemains.position, spawningRemains.rotation);
        remain1.transform.parent = _asteroidStorage.transform;
        remain2.transform.parent = _asteroidStorage.transform;

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

        remain1.GetComponent<Rigidbody>().velocity = (new Vector3(Velocity1_X, 0, Velocity1_Z) * RemainSpeed);
        remain2.GetComponent<Rigidbody>().velocity = (new Vector3(Velocity2_X, 0, Velocity2_Z) * RemainSpeed);
    }


    //Amené à être modifié selon les choix sur les power up
    /// <summary>
    /// Laisse tomber les power-up
    /// </summary>
    private void DropPowerUP()
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
    private void HitByAsteroid( )
    {
        Destroy(this.gameObject);
        print("Vaiseau -10 de vie");
    }

}
