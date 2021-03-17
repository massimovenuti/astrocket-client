using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSlowness : MonoBehaviour
{
    private GameObject[] players;

    /// <summary>
    /// Quand le joueur entre dans la zone de trigger du power-up, appel de la fonction "PowerUpSlowness()" du script "GunController"
    /// et détruit le gameObject 
    /// Agit sur tout les tag "Player" sauf celui du joueur entrant dans le trigger
    /// </summary>
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
                if (player != collider.gameObject)
                    player.GetComponent<Movements>().PowerUpSlowness();

            Destroy(this.gameObject);
        }
    }
}
