using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpTP : MonoBehaviour
{

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            GameObject handler = GameObject.Find("Map");
            collider.transform.position = handler.GetComponent<RespawnManager>().GetSafeRespawnPoint();

            Destroy(this.gameObject);
        }
    }
}
