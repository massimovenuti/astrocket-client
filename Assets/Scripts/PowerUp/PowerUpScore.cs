using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScore : MonoBehaviour
{
    /// <summary>
    /// Quand le joueur entre dans la zone de trigger du power-up, appel de la fonction "PowerUpBonus()" dans le script ???
    /// et détruit le gameObject 
    /// </summary>
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            // TODO: implement bonus
            Destroy(this.gameObject);
        }
    }
}
