using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpDrone : MonoBehaviour
{

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            collider.GetComponent<PlayerDrone>().PowerUpDrone();
            Destroy(this.gameObject);
        }
    }
}
