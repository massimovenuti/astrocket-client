using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Luminosity.IO;


public class GunController : MonoBehaviour
{
    public GameObject bullet;

    public float shootRate = 0.2f;
    public float shootForce = 3000;

    private GameObject _barrel;
    private GameObject _bulletStorage;
    private float lastShootingTimeRef;
    [SerializeField]
    private string InputManagerShootAxis = $"Shoot";

    private void Start( )
    {
        _barrel = gameObject.FindObjectByName("Barrel");
        _bulletStorage = GameObject.Find("BulletStorage");
    }

    // Update is called once per frame
    private void Update()
    {
        if(InputManager.GetButtonDown(InputManagerShootAxis)) 
            Shoot();
    }

    private void Shoot()
    {
        if(Time.time > lastShootingTimeRef) 
        {
            GameObject go = Instantiate(bullet, _barrel.transform.position, _barrel.transform.rotation);
            go.transform.parent = _bulletStorage.transform;
            go.GetComponent<Rigidbody>().AddForce(_barrel.transform.forward * shootForce);
            lastShootingTimeRef = Time.time + shootRate;
        }
    }
}
