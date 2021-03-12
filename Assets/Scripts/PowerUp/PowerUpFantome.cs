using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpFantome : MonoBehaviour
{

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            collider.GetComponent<PlayerHealth>().PowerUpFantome();
            Destroy(this.gameObject);
        }
    }
}
