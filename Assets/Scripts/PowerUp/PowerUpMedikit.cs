using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpMedikit : MonoBehaviour
{

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            collider.GetComponent<PlayerHealth>().PowerUpMedikit();
            Destroy(this.gameObject);
        }
    }
}
