using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prefabs : MonoBehaviour
{
    public static Prefabs Instance { get; private set; }

    [SerializeField] private PrefabSO prefabSO;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject GetEnemyPrefab()
    {
        return prefabSO.EnemyPrefab;
    }
    public GameObject GetEnemyDummyPrefab()
    {
        return prefabSO.EnemyDummyPrefab;
    }

    public GameObject GetPlayerPrefab()
    {
        return prefabSO.PlayerPrefab;
    }
}
