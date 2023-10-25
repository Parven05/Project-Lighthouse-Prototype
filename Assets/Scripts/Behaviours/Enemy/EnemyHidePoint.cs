using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHidePoint : MonoBehaviour,ILightAffectable
{
    [field: SerializeField] public bool IsAffectedByLight { get; set ;}
    [field: SerializeField] public bool IsOnPlayerFov {  get; set; }
    [field: SerializeField] public bool IsOnPlayerRadius { get; set; }
    public void SetLightAffected(bool isAffectedByLight)
    {
        IsAffectedByLight = isAffectedByLight;
    }

    public void SetOnPlayerFOV(bool isOnPlayerFOV)
    {
       this.IsOnPlayerFov = isOnPlayerFOV;
    }

    public void SetOnPlayerRadious(bool isOnPlayerRadious)
    {
        this.IsOnPlayerRadius = isOnPlayerRadious;  
    }

    private void OnDrawGizmos()
    {
        if (IsAffectedByLight)
        {
            Gizmos.color = Color.red;
        }
        else if(IsOnPlayerFov)
        {
            Gizmos.color = Color.magenta;
        }
        else
        {
            Gizmos.color = Color.green;
        }
        Gizmos.DrawSphere(transform.position, 0.5f);
    }

}
