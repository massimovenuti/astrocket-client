using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpFlash : MonoBehaviour
{
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            // DEBUG
            Debug.Log("Flash");

            collider.SendMessage("PowerUpFlash");
            Destroy(this.gameObject);
        }
    }
}
