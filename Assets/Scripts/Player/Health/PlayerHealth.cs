using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public Health playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = new Health(100);

        // DEBUG
        Debug.Log("Health : " + playerHealth.GetHealth());
    }

    // Fonction augmantant la vie d'un joueur lorsqu'il
    // récupère un power-up de soin
    public void PlayerHeal(int healValue)
    {
        // DEBUG
        Debug.Log("Picked up power-up");

        playerHealth.Heal(healValue);
    }

    // Fonction diminuant la vie du joueur lorsqu'il
    // est touché par un laser
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            // DEBUG
            Debug.Log("Touched by bullet");

            playerHealth.Damage(40);
        }
    }
}
