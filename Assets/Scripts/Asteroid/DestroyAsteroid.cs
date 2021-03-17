﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class DestroyAsteroid : MonoBehaviour
{
    public string AsteroidStorageTagName = "AsteroidStorage";

    public Transform spawningRemains;

    public GameObject medikit;
    public GameObject mitraillette;
    public GameObject akimbo;
    public GameObject shield;
    public GameObject flash;
    public GameObject bazooka;
    public GameObject fantome;
    public GameObject slowness;
    public GameObject jammer;
    public GameObject teleportation;
    public GameObject leurre;
    public GameObject drone;
    public GameObject homingMissile;
    public GameObject heavyLaser;
    public GameObject mine;
    // public GameObject bonus;

    // la taille sera attribué au spawn de l'astéroide (entre 3 et 1)
    public int _Size; 
    public bool inMapBounds = false;

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
        spawningRemains = this.transform;
    }

    /// <summary>
    /// Se déclenche lorsque l'astéroide entre en collision avec un élément
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {     
        if (collision.gameObject.tag == "Bullet" || collision.gameObject.tag == "Rocket" || collision.gameObject.tag == "HeavyLaser")
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
    public void DestructionAsteroid()
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
            DropPowerUp();
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

        GameObject remain1 = (GameObject)Instantiate(this.gameObject, spawningRemains.position, Random.rotation);
        GameObject remain2 = (GameObject)Instantiate(this.gameObject, spawningRemains.position, Random.rotation);
        remain1.GetComponent<DestroyAsteroid>().inMapBounds = inMapBounds;
        remain2.GetComponent<DestroyAsteroid>().inMapBounds = inMapBounds;
        remain1.transform.parent = _asteroidStorage.transform;
        remain2.transform.parent = _asteroidStorage.transform;
        remain1.name = "Remain1";
        remain2.name = "Remain2";

        float PointSpawn1_X = Mathf.Cos(Mathf.PI / 2) * vec.x - Mathf.Sin(Mathf.PI / 2) * vec.z;
        float PointSpawn1_Z = Mathf.Sin(Mathf.PI / 2) * vec.x + Mathf.Cos(Mathf.PI / 2) * vec.z;
        float PointSpawn2_X = Mathf.Cos(-Mathf.PI / 2) * vec.x - Mathf.Sin(-Mathf.PI / 2) * vec.z;
        float PointSpawn2_Z = Mathf.Sin(-Mathf.PI / 2) * vec.x + Mathf.Cos(-Mathf.PI / 2) * vec.z;


        Vector3 PointSpawn1 = new Vector3 (PointSpawn1_X, 0, PointSpawn1_Z).normalized * (remain1.transform.localScale.x / 20) + origin;
        Vector3 PointSpawn2 = new Vector3(PointSpawn2_X, 0, PointSpawn2_Z).normalized * (remain1.transform.localScale.x / 20) + origin;

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
    private void DropPowerUp()
    {
        // 1 chance sur 2 de faire apparaitre
        // un power-up
        if (Random.value < 0.5)
        {
            // DEBUG
            Debug.Log("Pas de power-up");

            return;
        }

        // Pourcentage de change d'avoir un power-up
        int drop = Random.Range(1, 100);

        // TODO: change values
        if (drop > 0 && drop <= 17)
            InstantiatePowerUp(medikit);

        else if (drop > 17 && drop <= 29)
            InstantiatePowerUp(shield);

        else if (drop > 29 && drop <= 41)
            InstantiatePowerUp(mine);

        else if (drop > 41 && drop <= 53)
            InstantiatePowerUp(flash);

        else if (drop > 53 && drop <= 61)
            InstantiatePowerUp(heavyLaser);

        else if (drop > 61 && drop <= 69)
            Debug.Log("Not implemented yet...");    // BONUS

        else if (drop > 69 && drop <= 77)
            InstantiatePowerUp(mitraillette);

        else if (drop > 77 && drop <= 82)
            InstantiatePowerUp(homingMissile);

        else if (drop > 82 && drop <= 87)
            InstantiatePowerUp(drone);

        else if (drop > 87 && drop <= 90)
            InstantiatePowerUp(leurre);

        else if (drop > 90 && drop <= 93)
            InstantiatePowerUp(teleportation);

        else if (drop > 93 && drop <= 95)
            InstantiatePowerUp(akimbo);

        else if (drop > 95 && drop <= 97)
            InstantiatePowerUp(bazooka);

        else if (drop > 97 && drop <= 98)
            InstantiatePowerUp(jammer);

        else if (drop > 98 && drop <= 99)
            InstantiatePowerUp(slowness);

        else
            InstantiatePowerUp(fantome);
    }

    // Fonction instanciant un power-up
    private void InstantiatePowerUp(GameObject powerUp)
    {
        // DEBUG
        Debug.Log("Drop Power-Up : " + powerUp.name);

        GameObject PowerUp = (GameObject)Instantiate(powerUp, spawningRemains.position, Quaternion.Euler(0, 0, 0));

        // TODO: change values
        // détruit le power-up s'il n'est pas ramassé
        // au bout de 15 secondes
        Destroy(PowerUp, 15);

    }

    /// <summary>
    /// Inflige des dégats au vaisseau
    /// </summary>
    private void HitByAsteroid( )
    {
        Destroy(this.gameObject);
    }
}
