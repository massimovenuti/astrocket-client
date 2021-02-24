using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpMitraillette : MonoBehaviour
{

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            // DEBUG
            Debug.Log("Mitraillette");

            collider.SendMessage("PowerUpNewShootRate");
            Destroy(this.gameObject);
        }
    }
}
