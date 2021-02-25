using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBazooka : MonoBehaviour
{
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            // DEBUG
            Debug.Log("Bazooka");

            collider.SendMessage("PowerUpBazooka");
            Destroy(this.gameObject);
        }
    }
}
