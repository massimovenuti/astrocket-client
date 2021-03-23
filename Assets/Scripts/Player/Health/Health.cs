using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health
{

    private int health;
    private int healthMax;
    private bool isDead;

    /// <summary>
    /// Fonction initialisant la vie d'un objet
    /// </summary>
    public Health(int healthMax) 
    {
        this.healthMax = healthMax;
        health = healthMax;
        isDead = false;
    }

    /// <summary>
    /// Fonction retournant la vie d'un objet
    /// </summary>
    public int GetHealth() 
    {
        return health;
    }

    /// <summary>
    /// Fonction changeant la valeur de la vie
    /// d'un objet
    /// </summary>
    public void SetHealth(int health)
    {
        this.health = health;
    }

    /// <summary>
    /// Fonction retournant true si l'objet
    /// est "mort" et false sinon
    /// </summary>
    public bool GetDead()
    {
        return isDead;
    }

    /// <summary>
    /// Fonction changeant l'état de mort de
    /// l'objet
    /// </summary>
    public void SetDead(bool isDead)
    {
        this.isDead = isDead;
    }

    /// <summary>
    /// Fonction diminuant la vie d'un objet
    /// </summary>
    public void Damage(int damageValue) 
    {
        health -= damageValue;
        if (health <= 0)
        {
            health = 0;
            isDead = true;
        }
    }

    /// <summary>
    /// Fonction augmentant la vie d'un objet
    /// </summary>
    public void Heal(int healValue) 
    {
        health += healValue;
        if (health > healthMax)
            health = healthMax;
    }
}
