using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpFlash : MonoBehaviour
{

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            collider.GetComponent<Movements>().PowerUpFlash();
            Destroy(this.gameObject);
        }
    }
}
