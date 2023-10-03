using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class PlayerFearSystem : MonoBehaviour
{
    [SerializeField] private float healthRefillSpeed = 0.5f;
    private HealthSystem healthSystem;
    private bool canHealHealth;
    private bool isHealing = false;

    public enum HealType
    {
        AutoHeal,
        ManualByInhaler,
        Both
    }
    [SerializeField] private HealType healType = HealType.Both; // By Default Both
    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        Healer.OnHealingStarted += Healer_OnHealingStarted;
    }

    private void Healer_OnHealingStarted(object sender, System.EventArgs e)
    {
        isHealing = true;
    }

    public void IncreaseFearLevel(float level)
    {
        if (healthSystem != null && !isHealing)
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
            if(healType == HealType.AutoHeal || healType == HealType.Both)
            {
                HealHealth();
                isHealing = false;   // While Healing Dont Fear
            }
            else if(healType == HealType.ManualByInhaler)
            {
                // HAndles by Others
                isHealing = false;   // While Healing Dont Fear
            }
           
        }
    }

    private void HealHealth()
    {
        float clampedHealth = healthSystem.GetHealth(); // Initialize with the current health

        clampedHealth += Time.deltaTime * healthRefillSpeed;
        clampedHealth = Mathf.Clamp(clampedHealth, 0, healthSystem.GetMaxHealth());

        healthSystem.SetHealth(clampedHealth); // Update the health
    }


    private void OnDisable()
    {
        Healer.OnHealingStarted -= Healer_OnHealingStarted;
    }
}
