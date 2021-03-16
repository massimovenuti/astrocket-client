using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public Health playerHealth;
    public int shieldDurability;

    public GameObject shield;
    public PlayerDrone accessDrone;
    public GameObject ui;

    // TODO: changes values
    private int _playerHealth = 100;

    private int _healValue = 40;

    private int _shieldDurabilityMax = 8;
    private int _shieldDurability;

    private int _damageBullet = 20;
    private int _damageRocket = 40;
    private int _damageHeavyLaser = 40;
    private int _damageCollisionAsteroid = 25;
    private int _damageCollisionPlayer = 25;
    private int _damageExplosion = 30;
    private int _damageMine = 50;
    private int _damageValue;

    public bool hasShield;
    private bool _isFantome;
    private bool _isHacked;

    // Start is called before the first frame update
    private void Start()
    {
        playerHealth = new Health(_playerHealth);

        // DEBUG
        Debug.Log("Health : " + playerHealth.GetHealth());

        shield = GameObject.Find("Shield");
        shield.SetActive(false);
        hasShield = false;
        shieldDurability = 0;

        _isFantome = false;
        _isHacked = false;

        accessDrone = this.GetComponent<PlayerDrone>();
        ui = this.transform.parent.Find("CanvasPowerUp").GetComponent<Canvas>().gameObject;
        ui.SetActive(false);
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
            this.GetComponent<PlayerDrone>().DesactivateDrone();
            DesactivateShield();
            if (_isFantome)
            {
                this.GetComponent<BoxCollider>().enabled = true;
                _isFantome = false;
            }
            if (_isHacked)
            {
                ui.SetActive(false);
                _isHacked = false;
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

            _damageValue = _damageBullet;
            dealDamage(_damageValue);

            Destroy(collision.gameObject);
        }

        // touché par une rocket
        if (collision.gameObject.tag == "Rocket")
        {
            // DEBUG
            Debug.Log("Touched by a rocket");

            _damageValue = _damageRocket;
            dealDamage(_damageValue);

            Destroy(collision.gameObject);
        }

        if(collision.gameObject.tag == "HeavyLaser")
        {
            // DEBUG
            Debug.Log("Touched by a heavy laser");
            _damageValue = _damageHeavyLaser;
            dealDamage(_damageValue);

            Destroy(collision.gameObject);
        }

        // collision avec un astéroide
        if (collision.gameObject.tag == "Asteroid")
        {
            // DEBUG
            Debug.Log("Collision with an asteroid");

            _damageValue = _damageCollisionAsteroid;
            dealDamage(_damageValue);

            collision.gameObject.GetComponent<DestroyAsteroid>().DestructionAsteroid();
        }

        // collision avec un joueur
        if (collision.gameObject.tag == "Player")
        {
            // DEBUG
            Debug.Log("Collision with a player");

            _damageValue = _damageCollisionPlayer;
            dealDamage(_damageValue);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        // collision avec un astéroide
        if (collision.gameObject.tag == "Mine")
        {
            // DEBUG
            Debug.Log("Collision with a mine");

            _damageValue = _damageMine;
            dealDamage(_damageValue);

            Destroy(collision.gameObject);
        }
    }

    // Fonction diminuant la vie du joueur
    // quand il est affecté par une explosion
    public void ExplosionDamage()
    {
        // DEBUG
        Debug.Log("Affected by an explosion");

        _damageValue = _damageExplosion;
        dealDamage(_damageValue);
    }

    // Fonction générale diminuant la vie du joueur
    // en gérant le bouclier
    private void dealDamage(int damageValue)
    {
        if (shieldDurability <= 0)
            playerHealth.Damage(damageValue);
        else
        {
            playerHealth.Damage(damageValue / 2);
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

        if (accessDrone.hasDrone)
            accessDrone.DesactivateDrone();

        hasShield = true;
        shieldDurability = _shieldDurabilityMax;
        shield.SetActive(true);
    }

    public void PowerUpFantome( )
    {
        // DEBUG
        Debug.Log("Fantome");

        _isFantome = true;
        this.GetComponent<BoxCollider>().enabled = false;

        StartCoroutine(TimerFantome());
    }

    public void PowerUpJammer( )
    {
        // DEBUG
        Debug.Log("Jammer");

        _isHacked = true;
        ui.SetActive(true);

        StartCoroutine(TimerJammer());
    }

    public void DesactivateShield( )
    {
        if (shieldDurability > 0)
        {
            hasShield = false;
            shield.SetActive(false);
            shieldDurability = 0;
        }
    }

    // Fonction attendant 5 secondes avant de
    // désactiver le power-up fantome
    private IEnumerator TimerFantome( )
    {
        // TODO: change value /!\ OP
        yield return new WaitForSeconds(5);

        this.GetComponent<BoxCollider>().enabled = true;
        _isFantome = false;
    }

    // Fonction attendant 5 secondes avant de
    // désactiver le power-up jammer
    private IEnumerator TimerJammer( )
    {
        yield return new WaitForSeconds(10);

        ui.SetActive(false);
        _isHacked = false;
    }
}
