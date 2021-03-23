using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpFlash : MonoBehaviour
{
    /// <summary>
    /// Quand le joueur entre dans la zone de trigger du power-up, appel de la fonction "PowerUpFlasho()" du script "Movements"
    /// et détruit le gameObject 
    /// </summary>
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            collider.GetComponent<Movements>().PowerUpFlash();
            Destroy(this.gameObject);
        }
    }
}
