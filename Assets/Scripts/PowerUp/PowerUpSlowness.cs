using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSlowness : MonoBehaviour
{
    private GameObject[] players;

    /// <summary>
    /// Quand le joueur entre dans la zone de trigger du power-up, appel de la fonction "PowerUpSlowness()"
    /// du script "Movements" de tous les joueurs, excepté lui-même, puis détruit le gameObject
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
