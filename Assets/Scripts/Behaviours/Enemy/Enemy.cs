using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy Instance { get; private set; }  // Singleton Because We Would Have One Enemy

    [SerializeField] private float enemyCoolDowntime = 10f;
    [SerializeField] private float enemyTriggerTimeMin;
    [SerializeField] private float enemyTriggerTimeMax;

    [Header("Dont edit Just For Debug")]
    [SerializeField] private float playerSpendedTimeOnDark;
    [SerializeField] private float selectedTriggerTime;
    [SerializeField] private int angryLevel = 0;
    private bool isEnemyTriggered = false;

    private TorchCollisionDetection torch;
    private EnemyMovement enemyMovement;

    [SerializeField] private bool isEnemyScared;
    [SerializeField] private Transform playerTransform;
    private Vector3 orginPosition;



    public enum EnemyState
    {
        idle,
        IgnoredPlayer,
        ShoeOff,
        Chase,
        DeadChase,
        FearedByLight
    }
    [SerializeField] private EnemyState enemyState = EnemyState.idle;

    private void Awake()
    {
        Instance = this;
        torch = FindObjectOfType<TorchCollisionDetection>();
        enemyMovement = GetComponent<EnemyMovement>();
    }

    private void Start()
    {
        torch.OnTorchFacingEnemy += Torch_OnTorchFacingEnemy;
        torch.OnTorchNotFacingEnemy += Torch_OnTorchNotFacingEnemy;

        selectedTriggerTime = GetRandomTimeForTrigger();
        orginPosition = transform.position;
    }


    private void Torch_OnTorchFacingEnemy(object sender, EventArgs e)
    {
        isEnemyScared = true;
        Debug.Log("Is Enemy Faced Torch :" +  isEnemyScared);
        SetEnemyState(EnemyState.FearedByLight);
        UpdateStateOnce();
      
    }
    private void Torch_OnTorchNotFacingEnemy(object sender, EventArgs e)
    {
        isEnemyScared = false;
        Debug.Log("Is Enemy Faced Torch :" + isEnemyScared);


    }

    private float GetRandomTimeForTrigger()
    {
        return UnityEngine.Random.Range(enemyTriggerTimeMin, enemyTriggerTimeMax);
    }

    private void Update()
    {
        playerSpendedTimeOnDark = PlayerFearSystem.PLAYER_SPENDING_TIME_DARK;

        InitializeAngryLevel();

        UpDateEnemyState();

    }

    private void UpdateStateOnce()
    {
        if (enemyState == EnemyState.idle)
        {

        }
        else if (enemyState == EnemyState.IgnoredPlayer)
        {
            Debug.Log("Enemy Screaming Warning !!!");

            Invoke(nameof(CoolDownEnemy), enemyCoolDowntime);
        }
        else if (enemyState == EnemyState.ShoeOff)
        {
            Debug.Log("Enemy Apeared But Ur Luck !!!");

            enemyMovement.SetTarget(playerTransform.position, enemyState, () =>
            {
                CoolDownEnemy();
            });

        }
        else if (enemyState == EnemyState.Chase)
        {
            Debug.Log("Enemy Triggerd Hard He Chasing You !!!");

            enemyMovement.SetTarget(playerTransform.position, enemyState, () =>
            {
                CoolDownEnemy();
            });

        }
        else if (enemyState == EnemyState.DeadChase)
        {
            Debug.Log("Enemy Triggers Soo Hard You Will Die Sure !!!");

            enemyMovement.SetTarget(playerTransform.position, enemyState, () =>
            {
                CoolDownEnemy();
            });

        }
        else if (enemyState == EnemyState.FearedByLight)
        {
            Debug.Log("Im Escape !!!");

            enemyMovement.SetTarget(orginPosition, enemyState, () =>
            {
                CoolDownEnemy();
            });

        }



    }

    private void UpDateEnemyState()
    {
        if (enemyState == EnemyState.idle)
        {

        }
        else if (enemyState == EnemyState.IgnoredPlayer)
        {
            //Debug.Log("Enemy Screaming Warning !!!");

            //Invoke(nameof(CoolDownEnemy), enemyCoolDowntime);
        }
        else if (enemyState == EnemyState.ShoeOff)
        {
            //Debug.Log("Enemy Apeared But Ur Luck !!!");

            //enemyMovement.SetTarget(playerTransform,enemyState,() =>
            //{
            //    CoolDownEnemy();
            //});

        }
        else if (enemyState == EnemyState.Chase)
        {
            //Debug.Log("Enemy Triggerd Hard He Chasing You !!!");

            //enemyMovement.SetTarget(playerTransform, enemyState, () =>
            //{
            //    CoolDownEnemy();
            //});

        }
        else if (enemyState == EnemyState.DeadChase)
        {
            //Debug.Log("Enemy Triggers Soo Hard You Will Die Sure !!!");

            //enemyMovement.SetTarget(playerTransform, enemyState, () =>
            //{
            //    CoolDownEnemy();
            //});

        }
        else if (enemyState == EnemyState.FearedByLight)
        {
            //Debug.Log("Im Escape !!!");

            //enemyMovement.SetTarget(orginPosition, enemyState, () =>
            //{
            //    CoolDownEnemy();
            //});

        }
    }

    private void InitializeAngryLevel()
    {
        if (playerSpendedTimeOnDark >= selectedTriggerTime && !isEnemyTriggered)
        {
            angryLevel++;
            isEnemyTriggered = true;
            PlayerFearSystem.PLAYER_SPENDING_TIME_DARK = 0f; // Direct Access From here
            selectedTriggerTime = GetRandomTimeForTrigger(); // Set AnotherTrigger Time

            if (angryLevel == 1)
            {
                SetEnemyState(EnemyState.IgnoredPlayer); // First Warning
            }
            if (angryLevel == 2)
            {
                SetEnemyState(EnemyState.ShoeOff); // Second Warning
            }
            if (angryLevel == 3)
            {
                SetEnemyState(EnemyState.Chase); // 3rd Warning
            }
            if (angryLevel == 4)
            {
                SetEnemyState(EnemyState.DeadChase); // 4th Warning
            }

            UpdateStateOnce();

        }

        if (isEnemyScared)
        {
            SetEnemyState(EnemyState.FearedByLight); // light fear
        }
    }

    private void CoolDownEnemy()
    {
        isEnemyTriggered = false;
    }

    public void SetEnemyState(EnemyState enemyState)
    {
        this.enemyState = enemyState;
    }



    private void OnDisable()
    {
        torch.OnTorchFacingEnemy -= Torch_OnTorchFacingEnemy;
        torch.OnTorchNotFacingEnemy -= Torch_OnTorchNotFacingEnemy;
    }
}
