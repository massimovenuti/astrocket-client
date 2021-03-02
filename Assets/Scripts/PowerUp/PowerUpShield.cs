using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpShield : MonoBehaviour
{

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            collider.GetComponent<PlayerHealth>().PowerUpShield();
            Destroy(this.gameObject);
        }
    }
}
