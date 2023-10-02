using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerIK : MonoBehaviour
{
    [SerializeField] private Transform rightHandTargetIK;
    [SerializeField] private Transform leftHandTargetIK;

    [SerializeField] private Rig rightHandRigLayer;
    [SerializeField] private float lerpSpeed = 3f;

    private Animator animator;
    private Action OnHandReachedTargetSuccess;
    private Transform handTargetPos;
    public enum HandState
    {
        idle,
        HandMovingToTarget,
        HandMovingBackToIdle
    }
    private HandState currentHandState;
    private void Awake()
    {
        rightHandRigLayer.weight = 0f;
        animator = GetComponent<Animator>();
    }
    public Transform GetRightHandTargetIK()
    {
        return rightHandTargetIK;
    }

    public void MoveRightHandTo(Transform target,Action OnHandReachedTargetSuccess)
    {
        // Assign Hand Target Refs
        this.rightHandTargetIK.position = target.position;
        handTargetPos = target;

        this.OnHandReachedTargetSuccess = OnHandReachedTargetSuccess;

        SetHandState(HandState.HandMovingToTarget);
    }

    private void Update()
    {
        if (currentHandState == HandState.idle)
        {
            
        }
        else if(currentHandState == HandState.HandMovingToTarget)
        {
            rightHandRigLayer.weight = Mathf.MoveTowards(rightHandRigLayer.weight, 1f, lerpSpeed * Time.deltaTime);
            if (rightHandRigLayer.weight == 1 )
            {
                animator.SetTrigger("Grab");
                Debug.Log("twist Animation place Here");
                handTargetPos.DORotate(new Vector3(60, 0, 0), 0.3f).OnComplete(() =>
                {
                    OnHandReachedTargetSuccess?.Invoke();
                    SetHandState(HandState.HandMovingBackToIdle);
                }); // Anim
            }


        }
        else if(currentHandState == HandState.HandMovingBackToIdle)
        {
            rightHandRigLayer.weight = Mathf.MoveTowards(rightHandRigLayer.weight, 0f, lerpSpeed * Time.deltaTime);
            if (rightHandRigLayer.weight == 0)
            {
                SetHandState(HandState.idle);
            }
        
        }

    }

    private void SetHandState(HandState state)
    {
        currentHandState = state;
    }
}
