using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float stoppingDistance = 2f;
    private NavMeshAgent navMeshAgent;
    private Vector3 targetTransFormPosition;
    private Enemy.EnemyState enemyState;
    private Action onTargetReachd;
    

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        InitializeDefaultValues();
    }

    private void InitializeDefaultValues()
    {
        navMeshAgent.speed = speed;
        navMeshAgent.stoppingDistance = stoppingDistance;
    }

    private void Update()
    {
        if(navMeshAgent != null && targetTransFormPosition != Vector3.zero)
        {
            navMeshAgent.destination = targetTransFormPosition;

            // Check if the enemy has reached its target
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                // The enemy has reached its target
                 onTargetReachd?.Invoke();

                InitializeDefaultValues();
            }
        }

    }
    public void SetTarget(Vector3 targetTranformPosition,Enemy.EnemyState enemyState,Action OnTargetReached)
    {
        this.targetTransFormPosition = targetTranformPosition;
        this.enemyState = enemyState;   
        this.onTargetReachd = OnTargetReached;

        InitializeNavMeshModedValues();
    }

    private void InitializeNavMeshModedValues() // initialize Enemy Speed And Stopping Others
    {
        if(enemyState == Enemy.EnemyState.idle)
        {
             
        }
        else if(enemyState == Enemy.EnemyState.IgnoredPlayer)
        {

        }
        else if (enemyState == Enemy.EnemyState.ShoeOff)
        {
            navMeshAgent.stoppingDistance = 10f;
        }
        else if (enemyState == Enemy.EnemyState.Chase)
        {
            navMeshAgent.stoppingDistance = 2f;
            navMeshAgent.speed = speed * 0.5f;
        }
        else if (enemyState == Enemy.EnemyState.DeadChase)
        {
            navMeshAgent.stoppingDistance = 2f;
            navMeshAgent.speed = speed * 0.9f;
        }
        else if (enemyState == Enemy.EnemyState.FearedByLight) // Run Fast As U Can Enemy To Escape From Light
        {
            navMeshAgent.stoppingDistance = 1f;
            navMeshAgent.speed = speed * 2f;
        }
    }
}
