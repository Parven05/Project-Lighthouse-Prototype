using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChasingStateEnemy : BaseEnemyState
{ 
    public override void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("Im In Chasing State");

        enemy.EnemyMovement.SetTarget(enemy.GetPlayerTransform(), () =>
        {
            Debug.Log("You Killed By Enemy Now Triggered To Attack state");
            //enemy.SwitchEnemyState(enemy.AttackStateEnemy);
        });

    }


    public override void UpdateState(EnemyStateManager enemy)
    {
        if (enemy.IsTestChase)
        {
            if (enemy.EnemyMovement.HasTarget()) 
                return;

            enemy.EnemyMovement.SetAgentSpeed(5f);
            enemy.EnemyMovement.SetTarget(enemy.GetPlayerTransform(), () =>
            {
                Debug.Log("You Killed By Enemy Now Triggered Attack state");
                //enemy.SwitchEnemyState(new AttackStateEnemy());
            });
        }
    }

    public override void ExitState(EnemyStateManager enemy)
    {
        enemy.SetPreviousEnemyState(this);
    }
   

    private void SpawnEnemyNearPlayer(EnemyStateManager enemy)
    {
        List<EnemyHidePoint> enemyHidePoints = enemy.TorchCollision.GetLightNotAffectedWithInTorchFOV();

        if (enemyHidePoints.Count > 0)
        {
            // Create an array to hold the spawn positions
            Vector3[] spawnPositions = new Vector3[enemyHidePoints.Count];

            // Extract positions from the enemy hide points
            for (int i = 0; i < enemyHidePoints.Count; i++)
            {
                spawnPositions[i] = enemyHidePoints[i].transform.position;
            }

            // Choose a random position from the list
            Vector3 randomSpawnPosition = spawnPositions[UnityEngine.Random.Range(0, spawnPositions.Length)];

            // Set the enemy's position to the random spawn position
            enemy.transform.position = randomSpawnPosition;
        }
        else
        {
            // Handle the case where there are no valid spawn positions
            Debug.LogWarning("No valid spawn positions found for the enemy.");
        }
    }

    private void SpawnEnemyNearPlayerPos(EnemyStateManager enemy)
    {
        Vector3 spawnPos = GetRandomHidePointPosition().position;

        // Set the enemy's position to the random spawn position
        enemy.transform.position = spawnPos;

    }

    private Transform GetRandomHidePointPosition()
    {
        List<EnemyHidePoint> enemyHidePoints = PlayerRadiusSensor.GetLightNotAffectedWithInPlayerRadius();

        if (enemyHidePoints.Count > 0)
        {
            // Create an array to hold the spawn positions
            Transform[] spawnPosTransforms = new Transform[enemyHidePoints.Count];

            // Extract positions from the enemy hide points
            for (int i = 0; i < enemyHidePoints.Count; i++)
            {
                spawnPosTransforms[i] = enemyHidePoints[i].transform;
            }

            // Choose a random position from the list
            Transform randomSpawnPosTransform = spawnPosTransforms[UnityEngine.Random.Range(0, spawnPosTransforms.Length)];

            return randomSpawnPosTransform;
        }
        else
        {
            // Handle the case where there are no valid spawn positions
            Debug.LogWarning("No valid spawn positions found for the enemy.");
            return null;
        }
    }

  
    //private void CoolDownEnemy(EnemyStateManager enemy)
    //{
    //    Debug.Log("Enemy Cool Downed");
    //    enemy.IsEnemyTriggered = false;
    //    enemy.SwitchEnemyState(enemy.IdleState);
    //}

    public override void PhysicsUpdateState(EnemyStateManager enemyStateManager)
    {
        
    }
}
