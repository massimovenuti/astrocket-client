using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpAkimbo : MonoBehaviour
{

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            collider.GetComponent<GunController>().PowerUpAkimbo();
            Destroy(this.gameObject);
        }
    }
}
