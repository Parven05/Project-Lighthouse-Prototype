using Micosmo.SensorToolkit.Example;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class PlayerFearSystem : MonoBehaviour
{
    public static float PLAYER_SPENDING_TIME_DARK;

    [SerializeField] private float playerFearLevel;
    [SerializeField] private float playerFearMin = 0 ;
    [SerializeField] private float playerFearMax = 100;
    [SerializeField] private float fearLevelDecreaseSpeed = 0.5f;
    [SerializeField] private float fearLevelIncreaseSpeed = 0.5f;

    private Torch playerTorch;
    private bool isUsingTorch = false;

    private bool isHealing = false;
    public enum HealType
    {
        AutoHeal,
        ManualByInhaler,
        Both
    }

    public enum PlayerFearState
    {
        Idle,
        Healing,
        OnFear,
        UsingTorch
    }

    public enum FearLevel
    {
        Low,
        Medium,
        High,
        Ultra
    }

    [SerializeField] private HealType healType = HealType.Both; // By Default Both
    private PlayerFearState fearState;
    private FearLevel fearLevel;

    private void Awake()
    {
        playerTorch = FindObjectOfType<Torch>();   
    }

    private void Update()
    {
        isUsingTorch = playerTorch.IsActive();

        if (isUsingTorch)
        {
            SetPlayerFearState(PlayerFearState.UsingTorch);
        }

        if (playerFearLevel > playerFearMin && fearState == PlayerFearState.Idle)
        {
            SetPlayerFearState(PlayerFearState.Healing);
        }

        if (playerFearLevel >= playerFearMax)
        {
            SetPlayerFearState(PlayerFearState.Idle);
        }

        if (fearState == PlayerFearState.Idle)
        {
            ReduceFearLevel(); // Decrease fear level when the player is idle
        }

        if (fearState == PlayerFearState.Healing)
        {
            ReduceFearLevel();
        }

        if (fearState == PlayerFearState.OnFear && fearState != PlayerFearState.UsingTorch && fearState != PlayerFearState.Healing)
        {
            IncreaseFearLevel();
        }

        if (fearState == PlayerFearState.UsingTorch)
        {
            ReduceFearLevel();
        }
    }


    public void IncreaseFearLevel()
    {
        if (!isHealing && !isUsingTorch)
        {
            float fearIncrease = Time.deltaTime * fearLevelIncreaseSpeed;

            if (fearLevel == FearLevel.Medium)
            {
                fearIncrease *= 2; // Modify as needed
            }
            else if (fearLevel == FearLevel.High)
            {
                fearIncrease *= 3; // Modify as needed
            }
            else if (fearLevel == FearLevel.Ultra)
            {
                fearIncrease *= 4; // Modify as needed
            }

            playerFearLevel = Mathf.Clamp(playerFearLevel + fearIncrease, playerFearMin, playerFearMax);

            // Track Spening Time On Dark
            PLAYER_SPENDING_TIME_DARK += Time.deltaTime;
            //Debug.Log("Player Sepending Time On Dark :" + PLAYER_SPENDING_TIME_DARK);
        }
    }

    private void ReduceFearLevel()
    {
        float playerFear = playerFearLevel;

        playerFear -= Time.deltaTime * fearLevelDecreaseSpeed;

        if(playerFear < playerFearMin)
        {
            isHealing = false;                           // Stop Healing
        }

        SetFearLevel(playerFear);
        
    }

    private void SetFearLevel(float playerFear)
    {
        if (playerFear < playerFearMin)
        {
            playerFearLevel = playerFearMin;
        }
        else
        {
            playerFearLevel = playerFear;
        }
    }



    public float GetFearLevel()
    {
        return playerFearLevel;
    }

    public float GetFearMax()
    {
        return playerFearMax;
    }

    public void SetPlayerFearState(PlayerFearState fearState)
    {
        this.fearState = fearState; 
    }
    public PlayerFearState GetFearState()
    {
        return this.fearState;
    }
    public void SetPlayerFearState(PlayerFearState fearState,FearLevel fearLevel)
    {
        this.fearState = fearState;
        this.fearLevel = fearLevel;
    }

}
