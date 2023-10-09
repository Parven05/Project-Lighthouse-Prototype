using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float maxHealth = 100f;
     private float minHealth = 0f;

    private void Awake()
    {
        health = maxHealth;
    }

    public float GetHealth()
    {
        return health;
    }

    public void SetHealth(float amount)  // Over Flow Limit Added
    {
        if(amount > maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health = amount;
        }
    }
    public void AddHealth(float amount) // Over Flow Limit Added
    {
        float newHealth = health + amount;

        if(newHealth > maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health = newHealth;
        }
    }

    public void DecreaseHealth(float amount)  // Over Flow Limit Added
    {
        float newHealth = health - amount;

        if(newHealth <= minHealth)
        {
            health = minHealth;
        }
        else
        {
            health -= amount;
        }
       
    }

    internal float GetMaxHealth()
    {
        return maxHealth;
    }
}
