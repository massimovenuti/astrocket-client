using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PowerUpJammer : NetworkBehaviour
{
    private GameObject[] players;

    /// <summary>
    /// Quand le joueur entre dans la zone de trigger du power-up, appel de la fonction "PowerUpJammer()" 
    /// dans le script "PlayerHealth" de tous les joueurs, excepté lui-même, puis détruit le gameObject 
    /// </summary>
    [ServerCallback]
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            collider.GetComponent<PlayerScore>().addPowerUP();

            players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
                if (player != collider.gameObject)
                    player.GetComponent<PlayerHealth>().PowerUpJammer();

            NetworkServer.Destroy(this.gameObject);
        }
    }
}
