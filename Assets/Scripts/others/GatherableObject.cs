using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class GatherableObject : MonoBehaviour
{
    [SerializeField] private GatherableSO gatherableSO;                  // Gatherable object data
    [SerializeField] private Quaternion targetFaceRotation;              // facing oject rotation towards player
    private Vector3 oldPosition;                                         // ref to the Orgin Pos 
    private Quaternion oldRotation;                                      // ref to the Old Rot
    private Transform oldParent;                                         // Ref to the old Parent
    private void Awake()
    {
        oldPosition = transform.position;
        oldRotation = transform.rotation;
        oldParent = transform.parent;

        SetActiveSelectedVisual(false);   // Disable Selected Visual At Start
    }
    public GatherableSO GetGatherableSO()
    {
        return gatherableSO;
    }
    public Vector3 GetOrginPosition()
    {
        return oldPosition;
    }
    public Quaternion GetTargetFaceRotation()
    {
        return targetFaceRotation;
    }
    public Quaternion GetOrginRotation()
    {
        return oldRotation;
    }
    public void SetActiveSelectedVisual(bool  active)
    {
        Outline[] outlines = GetComponentsInChildren<Outline>();
        foreach (Outline outline in outlines)
        {
            outline.enabled = active;
        }
    }
    public Transform GetOldParent()
    {
        return oldParent;
    }
}
