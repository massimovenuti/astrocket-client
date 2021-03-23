using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class DestroyAsteroid : MonoBehaviour, IScoreable
{
    public string AsteroidStorageTagName = "AsteroidStorage";
    public Transform spawningRemains;

    // la taille sera attribué au spawn de l'astéroide (entre 1 et 3)
    public int size; 
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

    public bool inMapBounds = false;
    public long Score { get; set; }

    private GameObject _asteroidStorage;
    private Rigidbody _rb;
    private void Awake( )
    {
        GameObject go = GameObject.FindGameObjectsWithTag(AsteroidStorageTagName).First();
        if (go == null)
            Debug.LogError($"There were no GameObjects with tag {AsteroidStorageTagName} assigned self");
        else
            _asteroidStorage = go;

        Score = 100;
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
            addScore("id", Score, ScoreManager.Instance);
        }       

        if (collision.gameObject.tag == "Player")
        {
            HitByAsteroid(collision.gameObject);
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

        if (--size >= 1)
            DropRemains(vector, origin);
        else
            DropPowerUP();
    }

    /// <summary>
    /// Créé deux débris d'astéroïdes et leur ajoute un comportement
    /// </summary>
    private void DropRemains(Vector3 vec, Vector3 origin)
    {
        float remainSpeed = 1.5f;

        float angle1 = Random.Range(Mathf.PI / 16, Mathf.PI / 8);
        float angle2 = Random.Range(Mathf.PI / 16, Mathf.PI / 8);

        GameObject remain1 = (GameObject)Instantiate(this.gameObject, spawningRemains.position, Random.rotation);
        GameObject remain2 = (GameObject)Instantiate(this.gameObject, spawningRemains.position, Random.rotation);

        Rigidbody rb1 = remain1.GetComponent<Rigidbody>();
        Rigidbody rb2 = remain2.GetComponent<Rigidbody>();

        remain1.GetComponent<DestroyAsteroid>().inMapBounds = inMapBounds;
        remain2.GetComponent<DestroyAsteroid>().inMapBounds = inMapBounds;

        remain1.transform.parent = _asteroidStorage.transform;
        remain2.transform.parent = _asteroidStorage.transform;

        float spawnPoint1_X = Mathf.Cos(Mathf.PI / 2) * vec.x - Mathf.Sin(Mathf.PI / 2) * vec.z;
        float spawnPoint1_Z = Mathf.Sin(Mathf.PI / 2) * vec.x + Mathf.Cos(Mathf.PI / 2) * vec.z;
        float spawnPoint2_X = Mathf.Cos(-Mathf.PI / 2) * vec.x - Mathf.Sin(-Mathf.PI / 2) * vec.z;
        float spawnPoint2_Z = Mathf.Sin(-Mathf.PI / 2) * vec.x + Mathf.Cos(-Mathf.PI / 2) * vec.z;

        Vector3 spawnPoint1 = new Vector3 (spawnPoint1_X, 0, spawnPoint1_Z).normalized * (remain1.transform.localScale.x / 20) + origin;
        Vector3 spawnPoint2 = new Vector3(spawnPoint2_X, 0, spawnPoint2_Z).normalized * (remain1.transform.localScale.x / 20) + origin;

        remain1.transform.position = spawnPoint1;
        remain2.transform.position = spawnPoint2;

        remain1.transform.localScale = new Vector3(remain1.transform.localScale.x / 2, remain1.transform.localScale.y / 2, remain1.transform.localScale.z / 2);
        remain2.transform.localScale = new Vector3(remain2.transform.localScale.x / 2, remain2.transform.localScale.y / 2, remain2.transform.localScale.z / 2);

        rb1.mass /= 2;
        rb2.mass /= 2;

        float velocity1_X = Mathf.Cos(angle1) * vec.x - Mathf.Sin(angle1) * vec.z;
        float velocity1_Z = Mathf.Sin(angle1) * vec.x + Mathf.Cos(angle1) * vec.z;
        float velocity2_X = Mathf.Cos(-angle2) * vec.x - Mathf.Sin(-angle2) * vec.z;
        float velocity2_Z = Mathf.Sin(-angle2) * vec.x + Mathf.Cos(-angle2) * vec.z;

        rb1.velocity = (new Vector3(velocity1_X, 0, velocity1_Z) * remainSpeed);
        rb2.velocity = (new Vector3(velocity2_X, 0, velocity2_Z) * remainSpeed);

        float rotForce_tmp = (remain1.GetComponent<DestroyAsteroid>().size == 2) ? 5000 : 1000;

        rb1.AddTorque(transform.up * rotForce_tmp * ((Random.value < 0.5f) ? 1 : -1));
        rb2.AddTorque(transform.up * rotForce_tmp * ((Random.value < 0.5f) ? 1 : -1));
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
            return;

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

    /// <summary>
    /// Fonction instanciant un power-up
    /// </summary>
    private void InstantiatePowerUp(GameObject powerUp)
    {
        GameObject PowerUp = (GameObject)Instantiate(powerUp, spawningRemains.position, Quaternion.Euler(0, 0, 0));

        // TODO: change values
        // détruit le power-up s'il n'est pas ramassé
        // au bout de 15 secondes
        Destroy(PowerUp, 15);

    }

    /// <summary>
    /// Fonction détruisant l'astéroïde
    /// </summary>
    private void HitByAsteroid(GameObject player)
    {
        Destroy(this.gameObject);
    }
    
    public void addScore(string token, long score, ScoreManager scm)
    {
        scm.AddScore(token, score);
    }
}
