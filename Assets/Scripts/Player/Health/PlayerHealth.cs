using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;
using Mirror;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] int maxPlayerHealth = 100;

    [SerializeField] Slider slider;
    [SerializeField] Gradient gradient;
    [SerializeField] Image fill;

    public int shieldDurability;

    public GameObject shield;
    public PlayerDrone accessDrone;
    public GameObject ui;

    private int _healValue = 40;

    private int _shieldDurabilityMax = 2;
    private int _shieldDurability;

    [SerializeField] int bulletDmg = 20;
    [SerializeField] int asteroidDmgRate = 10;
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

    [SyncVar(hook="OnHealthChange")]
    public int health;

    private bool isDead = false;

    private void Awake( )
    {
        slider.maxValue = maxPlayerHealth;
        slider.value = maxPlayerHealth;
    }

    public void Start( )
    {
        if (isServer)
        {
            this.health = this.maxPlayerHealth;
            hasShield = false;
            // initialise les booléens des power-ups 
            // (fantome et jammer) à faux
            _isFantome = false;
            _isHacked = false;
            shieldDurability = 0;
        }
 
        // récupère le bouclier du joueur et l'initialise
        shield = GameObject.Find("Shield");
        shield.SetActive(false);

        // pour accéder plus tard au drone du joueur
        accessDrone = this.GetComponent<PlayerDrone>();

        /*
        if (isLocalPlayer)
        {
            // récupère l'UI du power-up jammer et la désactive
            ui = GameObject.Find("PowerUpUI");
            ui.SetActive(false);
        }
        */
    }

    /// <summary>
    /// Fonction générale diminuant la vie du joueur
    /// en gérant le bouclier
    /// </summary>
    [Server]
    public void Damage(int damageValue)
    {
        if (shieldDurability <= 0)
        {
            health -= damageValue;
        }
        else
        {
            health -= damageValue/2;
            shieldDurability--;
            if (shieldDurability <= 0)
            {
                RpcActiveShield(false);
                hasShield = false;
            }
        }
        if (health <= 0)
        {
            health = 0;
            isDead = true;
        }
    }

    // Fonction augmentant la vie d'un objet
    [Server]
    public void Heal(int healValue)
    {
        health += healValue;
        if (health > maxPlayerHealth)
            health = maxPlayerHealth;
    }

    // Fonction diminuant la vie du joueur lorsqu'il est touché par quelque chose
    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        GameObject go = other.gameObject;

        // touché par une bullet
        if (other.CompareTag("Bullet") && go.GetComponent<Ammo>().ownerId != netId)
        {
            _damageValue = _damageBullet;
            Damage(_damageValue);
            NetworkServer.Destroy(go);
        }
        // touché par une roquette
        else if (other.CompareTag("Rocket") && go.GetComponent<Ammo>().ownerId != netId)
        {
            Debug.LogWarning("(PlayerHealth) Touched by rocket " + go.GetComponent<Ammo>().ownerId);
            _damageValue = _damageRocket;
            Damage(_damageValue);
            NetworkServer.Destroy(go);
        }
        // touché par un heavy laser (power-up)
        else if (other.CompareTag("HeavyLaser") && go.GetComponent<Ammo>().ownerId != netId)
        {
            _damageValue = _damageHeavyLaser;
            Damage(_damageValue);
            NetworkServer.Destroy(go);
        }
        else if (other.CompareTag("Asteroid"))
        {
            Damage(go.GetComponent<Asteroid>().GetSize() * asteroidDmgRate);
            NetworkServer.Destroy(go);
            RpcResetVelocity();
        }
        else if (other.CompareTag("Mine"))
        {
            _damageValue = _damageMine;
            Damage(_damageValue);
            NetworkServer.Destroy(go);
        }

        if (isDead)
        {
            Revive();
        }
    }

    [ServerCallback]
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("AbsoluteBorder"))
        {
            Revive();
        }
    }

    [Server]
    public void Revive( )
    {
        // le joueur est mort, un script va le
        // désactiver pendant 2 secondes
        GetComponent<PlayerRespawn>().Respawn();

        // réinitialise l'inertie
        RpcResetVelocity();

        // réinitialise la vie, et indique que
        // le joueur est à nouveau vivant
        this.health = this.maxPlayerHealth;
        this.isDead = false;

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
    }

    [ClientCallback]
    void OnHealthChange(int oldValue, int newValue)
    {
        if(isLocalPlayer)
        {
            slider.value = newValue;
        }
    }

    /*
    private void OnCollisionEnter(Collider collision)
    {
        // collision avec un joueur
        if (collision.gameObject.tag == "Player")
        {
            _damageValue = _damageCollisionPlayer;
            Damage(_damageValue);
        }
    }
    */

    /// <summary>
    /// Fonction appelant Damage quand le
    /// joueur est affecté par une explosion
    /// </summary>
    [Server]
    public void ExplosionDamage()
    {
        _damageValue = _damageExplosion;
        Damage(_damageValue);
        if (isDead)
        {
            Revive();
        }
    }

    /// <summary>
    /// Fonction augmantant la vie du joueur
    /// </summary>
    [Server]
    public void PowerUpMedikit()
    {
        Heal(_healValue);
    }

    /// <summary>
    /// Fonction activant le bouclier du joueur
    /// </summary>
    public void PowerUpShield()
    {
        /*if (accessDrone.hasDrone)
        {
            accessDrone.DesactivateDrone();
        }*/

        hasShield = true;
        shieldDurability = _shieldDurabilityMax;
        RpcActiveShield(true);
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
    [Server]
    public void DesactivateShield( )
    {
        if (shieldDurability > 0)
        {
            hasShield = false;
            shieldDurability = 0;
            RpcActiveShield(false);
        }
    }

    [ClientRpc]
    private void RpcActiveShield(bool active)
    {
        shield.SetActive(active);
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
    
    [TargetRpc]
    private void RpcResetVelocity()
    {
        if (isLocalPlayer)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
