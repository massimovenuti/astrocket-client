using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpHomingMissile : MonoBehaviour
{
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            collider.GetComponent<GunController>().PowerUpHomingMissile();
            Destroy(this.gameObject);
        }
    }
}
