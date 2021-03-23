using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpTP : MonoBehaviour
{

    /// <summary>
    /// Quand le joueur entre dans la zone de trigger du power-up, change la position du joueur en appellant la fonction "GetSafeRespawnPoint" du script "RespawnManager"
    /// et détruit le gameObject 
    /// </summary>
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
