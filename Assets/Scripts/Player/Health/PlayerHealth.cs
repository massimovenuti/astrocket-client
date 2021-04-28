using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;
using Mirror;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] 
    int maxPlayerHealth = 100;

    [SerializeField] 
    private int _asteroidRateDamage, _damageBullet, _damageRocket, _damageExplosion, _healValue;

    [SerializeField]
    private Slider slider;

    [SerializeField]
    private Gradient gradient;

    [SerializeField]
    private Image fill;

    [SerializeField]
    private Image[] _shieldBar;

    [SerializeField]
    public GameObject shield, DustVFX;

    [SyncVar(hook = "OnHealthChange")]
    public int health;

    [SyncVar(hook = "OnShieldChange")]
    public bool hasShield;

    [SyncVar(hook = "OnShieldDurabilityChange")]
    public int shieldDurability;

    private bool isDead = false;

    private int _damageValue = 0, _shieldDurabilityMax = 3;

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
            DesactivateShield();
        } 
        else if (isLocalPlayer)
        {
            FillShieldBar(0);
        }
    }

    [ClientCallback]
    private void OnHealthChange(int oldValue, int newValue)
    {
        if (isLocalPlayer)
        {
            slider.value = newValue;
        }
    }

    [ClientCallback]
    private void OnShieldChange(bool oldValue, bool newValue)
    {
        shield.SetActive(newValue);
    }

    [ClientCallback]
    private void OnShieldDurabilityChange(int oldValue, int newValue)
    {
        if(isLocalPlayer)
        {
            Debug.Log("Changed Shield Durability : " +  oldValue + " -> " + newValue);
            FillShieldBar(newValue);
        }
    }

    [Client]
    private void FillShieldBar(int shieldValue)
    {
        for (int i = 0; i < _shieldBar.Length; i++)
        {
            _shieldBar[i].enabled = DisplayShieldBar(shieldValue, i);
        }
    }

    [Client]
    private bool DisplayShieldBar(int shieldValue, int shieldBarIdx)
    {
        return shieldValue > shieldBarIdx;
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

            if (health <= 0)
            {
                health = 0;
                isDead = true;
            }
        }
        else
        {
            shieldDurability--;
            if (shieldDurability <= 0)
            {
                DesactivateShield();
            }
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
        else if (other.CompareTag("Asteroid"))
        {
            RpcParticuleDust(gameObject.transform.position);
            Damage(go.GetComponent<Asteroid>().GetSize() * _asteroidRateDamage);
            NetworkServer.Destroy(go);
            RpcResetVelocity();
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
        hasShield = true;
        shieldDurability = _shieldDurabilityMax;
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
