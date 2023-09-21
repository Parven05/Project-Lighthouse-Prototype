using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float maxHealth = 100f;

    private void Awake()
    {
        health = maxHealth;
    }

    public float GetHealth()
    {
        return health;
    }

    public void AddHealth(float amount)
    {
        health += amount;
    }

    public void DecreaseHealth(float amount)
    {
        health -= amount;
    }
}
