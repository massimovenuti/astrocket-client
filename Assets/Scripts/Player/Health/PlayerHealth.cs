using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Mirror;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] int bulletDmg = 20;

    [SerializeField] int asteroidDmgRate = 10;

    [SerializeField] int maxPlayerHealth = 100;

    private int health;

    private bool isDead = false;


    [ServerCallback]
    public void Start( )
    {
        this.health = this.maxPlayerHealth;
    }

    // Fonction diminuant la vie d'un objet
    [Server]
    public void Damage(int damageValue)
    {
        health -= damageValue;
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

        if (other.CompareTag("Bullet") && go.GetComponent<Bullet>().ownerId != netId)
        {
            NetworkServer.Destroy(go);
            Damage(bulletDmg);
        }

        if (other.CompareTag("Asteroid"))
        {
            Damage(go.GetComponent<Asteroid>().GetSize() * asteroidDmgRate);
            NetworkServer.Destroy(go);
            RpcResetVelocity();
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
        GetComponent<PlayerRespawn>().Respawn();
        RpcResetVelocity();
        this.health = this.maxPlayerHealth;
        this.isDead = false;
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
