using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpDrone : MonoBehaviour
{
    /// <summary>
    /// Quand le joueur entre dans la zone de trigger du power-up, appel de la fonction "PowerUpDrone()" du script "PlayerDrone"
    /// et détruit le gameObject 
    /// </summary>
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            collider.GetComponent<PlayerDrone>().PowerUpDrone();
            Destroy(this.gameObject);
        }
    }
}
