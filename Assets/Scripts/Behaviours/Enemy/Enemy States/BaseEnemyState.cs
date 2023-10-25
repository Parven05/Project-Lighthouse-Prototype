using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class BaseEnemyState 
{
    public abstract void EnterState(EnemyStateManager enemyStateManager);

    public abstract void UpdateState(EnemyStateManager enemyStateManager);

    public abstract void PhysicsUpdateState(EnemyStateManager enemyStateManager);

    public abstract void ExitState(EnemyStateManager enemyStateManager);
}
