using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    [SerializeField] private bool SPAWN_TEST = false;  // DELETE ASAP Press Space

    public static EnemySpawnController Instance;

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform enemyTriggerParent;

    [SerializeField] private float enemyTriggerTimeMin;
    [SerializeField] private float enemyTriggerTimeMax;

    [SerializeField] private float playerSpendedTimeOnDark;
    [SerializeField] private float selectedTriggerTime;
    [SerializeField] private int angryLevel = 0;

    [SerializeField] private bool isEnemyTriggered = false;

    

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        selectedTriggerTime = GetRandomTimeForTrigger();     // Set Random Trigger Time

        enemyTriggerParent.gameObject.SetActive(false);
    }

    public void DisableTriggers()
    {
        enemyTriggerParent.gameObject.SetActive(false); //test Commanding
        Debug.Log("Enemy Showed Triggers Disabled For test not deactivated");
        CoolDownEnemy();
        ResetPlayerSpendTimeDark();
    }

    private void Update()
    {
        playerSpendedTimeOnDark = PlayerFearSystem.PLAYER_SPENDING_TIME_DARK;

        if (playerSpendedTimeOnDark >= selectedTriggerTime && !isEnemyTriggered)
        {
            angryLevel++;
            isEnemyTriggered = true;

            ResetPlayerSpendTimeDark();

            selectedTriggerTime = GetRandomTimeForTrigger(); // Set AnotherTrigger Time

            if (angryLevel == 1)
            {
                //enemyStateManager.SwitchEnemyState(new ChasingStateEnemy(ChaseMode.Ignore)); // ignore first time
                ResetPlayerSpendTimeDark();
                CoolDownEnemy();
            }
            if (angryLevel == 2)
            {
                enemyTriggerParent.gameObject.SetActive(true);
            }
            if (angryLevel == 3)
            {
                SpawnEnemyAtPlayer(); // chase State 
            }
            if (angryLevel == 4)
            {
                //enemyStateManager.SwitchEnemyState(new ChasingStateEnemy(ChaseMode.DeadChase)); // 4th Warning
            }

        }


        if(SPAWN_TEST)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //SpawnEnemyAtPlayer();
            }
        }
    }

    private static void ResetPlayerSpendTimeDark()
    {
        PlayerFearSystem.PLAYER_SPENDING_TIME_DARK = 0f; // Direct Access From here
    }

    private void SpawnEnemyAtPlayer()
    {
        var enemyInstance = Instantiate(enemyPrefab);

        var enemyStateManager = enemyInstance.GetComponent<EnemyStateManager>();

        if (TryGetEnemyHidePointExeptInPlayerFOV(out Transform spawnPos))
        {
            enemyInstance.transform.position = spawnPos.position;
        }
        else
        {
            List<EnemyHidePoint> enemyHidePoints = new(enemyStateManager.AstrRoom.GetRoomEnemyHidePoints());

            Transform spawnPoint = enemyHidePoints[Random.Range(0, enemyHidePoints.Count)].transform;

            enemyInstance.transform.position = spawnPoint.position;
        }

        enemyStateManager.SwitchEnemyState(enemyStateManager.ChasingStateEnemy); // Chase
    }

    private float GetRandomTimeForTrigger()
    {
        return Random.Range(enemyTriggerTimeMin, enemyTriggerTimeMax);
    }

    private bool TryGetEnemyHidePointExeptInPlayerFOV(out Transform spawnPos)
    {
        List<EnemyHidePoint> enemyHidePoints = PlayerRadiusSensor.GetLightNotAffectedAndInPlayerFOV();

        if (enemyHidePoints.Count > 0)
        {
            Transform[] spawnPositions = new Transform[enemyHidePoints.Count];

            // Extract positions from the enemy hide points
            for (int i = 0; i < enemyHidePoints.Count; i++)
            {
                spawnPositions[i] = enemyHidePoints[i].transform;
            }

            // Choose a random position from the list
            Transform randomSpawnPosition = spawnPositions[Random.Range(0, spawnPositions.Length)];

            spawnPos = randomSpawnPosition;
            return true;
        }
        else
        {
            Debug.LogWarning("No valid spawn positions found for the enemy.");
            spawnPos = null;
            return false;
        }

    }

    private void CoolDownEnemy()
    {
        isEnemyTriggered = false;
    }

   
}
