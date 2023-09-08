using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherableObject : MonoBehaviour
{
    [SerializeField] private GatherableSO gatherableSO;
    private Vector3 originalPosition; 


    private void Awake()
    {
        originalPosition = transform.position;          // Taking Old Ref For Return Back
    }
    public GatherableSO GetGatherableSO()
    {
        return gatherableSO;
    }

    public void SetParent(Transform grabPoint, Action<bool> OnReachedTargetAndIsReturning)  // Bool Just Getting Is Returning Or Not Usabl For Ui On Off.
    {
        // if grab point != null so Has Parent
        if(grabPoint != null)
        {
            // DotWeen Tweening Small Anim
            transform.DOMove(grabPoint.transform.position, .3f).SetEase(Ease.Linear).OnComplete(() =>
            {
                OnReachedTargetAndIsReturning?.Invoke(false);                           // invoke with false because it Going for Grab point
            });
            transform.SetParent(grabPoint.transform);                                   // Setting Parent To The GrabPoint 
            transform.localPosition = Vector3.zero;                                     // resetting local Pos

        }
        else
        {   // Player Sended null Parent
            // Return Original Pos
            transform.DOMove(originalPosition, .3f).SetEase(Ease.Linear);           
            transform.SetParent(null);                                                  // Setting Parent To null So can Be Indipendent Again
            transform.localPosition = Vector3.zero;

            // No Need OnComplete Anim Because Ui Should Close Instantly.
            OnReachedTargetAndIsReturning?.Invoke(true);                                // invoke with true because it Going Old Place

        }

    }

}
