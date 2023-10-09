using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class PlayerFearSystem : MonoBehaviour
{
    [SerializeField] private float healthRefillSpeed = 0.5f;
    private HealthSystem playerHealthSystem;

    private Torch playerTorch;
    private bool isUsingTorch = false;

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
        playerHealthSystem = GetComponent<HealthSystem>();
        playerTorch = FindObjectOfType<Torch>();   
       
    }

    public void IncreaseFearLevel(float level)
    {
        if (playerHealthSystem == null) return;

        if (!isHealing && !isUsingTorch)
        {
            playerHealthSystem.DecreaseHealth(level);
        }
    }

    public void SetIsHealing(bool isHealing)
    {
        this.isHealing = isHealing;   // Can Now Start Recovering Health
    }

    private void Update()
    {
        isUsingTorch = playerTorch.IsActive();

        if (isHealing || isUsingTorch)
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
        float playerHealth = playerHealthSystem.GetHealth(); // Initialize with the current health

        playerHealth += Time.deltaTime * healthRefillSpeed;

        if(playerHealth > playerHealthSystem.GetMaxHealth())
        {
            isHealing = false;                           // Stop Healing
        }

        playerHealthSystem.SetHealth(playerHealth); // Update the health
    }

   
}
