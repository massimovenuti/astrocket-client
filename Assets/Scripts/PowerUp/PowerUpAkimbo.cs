using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpAkimbo : MonoBehaviour
{

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            // DEBUG
            Debug.Log("Akimbo");

            collider.SendMessage("PowerUpAkimbo");
            Destroy(this.gameObject);
        }
    }
}
