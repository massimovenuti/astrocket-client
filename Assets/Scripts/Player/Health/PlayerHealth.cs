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

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    private void Start()
    {
        playerHealth = new Health(_playerHealth);

        // récupère le bouclier du joueur et l'initialise
        shield = GameObject.Find("Shield");
        shield.SetActive(false);
        hasShield = false;
        shieldDurability = 0;

        // initialise les booléens des power-ups 
        // (fantome et jammer) à faux
        _isFantome = false;
        _isHacked = false;

        // pour accéder plus tard au drone du joueur
        accessDrone = this.GetComponent<PlayerDrone>();

        // récupère l'UI du power-up jammer et la désactive
        ui = this.transform.parent.Find("CanvasPowerUp").GetComponent<Canvas>().gameObject;
        ui.SetActive(false);
    }

    /// <summary>
    /// Fonction Update, appelée à chaque frame
    /// </summary>
    private void Update()
    {
        // si le joueur est mort
        if (playerHealth.GetDead())
        {
            // réinitialise les power-ups
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

            // réinitialise la vie, et indique que
            // le joueur est à nouveau vivant
            playerHealth.SetDead(false);
            playerHealth.SetHealth(_playerHealth);

            // réinitialise l'inertie
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    /// <summary>
    /// Fonction diminuant la vie du joueur lorsqu'il
    /// est touché par quelque chose
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        // touché par un laser
        if (collision.gameObject.tag == "Bullet")
        {
            _damageValue = _damageBullet;
            dealDamage(_damageValue);

            Destroy(collision.gameObject);
        }

        // touché par une roquette
        if (collision.gameObject.tag == "Rocket")
        {
            _damageValue = _damageRocket;
            dealDamage(_damageValue);

            Destroy(collision.gameObject);
        }

        // touché par un heavy laser (power-up)
        if(collision.gameObject.tag == "HeavyLaser")
        {
            _damageValue = _damageHeavyLaser;
            dealDamage(_damageValue);

            Destroy(collision.gameObject);
        }

        // collision avec un astéroide
        if (collision.gameObject.tag == "Asteroid")
        {
            _damageValue = _damageCollisionAsteroid;
            dealDamage(_damageValue);

            collision.gameObject.GetComponent<DestroyAsteroid>().DestructionAsteroid();
        }

        // collision avec un joueur
        if (collision.gameObject.tag == "Player")
        {
            _damageValue = _damageCollisionPlayer;
            dealDamage(_damageValue);
        }
    }

    /// <summary>
    /// Fonction gérant l'entrée du joueur dans un
    /// trigger: sert uniquement pour la mine
    /// </summary>
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Mine")
        {
            _damageValue = _damageMine;
            dealDamage(_damageValue);

            Destroy(collision.gameObject);
        }
    }

    /// <summary>
    /// Fonction appelant dealDamage quand le
    /// joueur est affecté par une explosion
    /// </summary>
    public void ExplosionDamage()
    {
        _damageValue = _damageExplosion;
        dealDamage(_damageValue);
    }

    /// <summary>
    /// Fonction générale diminuant la vie du joueur
    /// en gérant le bouclier
    /// </summary>
    private void dealDamage(int damageValue)
    {
        if (shieldDurability <= 0)
            playerHealth.Damage(damageValue);
        else
        {
            playerHealth.Damage(damageValue / 2);
            shieldDurability--;
            if (shieldDurability <= 0)
            {
                shield.SetActive(false);
                hasShield = false;
            }
        }
    }

    /// <summary>
    /// Fonction augmantant la vie du joueur
    /// </summary>
    public void PowerUpMedikit()
    {
        playerHealth.Heal(_healValue);
    }

    /// <summary>
    /// Fonction activant le bouclier du joueur
    /// </summary>
    public void PowerUpShield()
    {
        if (accessDrone.hasDrone)
            accessDrone.DesactivateDrone();

        hasShield = true;
        shieldDurability = _shieldDurabilityMax;
        shield.SetActive(true);
    }

    /// <summary>
    /// Fonction activant le power-up fantome
    /// </summary>
    public void PowerUpFantome( )
    {
        _isFantome = true;
        this.GetComponent<BoxCollider>().enabled = false;

        StartCoroutine(TimerFantome());
    }

    /// <summary>
    /// Fonction activant l'UI du malus jammer
    /// </summary>
    public void PowerUpJammer( )
    {
        _isHacked = true;
        ui.SetActive(true);

        StartCoroutine(TimerJammer());
    }

    /// <summary>
    /// Fonction réinitialisant à 0 et désactivant
    /// le shield du joueur
    /// </summary>
    public void DesactivateShield( )
    {
        if (shieldDurability > 0)
        {
            hasShield = false;
            shield.SetActive(false);
            shieldDurability = 0;
        }
    }

    /// <summary>
    /// Fonction attendant 5 secondes avant de
    /// désactiver le power-up fantome
    /// </summary>
    private IEnumerator TimerFantome( )
    {
        // OP /!\
        yield return new WaitForSeconds(5);

        this.GetComponent<BoxCollider>().enabled = true;
        _isFantome = false;
    }

    /// <summary>
    /// Fonction attendant 10 secondes avant de
    /// désactiver le power-up jammer
    /// </summary>
    private IEnumerator TimerJammer( )
    {
        // TODO: change value
        yield return new WaitForSeconds(10);

        ui.SetActive(false);
        _isHacked = false;
    }
}
