using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Luminosity.IO;


public class GunController : MonoBehaviour
{
    public GameObject bullet;
    public GameObject barrel;
    public GameObject bulletSpawn;


    public float shootForce = 2000;
    public float shootRate = 0.1f;

    [SerializeField]
    private string InputManagerShootAxis = $"Shoot";
    private float lastShootingTimeRef;

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
            GameObject go = Instantiate(bullet, barrel.transform.position, barrel.transform.rotation);
            go.transform.parent = bulletSpawn.transform;
            go.GetComponent<Rigidbody>().AddForce(barrel.transform.forward * shootForce);
            lastShootingTimeRef = Time.time + shootRate;
        }
    }
}
