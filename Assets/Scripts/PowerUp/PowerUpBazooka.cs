using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBazooka : MonoBehaviour
{

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            collider.GetComponent<GunController>().PowerUpBazooka();
            Destroy(this.gameObject);
        }
    }
}
