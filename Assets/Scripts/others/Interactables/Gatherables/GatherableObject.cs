using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class GatherableObject : MonoBehaviour,IInteractable
{
    [SerializeField] private GatherableSO gatherableSO;                  // Gatherable object data
    [SerializeField] private bool canDoSelectedVisual = false;

    private void Awake()
    {
       SetActiveSelectedVisual(false);   // Disable Selected Visual At Start
    }

    public void Interact()
    {
        //Go To Inventory
        Inventory.Instance.AddObjectToInventory(this);
    }

    public GatherableSO GetGatherableSO()
    {
        return gatherableSO;
    }

    public void SetActiveSelectedVisual(bool  active)
    {
        if (!canDoSelectedVisual) return;

        Outline[] outlines = GetComponentsInChildren<Outline>();
        foreach (Outline outline in outlines)
        {
            outline.enabled = active;
        }
    }

    public Transform GetHandTargetTransform()
    {
        return null;
    }

    public Transform[] GetHandFingersTargetTransforms()
    {
        return null;
    }

    public Transform GetObjectTransform()
    {
        return transform;
    }
}
