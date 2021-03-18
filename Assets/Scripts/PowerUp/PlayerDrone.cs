using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrone : MonoBehaviour
{
    public GameObject drone;
    public GameObject bullet;
    public Material bulletMaterial;
    public PlayerHealth accessShield;

    private GameObject _player;
    private GameObject _droneBarrel;

    public bool hasDrone;

    private float _droneShootRate = 1f;
    private float _droneShootForce = 3000f;
    private float _lastShootingTimeRef;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    private void Start( )
    {
        // on désactive le drone au tout début
        drone = GameObject.Find("Drone");
        drone.SetActive(false);
        hasDrone = false;

        // on récupère le point de tir du drone
        _droneBarrel = transform.gameObject.FindObjectByName("DroneBarrel");

        // on récupère le joueur auquel appartient le drone
        _player = this.gameObject.transform.GetChild(2).gameObject;

        // pour accéder au bouclier du joueur
        accessShield = this.GetComponent<PlayerHealth>();
    }

    /// <summary>
    /// Fonction faisant tourner le drone autour du joueur
    /// </summary>
    private void Update( )
    {
        drone.transform.RotateAround(_player.transform.position, Vector3.up, 80 * Time.deltaTime);
        drone.transform.Rotate(Vector3.up * Time.deltaTime, Space.Self);
    }

    /// <summary>
    /// Fonction détéctant si un objet entre dans le rayon de
    /// tir du drone
    /// </summary>
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Asteroid" || collider.tag == "Player")
        {
            DroneShoot(collider);
        }
    }

    /// <summary>
    /// Fonction de tir du drone
    /// </summary>
    private void DroneShoot(Collider collider)
    {
        if (Time.time > _lastShootingTimeRef)
        {
            Quaternion rot = _droneBarrel.transform.rotation * Quaternion.Euler(90, 0, 0);
            GameObject go = (GameObject)Instantiate(bullet, _droneBarrel.transform.position, rot);

            go.GetComponent<DroneBullet>().target = collider;
            
            go.GetComponent<MeshRenderer>().material = bulletMaterial;
            go.GetComponent<TrailRenderer>().material = bulletMaterial;

            go.transform.parent = this.transform.parent.gameObject.transform.Find("BulletStorage").transform;
            go.GetComponent<Rigidbody>().AddForce(_droneBarrel.transform.forward * _droneShootForce);

            _lastShootingTimeRef = Time.time + _droneShootRate;
        }
    }

    /// <summary>
    /// Fonction activant le drone du joueur lorsqu'il
    /// récupère le power-up correspondant
    /// </summary>
    public void PowerUpDrone()
    {
        if (accessShield.hasShield)
            accessShield.DesactivateShield();

        hasDrone = true;
        drone.SetActive(true);
        StartCoroutine(TimerDrone());
    }

    /// <summary>
    /// Fonction attendant 30 secondes avant de
    /// désactiver le drone
    /// </summary>
    private IEnumerator TimerDrone( )
    {
        // TODO: change value
        yield return new WaitForSeconds(30);

        drone.SetActive(false);
        hasDrone = false;
    }

    /// <summary>
    /// Fonction désactivant le drone du joueur
    /// </summary>
    public void DesactivateDrone( )
    {
        drone.SetActive(false);
        hasDrone = false;
    }
}
