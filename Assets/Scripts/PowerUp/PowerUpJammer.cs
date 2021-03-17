using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpJammer : MonoBehaviour
{
    private GameObject[] players;

    /// <summary>
    /// Quand le joueur entre dans la zone de trigger du power-up, appel de la fonction "PowerUpJammer()" dans le script "PlayerHealth"
    /// et détruit le gameObject 
    /// </summary>
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
                if (player != collider.gameObject)
                    player.GetComponent<PlayerHealth>().PowerUpJammer();

            Destroy(this.gameObject);
        }
    }
}
