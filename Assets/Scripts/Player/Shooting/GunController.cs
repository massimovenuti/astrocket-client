using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public GameObject bullet;
    public Transform barrel;

    public float shootForce;
    public float shootRate;

    private float lastShootingTimeRef;

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetMouseButtonDown(0)) 
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if(Time.time > lastShootingTimeRef) 
        {
            GameObject go = (GameObject)Instantiate(bullet, barrel.position, barrel.rotation);
            go.GetComponent<Rigidbody>().AddForce(barrel.forward * shootForce);
            lastShootingTimeRef = Time.time + shootRate;
        }
    }
}
