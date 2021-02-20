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

    // Fonction Update, appelée à chaque frame
    void Update()
    {

        // si le joueur est mort
        if (playerHealth.GetDead())
        {
            // DEBUG
            Debug.Log("It's dead :(");

            // le joueur est mort, un script va le
            // désactiver pendant X secondes
            GameObject handler = GameObject.Find("RespawnHandler");
            handler.SendMessage("SwitchPlayerActivation", gameObject);

            // le joueur est téléporté à son point
            // de réapparition
            // TODO: change with Rayan's spawn system
            this.gameObject.transform.position = new Vector3(0, 2.9f, 0);

            // Réinitialise la vie, et indique que
            // le joueur est à nouveau vivant
            playerHealth.SetDead(false);
            playerHealth.SetHealth(100);
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

            // TODO: change value
            playerHealth.Damage(20);
            Destroy(collision.gameObject);
        }

        // TODO: collision with players and asteroids
    }
}
