using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class FearedStateEnemy : BaseEnemyState
{
    private BaseEnemyState previousState;
    private EnemyMovement enemyMovement;

    private Transform target;
    private EnemyStateManager enemy;



    public override void EnterState(EnemyStateManager enemy)
    {
        Debug.Log("Im In Feared State");
        this.enemy = enemy;
        this.enemyMovement = enemy.EnemyMovement;
        this.previousState = enemy.GetPreviousEnemyState();

        enemy.OnEnemyDetectedTorch += HandleOnEnemyFear;
        enemy.OnEnemyLossFromTorch += HandleOnEnemyNotFear;

        StopChase();

        HandleOnEnemyFear(enemy.GetPlayerTransform());
    }

    private void HandleOnEnemyNotFear(Transform Target)
    {
        if (enemy.MovementCoroutine != null)
        {
            enemy.StopCoroutine(enemy.MovementCoroutine);
        }
        target = null;

        enemy.SwitchEnemyState(previousState);
    }

    private void HandleOnEnemyFear(Transform Target)
    {
        if (enemy.MovementCoroutine != null)
        {
            enemy.StopCoroutine(enemy.MovementCoroutine);
        }
        target = Target;

        enemy.MovementCoroutine = enemy.StartCoroutine(Hide(Target));
    }

    private void StopChase()
    {
        enemyMovement.SetTarget(null);
        enemyMovement.SetAgentSpeed(15f);
        enemyMovement.SetAgentAcceleration(15);
    }

    public override void ExitState(EnemyStateManager enemy)
    {
        enemy.OnEnemyDetectedTorch -= HandleOnEnemyFear;
        enemy.OnEnemyLossFromTorch -= HandleOnEnemyNotFear;

        enemyMovement.ResetAgentSpeed();

        //HandleLoseSight(enemy.GetPlayerTransform());

        enemy.SetPreviousEnemyState(this);
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        //if (!enemy.IsAffectedByLight)
        //{
        //    if (enemy.MovementCoroutine != null)
        //    {
        //        enemy.StopCoroutine(enemy.MovementCoroutine);
        //    }
        //    target = null;

        //    enemy.SwitchEnemyState(previousState);
        //}
        //else if(enemy.IsAffectedByLight)
        //{
        //    if (enemy.MovementCoroutine != null)
        //    {
        //        enemy.StopCoroutine(enemy.MovementCoroutine);
        //    }
        //    target = enemy.GetPlayerTransform();

        //    // Set the flag to true to indicate that Hide should be called
        //    //shouldCallHide = true;

        //    enemy.MovementCoroutine = enemy.StartCoroutine(Hide(target));
        //}

        // Check the flag and call Hide if it's true
        //if (shouldCallHide)
        //{
        //    enemy.MovementCoroutine = enemy.StartCoroutine(Hide(target));
        //    shouldCallHide = false;  // Reset the flag
        //}

    }

    private IEnumerator Hide(Transform target)
    {
        WaitForSeconds Wait = new WaitForSeconds(enemy.UpdateFrequency);
        while (true)
        {
            for (int i = 0; i < enemy.Colliders.Length; i++)
            {
                enemy.Colliders[i] = null;
            }

            int hits = Physics.OverlapSphereNonAlloc(enemyMovement.navMeshAgent.transform.position, enemy.sphereCollider.radius, enemy.Colliders, enemy.HidableLayers);

            int hitReduction = 0;
            for (int i = 0; i < hits; i++)
            {
                if (Vector3.Distance(enemy.Colliders[i].transform.position, target.position) < enemy.MinPlayerDistance || enemy.Colliders[i].bounds.size.y < enemy.MinObstacleHeight)
                {
                    enemy.Colliders[i] = null;
                    hitReduction++;
                }
            }
            hits -= hitReduction;

            System.Array.Sort(enemy.Colliders, ColliderArraySortComparer);

            for (int i = 0; i < hits; i++)
            {
                if (NavMesh.SamplePosition(enemy.Colliders[i].transform.position, out NavMeshHit hit, 2f, enemyMovement.navMeshAgent.areaMask))
                {
                    if (!NavMesh.FindClosestEdge(hit.position, out hit, enemyMovement.navMeshAgent.areaMask))
                    {
                        Debug.LogError($"Unable to find edge close to {hit.position}");
                    }

                    if (Vector3.Dot(hit.normal, (target.position - hit.position).normalized) < enemy.HideSensitivity)
                    {
                        enemyMovement.navMeshAgent.SetDestination(hit.position);
                        break;
                    }
                    else
                    {
                        // Since the previous spot wasn't facing "away" enough from teh target, we'll try on the other side of the object
                        if (NavMesh.SamplePosition(enemy.Colliders[i].transform.position - (target.position - hit.position).normalized * 2, out NavMeshHit hit2, 2f, enemyMovement.navMeshAgent.areaMask))
                        {
                            if (!NavMesh.FindClosestEdge(hit2.position, out hit2, enemyMovement.navMeshAgent.areaMask))
                            {
                                Debug.LogError($"Unable to find edge close to {hit2.position} (second attempt)");
                            }

                            if (Vector3.Dot(hit2.normal, (target.position - hit2.position).normalized) < enemy.HideSensitivity)
                            {
                                enemyMovement.navMeshAgent.SetDestination(hit2.position);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    Debug.LogError($"Unable to find NavMesh near object {enemy.Colliders[i].name} at {enemy.Colliders[i].transform.position}");
                }
            }
            yield return Wait;
        }
    }

    public int ColliderArraySortComparer(Collider A, Collider B)
    {
        if (A == null && B != null)
        {
            return 1;
        }
        else if (A != null && B == null)
        {
            return -1;
        }
        else if (A == null && B == null)
        {
            return 0;
        }
        else
        {
            return Vector3.Distance(enemyMovement.navMeshAgent.transform.position, A.transform.position).CompareTo(Vector3.Distance(enemyMovement.navMeshAgent.transform.position, B.transform.position));
        }
    }

    public override void PhysicsUpdateState(EnemyStateManager enemyStateManager)
    {

    }
}
