using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherableObject : MonoBehaviour
{
    [SerializeField] private GatherableSO gatherableSO;



    public GatherableSO GetGatherableSO()
    {
        return gatherableSO;
    }
}
