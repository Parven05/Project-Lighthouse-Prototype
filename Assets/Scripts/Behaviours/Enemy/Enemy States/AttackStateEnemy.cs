using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateEnemy : BaseEnemyState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("Im Now Attack State");

        Debug.Log("Killed by enemy");
    }

    public override void ExitState(EnemyStateManager enemy)
    {
        enemy.SetPreviousEnemyState(this);
    }

    public override void PhysicsUpdateState(EnemyStateManager enemyStateManager)
    {
        
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        
    }
}
