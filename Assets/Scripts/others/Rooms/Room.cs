using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private List<EnemyHidePoint> enemyHidePoints;
    [SerializeField] private int roomId;

    public List<EnemyHidePoint> GetRoomEnemyHidePoints()
    { 
        return enemyHidePoints; 
    }

    public int GetRoomId()
    {
        return roomId;
    }
}
