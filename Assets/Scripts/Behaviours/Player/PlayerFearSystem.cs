using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class PlayerFearSystem : MonoBehaviour
{
    [SerializeField] private float healthRefillSpeed = 0.5f;
    private HealthSystem healthSystem;
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

    public void IncreaseFearLevel(float level)
    {
        if (healthSystem != null && !isHealing)
        {
            healthSystem.DecreaseHealth(level);
        }
    }

    public void SetIsHealing(bool isHealing)
    {
        this.isHealing = isHealing;   // Can Now Start Recovering Health
    }

    private void Update()
    {
        if (isHealing)
        {
            if(healType == HealType.AutoHeal || healType == HealType.Both)
            {
                HealHealth();
            }
            else if(healType == HealType.ManualByInhaler)
            {
                // HAndles by Others
            }
            
        }
       
    }

    private void HealHealth()
    {
        float playerHealth = healthSystem.GetHealth(); // Initialize with the current health

        playerHealth += Time.deltaTime * healthRefillSpeed;

        if(playerHealth >= healthSystem.GetMaxHealth())
        {
            playerHealth = healthSystem.GetMaxHealth();  // Dont Allow To OverFlow Health
            isHealing = false;                           // Stop Healing
        }

        //playerHealth = Mathf.Clamp(playerHealth, 0, healthSystem.GetMaxHealth());

        healthSystem.SetHealth(playerHealth); // Update the health
    }


}
