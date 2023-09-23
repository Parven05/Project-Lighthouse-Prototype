using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFearSystem : MonoBehaviour
{
    [SerializeField] private float healthRefillSpeed = 0.5f;
    private HealthSystem healthSystem;
    private bool canHealHealth;
    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
    }

    public void IncreaseFearLevel(float level)
    {
        if (healthSystem != null)
        {
            healthSystem.DecreaseHealth(level);
        }
    }

    public void NormalizeFearLevel()
    {
        canHealHealth = true;
    }

    private void Update()
    {
        if (canHealHealth)
        {
            HealHealth();
        }
    }

    private void HealHealth()
    {
        float clampedHealth = healthSystem.GetHealth(); // Initialize with the current health

        clampedHealth += Time.deltaTime * healthRefillSpeed;
        clampedHealth = Mathf.Clamp(clampedHealth, 0, healthSystem.GetMaxHealth());

        healthSystem.SetHealth(clampedHealth); // Update the health
    }
}
