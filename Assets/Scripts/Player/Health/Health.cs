using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health
{

    private int health;
    private int healthMax;
    private bool isDead;

    // Fonction initialisant la vie d'un objet
    public Health(int healthMax) {
        this.healthMax = healthMax;
        health = healthMax;
        isDead = false;
    }

    // Fonction retournant la vie d'un objet
    public int GetHealth() {
        return health;
    }

    // Fonction changeant la valeur de la vie
    // d'un objet
    public void SetHealth(int health)
    {
        this.health = health;
    }

    // Fonction retournant true si l'objet
    // est "mort" et false sinon
    public bool GetDead()
    {
        return isDead;
    }

    // Fonction changeant l'état de mort de
    // l'objet
    public void SetDead(bool isDead)
    {
        this.isDead = isDead;
    }

    // Fonction diminuant la vie d'un objet
    public void Damage(int damageValue) {
        health -= damageValue;
        if (health <= 0)
        {
            health = 0;
            isDead = true;
        }

        // DEBUG
        Debug.Log("Health : " + GetHealth());
    }

    // Fonction augmentant la vie d'un objet
    public void Heal(int healValue) {
        health += healValue;
        if (health > healthMax)
            health = healthMax;

        // DEBUG
        Debug.Log("Health : " +  GetHealth());
    }
}
