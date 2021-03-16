using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpJammer : MonoBehaviour
{
    private GameObject[] players;

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
                // if (player != collider.gameObject)   // TODO: remove comment
                    collider.GetComponent<PlayerHealth>().PowerUpJammer();

            Destroy(this.gameObject);
        }
    }
}
