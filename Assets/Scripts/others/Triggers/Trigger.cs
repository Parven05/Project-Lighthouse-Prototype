using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Trigger : MonoBehaviour
{
    [SerializeField] private bool TEST_SPAWN_MOVE;
    [SerializeField] private List<CrossPoint> enemyCrossPointList;
    [SerializeField] private float moveDelay = 0.3f;

    private Transform playerTransform;

    private void Start()
    {
        playerTransform = FirstPersonController.Instance.gameObject.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out FirstPersonController player))
        {
            SpawnToMoveEnemy();
        }
    }

    private void SpawnToMoveEnemy()
    {
        CrossPoint crossPoint = GetRandomCrossPoint();

        var enemyInstance = Instantiate(Prefabs.Instance.GetEnemyDummyPrefab());
        enemyInstance.transform.position = crossPoint.PositionA.position;

        // Calculate the rotation direction based on the movement direction
        Vector3 lookDirection = (crossPoint.PositionB.position - crossPoint.PositionA.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

        enemyInstance.transform.DORotateQuaternion(targetRotation, 0.1f).OnComplete(() =>
        {
            enemyInstance.transform.DOMove(crossPoint.PositionB.position, moveDelay).OnComplete(() =>
            {
                Destroy(enemyInstance.gameObject);
            });

        });

    }

    private CrossPoint GetRandomCrossPoint()
    {
        if(enemyCrossPointList.Count == 1)
        {
            return enemyCrossPointList[0];
        }
        else if(enemyCrossPointList.Count > 1) 
        { 
            return enemyCrossPointList[Random.Range(0,enemyCrossPointList.Count)];
        }
        else
        {
            Debug.LogWarning("No Croos Point Assigned");
            return null;
        }
    }

    private void Update()
    {
        if (TEST_SPAWN_MOVE)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SpawnToMoveEnemy();
            }
        }

    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if(enemyCrossPointList.Count > 0) 
        {
            foreach (CrossPoint crossPoint in enemyCrossPointList)
            {
                Gizmos.DrawLine(crossPoint.PositionA.position, crossPoint.PositionB.position);
                Gizmos.DrawSphere(crossPoint.PositionA.position,0.3f);
                Gizmos.DrawSphere(crossPoint.PositionB.position, 0.3f);
            }
        }

        
    }

}
[System.Serializable]
public class CrossPoint
{
    public Transform PositionA;
    public Transform PositionB;
}