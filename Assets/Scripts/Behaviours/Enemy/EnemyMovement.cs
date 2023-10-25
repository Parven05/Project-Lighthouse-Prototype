using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float stoppingDistance = 2f;
    public NavMeshAgent navMeshAgent;
    private Transform targetTransForm = null;
    private Vector3 targetPosition = Vector3.zero;
    private Action onTargetReachd;

    private bool isReachedTarget = false;

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

        if (navMeshAgent != null && targetTransForm != null)
        {
            if (navMeshAgent.isStopped)
            {
                navMeshAgent.isStopped = false;
            }

            navMeshAgent.destination = targetTransForm.position;

            // Check if the enemy has reached its target
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                // The enemy has reached its target
                onTargetReachd?.Invoke();
                targetTransForm = null;
                isReachedTarget = true;
                navMeshAgent.isStopped = true;
            }
            else
            {
                isReachedTarget = false;
            }
        }
       


        if (targetPosition != Vector3.zero)
        {
            navMeshAgent.destination = targetPosition;

            // Check if the enemy has reached its target
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                targetPosition = Vector3.zero;
            }
        }

    }

    public void SetTarget(Transform targetTranform, Action OnTargetReached = null)
    {
        this.targetTransForm = targetTranform;
        this.onTargetReachd = OnTargetReached;

    }
    public void SetTarget(Vector3 targetPosition, Action OnTargetReached = null)
    {
        this.targetPosition = targetPosition;
        this.onTargetReachd = OnTargetReached;
    }

    public void SetAgentSpeed(float newSpeed)
    {
       navMeshAgent.speed = newSpeed;
    }
    public void SetAgentAcceleration(float newAcceleration)
    {
        navMeshAgent.acceleration = newAcceleration;
    }

    public bool HasTarget()
    {
        return targetTransForm != null || targetPosition != Vector3.zero;
    }

    public void ResetAgentSpeed()
    {
        navMeshAgent.speed = 3.5f;
    }

    internal bool HasReachedTarget()
    {
        return isReachedTarget;
    }
}
