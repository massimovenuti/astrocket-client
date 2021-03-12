using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpHeavyLaser : MonoBehaviour
{
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            collider.GetComponent<GunController>().PowerUpHeavyLaser();
            Destroy(this.gameObject);
        }
    }
}
