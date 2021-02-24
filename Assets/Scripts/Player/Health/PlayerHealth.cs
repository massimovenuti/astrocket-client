using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public Health playerHealth;
    public int shieldDurability;

    public GameObject shield;

    // Start is called before the first frame update
    private void Start()
    {
        playerHealth = new Health(100);

        // DEBUG
        Debug.Log("Health : " + playerHealth.GetHealth());

        shieldDurability = 0;
        shield = GameObject.Find("Shield");
        shield.SetActive(false);
    }

    // Fonction Update, appelée à chaque frame
    private void Update()
    {
        // DEBUG
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
            playerHealth.Damage(20);

        // si le joueur est mort
        if (playerHealth.GetDead())
        {
            // DEBUG
            Debug.Log("It's dead :(");

            // le joueur est mort, un script va le
            // désactiver pendant X secondes
            GameObject handler = GameObject.Find("Map");
            handler.SendMessage("SwitchPlayerActivation", gameObject);

            // Réinitialise la vie, et indique que
            // le joueur est à nouveau vivant
            playerHealth.SetDead(false);
            playerHealth.SetHealth(100);

            // reset l'inertie
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    // Fonction diminuant la vie du joueur lorsqu'il
    // est touché par quelque chose
    private void OnCollisionEnter(Collision collision)
    {
        // touché par un laser
        if (collision.gameObject.tag == "Bullet")
        {
            // DEBUG
            Debug.Log("Touched by a bullet");

            // TODO: change values
            if (shieldDurability <= 0)
                playerHealth.Damage(20);
            else
            {
                playerHealth.Damage(5);
                shieldDurability--;
            }
            Destroy(collision.gameObject);
        }

        // TODO: collision with players and asteroids
        // DEBUG
        if (collision.gameObject.tag == "Asteroid")
        {

            // TODO: change values
            if (shieldDurability <= 0)
                playerHealth.Damage(20);
            else
            {
                playerHealth.Damage(5);
                shieldDurability--;
                if (shieldDurability == 0)
                    shield.SetActive(false);
            }
            Destroy(collision.gameObject);
        }
    }

    // Fonction augmantant la vie du joueur
    private void PowerUpMedikit(int value)
    {
        playerHealth.Heal(value);
    }

    // Fonction donnant un bouclier au joueur
    private void PowerUpShield()
    {
        // TODO: change value
        shieldDurability = 5;
        shield.SetActive(true);
    }
}
