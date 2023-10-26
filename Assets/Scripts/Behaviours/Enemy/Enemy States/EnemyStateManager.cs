using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour,ILightAffectable
{
    // Debugging Enemy State
    private enum EnemyState
    {
        ChasePlayer,Idle
    }
    [Header("Debug Enemy States")]
    [SerializeField] private EnemyState enemyState = EnemyState.Idle;

    public delegate void GainSightEvent(Transform Target);
    public GainSightEvent OnEnemyDetectedTorch;
    public delegate void LoseSightEvent(Transform Target);
    public LoseSightEvent OnEnemyLossFromTorch;

    [SerializeField] private BaseEnemyState currentEnemyState;
    [SerializeField] private BaseEnemyState previousEnemyState;

    [field: SerializeField] public bool IsAffectedByLight { get; set; }
    [field: SerializeField] public bool IsOnPlayerFov { get; set; }
    [field: SerializeField] public bool IsOnPlayerRadius { get; set; }

    public bool IsTestChase;

    public IdleStateEnemy IdleState = new ();
    public ChasingStateEnemy ChasingStateEnemy = new ();
    public FearedStateEnemy FearedStateEnemy = new ();
    public AttackStateEnemy AttackStateEnemy = new();

    public float EnemyCoolDowntime = 10f;
    //public float EnemyTriggerTimeMin;
    //public  float EnemyTriggerTimeMax;

    [Header("Dont edit Just For Debug")]
    public float PlayerSpendedTimeOnDark;
    public float SelectedTriggerTime;
    public int AngryLevel = 0;

    public bool IsEnemyTriggered = false;

    [Header("Fear State Variables")]
    public LayerMask HidableLayers;
    [Range(-1, 1)]
    [Tooltip("Lower is a better hiding spot")]
    public float HideSensitivity = 0;
    [Range(1, 10)]
    public float MinPlayerDistance = 5f;
    [Range(0, 5f)]
    public float MinObstacleHeight = 1.25f;
    [Range(0.01f, 1f)]
    public float UpdateFrequency = 0.25f;

    public Coroutine MovementCoroutine;
    public Collider[] Colliders = new Collider[10]; // more is less performant, but more options
    [HideInInspector] public SphereCollider sphereCollider;

    [HideInInspector] public TorchCollisionDetection TorchCollision;
    [HideInInspector] public EnemyMovement EnemyMovement;
    [HideInInspector] public Room AstrRoom;



    private void Awake()
    {
        EnemyMovement = GetComponent<EnemyMovement>();
        TorchCollision = FindObjectOfType<TorchCollisionDetection>();
        sphereCollider = GetComponent<SphereCollider>();
        AstrRoom = FindObjectOfType<Room>();
    }
    private void Start()
    {
        //currentEnemyState = IdleState;      // Set as default Idle State

        currentEnemyState?.EnterState(this); // Passing this class Context
    }

    private void Update()
    {
        currentEnemyState?.UpdateState(this); // Passing this class Context

        //if (IsOnPlayerFov)
        //{
        //    if (currentEnemyState != hideStateEnemy)
        //        SwitchEnemyState(hideStateEnemy);
        //}

        if(IsAffectedByLight)
        {
            SwitchEnemyState(FearedStateEnemy);  
        }

        // Debug Testing
        if(enemyState == EnemyState.ChasePlayer)
        {
            SwitchEnemyState(ChasingStateEnemy);
        }
        else if(enemyState == EnemyState.Idle)
        {
            currentEnemyState = null;
        }
    }

    private void FixedUpdate()
    {
        currentEnemyState?.PhysicsUpdateState(this);
    }

    public void SwitchEnemyState(BaseEnemyState newEnemyState)
    {
        if(currentEnemyState == newEnemyState) return;

        if(currentEnemyState != null) 
        {
            currentEnemyState.ExitState(this);
        }
        else
        {
            Debug.LogWarning("No Current State Found");
        }
        
        
        currentEnemyState = newEnemyState;

        currentEnemyState?.EnterState(this);
    }

    public BaseEnemyState GetCurrentEnemyState()
    {
        return currentEnemyState;
    }

    public BaseEnemyState GetPreviousEnemyState()
    {
        return previousEnemyState;
    }

    public void SetLightAffected(bool isAffectedByLight)
    {
        this.IsAffectedByLight = isAffectedByLight;

        if(isAffectedByLight)
        {
            // invoke
            OnEnemyDetectedTorch?.Invoke(GetPlayerTransform());
        }
        else if(!isAffectedByLight)
        {
            OnEnemyLossFromTorch?.Invoke(GetPlayerTransform());  
        }
    }

    public void SetOnPlayerFOV(bool isOnPlayerFOV)
    {
        this.IsOnPlayerFov = isOnPlayerFOV;
    }
    public void DestroyEnemyGameObject()
    {
        Destroy(this.gameObject);   
    }

    public Transform GetPlayerTransform()
    {
        return FindObjectOfType<FirstPersonController>().gameObject.transform;
    }

    public void SetPreviousEnemyState(BaseEnemyState previousState)
    {
        previousEnemyState = previousState;
    }

    public void SetOnPlayerRadious(bool isOnPlayerRadious)
    {
       this.IsOnPlayerRadius = isOnPlayerRadious;
    }
}
