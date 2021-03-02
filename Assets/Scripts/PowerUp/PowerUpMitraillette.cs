using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpMitraillette : MonoBehaviour
{

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            collider.GetComponent<GunController>().PowerUpNewShootRate();
            Destroy(this.gameObject);
        }
    }
}
