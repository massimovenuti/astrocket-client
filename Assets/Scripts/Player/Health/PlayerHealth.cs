﻿using System.Collections;
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

    public GameObject shield;
    public PlayerDrone accessDrone;
    public GameObject ui;

    private int _healValue = 40;

    private int _shieldDurabilityMax = 2;
    public int shieldDurability;

    [SerializeField] int asteroidDmgRate = 10;
    private int _damageBullet = 20;
    private int _damageRocket = 40;
    private int _damageHeavyLaser = 40;
    private int _damageHomingMissile = 20;
    private int _damageExplosion = 30;
    private int _damageMine = 50;
    private int _damageValue;

    [SyncVar(hook = "OnShieldChange")]
    public bool hasShield;

    [SyncVar(hook = "OnHealthChange")]
    public int health;

    private bool _isFantome;
    private bool _isHacked;

    private bool isDead = false;

    public GameObject DustVFX;


    private void Awake( )
    {
        // récupère le bouclier du joueur
        shield = transform.Find("Shield").gameObject;

        slider.maxValue = maxPlayerHealth;
        slider.value = maxPlayerHealth;
    }


    public void Start( )
    {
        if (isServer)
        {
            this.health = this.maxPlayerHealth;
            // initialise les booléens des power-ups 
            // (fantome et jammer) à faux
            _isFantome = false;
            _isHacked = false;
            DesactivateShield();
        }

        // pour accéder plus tard au drone du joueur
        //accessDrone = this.GetComponent<PlayerDrone>();

        //if (isLocalPlayer)
        //{
        //    // récupère l'UI du power-up jammer et la désactive
        //    ui = GameObject.Find("PowerUpUI");
        //    ui.SetActive(false);
        //}
    }

    [ClientCallback]
    private void OnShieldChange(bool oldValue, bool newValue)
    {
        shield.SetActive(newValue);
    }

    [ClientCallback]
    private void OnEnable( )
    {
        shield.SetActive(false);
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
            health -= damageValue / 2;
            shieldDurability--;
            if (shieldDurability <= 0)
            {
                DesactivateShield();
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
        {
            health = maxPlayerHealth;
        }
    }

    // Fonction diminuant la vie du joueur lorsqu'il est touché par quelque chose
    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        GameObject go = other.gameObject;
        bool playerShoot = false;
        uint otherNetId = 0;

        // touché par une bullet
        if (other.CompareTag("Bullet") && go.GetComponent<Ammo>().ownerId != netId)
        {
            playerShoot = true;
            otherNetId = go.GetComponent<Ammo>().ownerId;
            _damageValue = _damageBullet;
            Damage(_damageValue);
            NetworkServer.Destroy(go);
        }
        // touché par une roquette
        else if (other.CompareTag("Rocket") && go.GetComponent<Ammo>().ownerId != netId)
        {
            playerShoot = true;
            otherNetId = go.GetComponent<Ammo>().ownerId;
            _damageValue = _damageRocket;
            Damage(_damageValue);
        }
        // touché par un heavy laser (power-up)
        else if (other.CompareTag("HeavyLaser") && go.GetComponent<Ammo>().ownerId != netId)
        {
            playerShoot = true;
            otherNetId = go.GetComponent<Ammo>().ownerId;
            _damageValue = _damageHeavyLaser;
            Damage(_damageValue);
            NetworkServer.Destroy(go);
        }
        // touché par un homing missile (power-up)
        else if (other.CompareTag("HomingMissile") && go.GetComponent<Ammo>().ownerId != netId)
        {
            playerShoot = true;
            otherNetId = go.GetComponent<Ammo>().ownerId;
            _damageValue = _damageHomingMissile;
            Damage(_damageValue);
            NetworkServer.Destroy(go);
        }
        else if (other.CompareTag("Asteroid"))
        {
            RpcParticuleDust(gameObject.transform.position);
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
            if (playerShoot)
            {
                NetworkIdentity.spawned[otherNetId].GetComponent<PlayerScore>().addKill();
            }

            GetComponent<PlayerScore>().addDeath();

            GetComponent<PlayerSpawn>().Respawn();
        }
    }

    [Server]
    public void Revive( )
    {
        // réinitialise la vie, et indique que le joueur est à nouveau vivant
        this.health = this.maxPlayerHealth;
        this.isDead = false;
    }

    [ClientCallback]
    void OnHealthChange(int oldValue, int newValue)
    {
        if (isLocalPlayer)
        {
            slider.value = newValue;
        }
    }

    /// <summary>
    /// Fonction appelant Damage quand le
    /// joueur est affecté par une explosion
    /// </summary>
    [Server]
    public void ExplosionDamage( )
    {
        _damageValue = _damageExplosion;
        Damage(_damageValue);
        if (isDead)
        {
            GetComponent<PlayerSpawn>().Respawn();
        }
    }

    /// <summary>
    /// Fonction augmantant la vie du joueur
    /// </summary>
    [Server]
    public void PowerUpMedikit( )
    {
        Heal(_healValue);
    }

    /// <summary>
    /// Fonction activant le bouclier du joueur
    /// </summary>
    public void PowerUpShield( )
    {
        //if (accessDrone.hasDrone)
        //{
        //    accessDrone.DesactivateDrone();
        //}
        hasShield = true;
        shieldDurability = _shieldDurabilityMax;
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
        hasShield = false;
        shieldDurability = 0;
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
        yield return new WaitForSeconds(2f);

        ui.SetActive(false);
        _isHacked = false;
    }

    [TargetRpc]
    private void RpcResetVelocity( )
    {
        if (isLocalPlayer)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    /// <summary>
    /// Instanciation de l'animation des particules de poussières quand un astéroide est dértuit
    /// </summary>
    [ClientRpc]
    private void RpcParticuleDust(Vector3 dustPosition)
    {
        GameObject dust = Instantiate(DustVFX, dustPosition, Quaternion.identity);
        ParticleSystem dustParticles = dust.transform.GetChild(0).GetComponent<ParticleSystem>();
        dustParticles.Play();
        float dustDuration = dustParticles.main.duration + dustParticles.main.startLifetimeMultiplier;
        Destroy(dust, dustDuration);
    }
}
