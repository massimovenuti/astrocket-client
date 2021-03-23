using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpHomingMissile : MonoBehaviour
{
    /// <summary>
    /// Quand le joueur entre dans la zone de trigger du power-up, appel de la fonction "PowerUpHomingMissile()" du script "GunController"
    /// et détruit le gameObject 
    /// </summary>
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            collider.GetComponent<GunController>().PowerUpHomingMissile();
            Destroy(this.gameObject);
        }
    }
}
