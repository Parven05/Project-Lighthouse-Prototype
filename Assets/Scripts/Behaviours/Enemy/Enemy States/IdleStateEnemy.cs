using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleStateEnemy : BaseEnemyState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("Im In Idle State");
    }
    public override void UpdateState(EnemyStateManager enemy) // Esm = Enemy State Manager Reference
    {
        
    }

    public override void ExitState(EnemyStateManager enemy)
    {
        enemy.SetPreviousEnemyState(this);
    }

    public override void PhysicsUpdateState(EnemyStateManager enemyStateManager)
    {
        
    }
}
