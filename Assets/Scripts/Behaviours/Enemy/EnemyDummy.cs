using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDummy : MonoBehaviour, ILightAffectable
{
    [field: SerializeField] public bool IsAffectedByLight { get ; set; }
    [field: SerializeField] public bool IsOnPlayerFov { get ; set; }
    [field: SerializeField] public bool IsOnPlayerRadius { get ; set ; }

    public void SetLightAffected(bool isAffectedByLight)
    {
       this.IsAffectedByLight = isAffectedByLight;
    }

    public void SetOnPlayerFOV(bool isOnPlayerFOV)
    {
       this.IsOnPlayerFov = isOnPlayerFOV;

       if(isOnPlayerFOV )
       {
           Debug.Log("Enemy Seen by Player");
           EnemySpawnController.Instance.DisableTriggers();
       }
    }

    public void SetOnPlayerRadious(bool isOnPlayerRadious)
    {
        this.IsOnPlayerRadius = isOnPlayerRadious;
    }
}
