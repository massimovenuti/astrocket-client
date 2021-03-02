using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public Health playerHealth;
    public int shieldDurability;

    public GameObject shield;

    // TODO: changes values
    private int _playerHealth = 100;

    private int _healValue = 40;

    private int _shieldDurabilityMax = 8;
    private int _shieldDurability;

    private int _damageBullet = 20;
    private int _damageRocket = 40;
    private int _damageCollisionAsteroid = 25;
    private int _damageCollisionPlayer = 25;
    private int _damageExplosion = 30;

    // Start is called before the first frame update
    private void Start()
    {
        playerHealth = new Health(_playerHealth);

        // DEBUG
        Debug.Log("Health : " + playerHealth.GetHealth());

        shieldDurability = 0;
        shield = GameObject.Find("Shield");
        shield.SetActive(false);
    }

    // Fonction Update, appelée à chaque frame
    private void Update()
    {
        // si le joueur est mort
        if (playerHealth.GetDead())
        {
            // DEBUG
            Debug.Log("The player is dead");

            // Réinitialise les power-ups
            this.GetComponent<Movements>().ResetPowerUps();
            this.GetComponent<GunController>().ResetPowerUps();
            if (shieldDurability > 0)
            {
                shield.SetActive(false);
                shieldDurability = 0;
            }

            // le joueur est mort, un script va le
            // désactiver pendant 2 secondes
            GameObject handler = GameObject.Find("Map");
            handler.SendMessage("SwitchPlayerActivation", gameObject);

            // Réinitialise la vie, et indique que
            // le joueur est à nouveau vivant
            playerHealth.SetDead(false);
            playerHealth.SetHealth(_playerHealth);

            // Réinitialise l'inertie
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

            if (shieldDurability <= 0)
                playerHealth.Damage(_damageBullet);

            else
            {
                playerHealth.Damage(_damageBullet / 2);
                shieldDurability--;
                if (shieldDurability <= 0)
                    shield.SetActive(false);
            }

            Destroy(collision.gameObject);
        }

        // touché par une rocket
        if (collision.gameObject.tag == "Rocket")
        {
            // DEBUG
            Debug.Log("Touched by a rocket");

            if (shieldDurability <= 0)
                playerHealth.Damage(_damageRocket);

            // TODO: voir comportement des rockets sur le bouclier
            else
            {
                playerHealth.Damage(_damageRocket / 2);
                shieldDurability -= 2;
                if (shieldDurability <= 0)
                    shield.SetActive(false);
            }

            Destroy(collision.gameObject);
        }

        // collision avec un astéroide
        if (collision.gameObject.tag == "Asteroid")
        {
            // DEBUG
            Debug.Log("Collision with an asteroid");

            if (shieldDurability <= 0)
                playerHealth.Damage(_damageCollisionAsteroid);

            else
            {
                playerHealth.Damage(_damageCollisionAsteroid / 2);
                shieldDurability--;
                if (shieldDurability <= 0)
                    shield.SetActive(false);
            }

            collision.gameObject.GetComponent<DestroyAsteroid>().DestructionAsteroid();
        }

        // collision avec un joueur
        if (collision.gameObject.tag == "Player")
        {
            // DEBUG
            Debug.Log("Collision with a player");

            if (shieldDurability <= 0)
                playerHealth.Damage(_damageCollisionPlayer);

            else
            {
                playerHealth.Damage(_damageCollisionPlayer / 2);
                shieldDurability--;
                if (shieldDurability <= 0)
                    shield.SetActive(false);
            }
        }
    }

    // Fonction diminuant la vie du joueur
    // quand il est affecté par une explosion
    private void ExplosionDamage()
    {
        // DEBUG
        Debug.Log("Affected by an explosion");

        if (shieldDurability <= 0)
            playerHealth.Damage(_damageExplosion);
        else
        {
            playerHealth.Damage(_damageExplosion / 2);
            shieldDurability--;
            if (shieldDurability <= 0)
                shield.SetActive(false);
        }
    }

    // Fonction augmantant la vie du joueur
    public void PowerUpMedikit()
    {
        // DEBUG
        Debug.Log("Medikit");

        playerHealth.Heal(_healValue);
    }

    // Fonction donnant un bouclier au joueur
    public void PowerUpShield()
    {
        // DEBUG
        Debug.Log("Shield");

        shieldDurability = _shieldDurabilityMax;
        shield.SetActive(true);
    }
}
