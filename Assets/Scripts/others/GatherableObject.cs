using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class GatherableObject : MonoBehaviour
{
    [SerializeField] private GatherableSO gatherableSO;                  // Gatherable object data
    [SerializeField] private Quaternion targetFaceRotation;              // facing oject rotation towards player
    [SerializeField] private float moveDuration = 0.1f;
    [SerializeField] private Ease easeMode = Ease.Linear;

    private Vector3 oldPosition;                                         // ref to the Orgin Pos 
    private Quaternion oldRotation;                                      // ref to the Old Rot
    private Transform oldParentTransform;                                // ref to the Parent Transform of Current Object.

    private ObjectRotator objectRotator;
    private ObjectZoomer objectZoomer;

    private void Awake()
    {
        oldPosition = transform.position;
        oldRotation = transform.rotation;
        oldParentTransform = transform.parent;

        objectRotator = GetComponent<ObjectRotator>();
        objectZoomer = GetComponent<ObjectZoomer>();

        SetActiveSelectedVisual(false);   // Disable Selected Visual At Start
    }

    public GatherableSO GetGatherableSO()
    {
        return gatherableSO;
    }

    public void SetActiveSelectedVisual(bool  active)
    {
        Outline[] outlines = GetComponentsInChildren<Outline>();
        foreach (Outline outline in outlines)
        {
            outline.enabled = active;
        }
    }

    public void Grab(Transform targetTransform,Action OnObjectGrabbedDone)
    {
        transform.SetParent(targetTransform, true);
        MoveAndRotateToTarget(targetTransform.position,targetFaceRotation,() =>
        {
            //On this Object Reached Hold Position
            objectRotator.SetCanRotateObject(true);
            objectZoomer.SetCanZoomObject(true);

            OnObjectGrabbedDone?.Invoke();
        });
        SetActiveSelectedVisual(false);
        EffectManager.Instance.SetPostProccessingExaminaionBlurEnabled(true);
    }
    public void Drop()
    {
        transform.SetParent(oldParentTransform, true);
        MoveAndRotateToTarget(oldPosition, oldRotation);

        EffectManager.Instance.SetPostProccessingExaminaionBlurEnabled(false);
        objectRotator.SetCanRotateObject(false);
        objectZoomer.SetCanZoomObject(false);
    }

    private void MoveAndRotateToTarget(Vector3 target, Quaternion targetRotation, Action OnTargetPosRotReached = null)
    {
        int tweensCompleted = 0;

        // Move to target position
        transform.DOMove(target, moveDuration).SetEase(easeMode).OnKill(() =>
        {
            tweensCompleted++;

            // Check if both tweens are completed
            if (tweensCompleted >= 2)
            {
                OnTargetPosRotReached?.Invoke();
            }
        });

        // Rotate to target rotation
        transform.DORotateQuaternion(targetRotation, moveDuration).SetEase(easeMode).OnKill(() =>
        {
            tweensCompleted++;

            // Check if both tweens are completed
            if (tweensCompleted >= 2)
            {
                OnTargetPosRotReached?.Invoke();
            }
        });
    }

}
