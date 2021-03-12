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

    // Start is called before the first frame update
    private void Start( )
    {
        drone = GameObject.Find("Drone");
        drone.SetActive(false);
        hasDrone = false;

        _droneBarrel = transform.gameObject.FindObjectByName("DroneBarrel");

        _player = this.gameObject.transform.GetChild(2).gameObject;

        accessShield = this.GetComponent<PlayerHealth>();
    }

    private void Update( )
    {
        drone.transform.RotateAround(_player.transform.position, Vector3.up, 80 * Time.deltaTime);
        drone.transform.Rotate(Vector3.up * Time.deltaTime, Space.Self);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Asteroid" || collider.tag == "Player")
        {
            // DEBUG
            Debug.Log("Something enters the trigger");

            DroneShoot(collider);
        }
    }

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

    // Update is called once per frame
    public void PowerUpDrone()
    {
        // DEBUG
        Debug.Log("Drone");

        if (accessShield.hasShield)
            accessShield.DesactivateShield();

        hasDrone = true;
        drone.SetActive(true);
        StartCoroutine(TimerDrone());
    }

    // Fonction attendant 30 secondes avant de
    // désactiver le drone
    private IEnumerator TimerDrone( )
    {
        // TODO: change value
        yield return new WaitForSeconds(30);

        drone.SetActive(false);
        hasDrone = false;
    }

    public void DesactivateDrone( )
    {
        drone.SetActive(false);
        hasDrone = false;
    }
}
