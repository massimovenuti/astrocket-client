using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpAkimbo : MonoBehaviour
{

    /// <summary>
    /// Quand le joueur entre dans la zone de trigger du power-up, appel de la fonction "PowerUpAkimbo()" du script "GunController"
    /// et détruit le gameObject 
    /// </summary>
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            collider.GetComponent<GunController>().PowerUpAkimbo();
            Destroy(this.gameObject);
        }
    }
}
