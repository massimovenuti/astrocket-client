using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpFantome : MonoBehaviour
{
    /// <summary>
    /// Quand le joueur entre dans la zone de trigger du power-up, appel de la fonction "PowerUpFantome()" du script "PlayerHealth"
    /// et détruit le gameObject 
    /// </summary>
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            collider.GetComponent<PlayerHealth>().PowerUpFantome();
            Destroy(this.gameObject);
        }
    }
}
