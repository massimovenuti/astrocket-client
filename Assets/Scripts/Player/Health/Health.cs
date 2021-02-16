using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health
{
    public event EventHandler GetDamage;

    private int health;
    private int healthMax;

    // Fonction initialisant la vie d'une instance
    public Health(int healthMax) {
        this.healthMax = healthMax;
        health = healthMax;
    }

    // Fonction retournant la vie d'une instance
    public int GetHealth() {
        return health;
    }

    // Fonction diminuant la vie d'une instance
    public void Damage(int damageValue) {
        health -= damageValue;
        if (health < 0)
            health = 0;
        if (GetDamage != null)
            GetDamage(this, EventArgs.Empty);

        // DEBUG
        Debug.Log("Health : " + GetHealth());
        if (health == 0)
            Debug.Log("It's dead :(");
    }

    // Fonction augmentant la vie d'une instance
    public void Heal(int healValue) {
        health += healValue;
        if (health > healthMax)
            health = healthMax;

        // DEBUG
        Debug.Log("Health : " +  GetHealth());
    }
}
