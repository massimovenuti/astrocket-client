using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpMedikit : MonoBehaviour
{
    // TODO: change value
    public int healValue = 30;

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            // DEBUG
            Debug.Log("Medikit");

            collider.SendMessage("PowerUpMedikit", healValue);
            Destroy(this.gameObject);
        }
    }
}
