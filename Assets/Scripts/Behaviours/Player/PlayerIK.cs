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
    [SerializeField] private Transform torchHoldPoint;

    [SerializeField] private Rig rightHandRigLayer;
    [SerializeField] private float lerpSpeed = 3f;
    [SerializeField] private float torchEquipSpeed = 5f;

    private Animator animator;
    private FirstPersonController firstPersonController;
    private Action OnHandReachedTargetSuccess;
    private Transform handTargetPos;

    private Torch playerTorch;
    public enum HandState
    {
        idle,
        HandMovingToTarget,
        HandMovingBackToIdle,
        HoldingTorch
    }
    private HandState currentHandState;
    private void Awake()
    {
        rightHandRigLayer.weight = 0f;

        animator = GetComponent<Animator>();
        firstPersonController = GetComponent<FirstPersonController>();
        playerTorch = FindObjectOfType<Torch>();
    }
    private void Start()
    {
        Door.OnAnyDoorKnobAnimFinished += Door_OnAnyDoorKnobAnimFinished;
    }

    private void Door_OnAnyDoorKnobAnimFinished(object sender, EventArgs e)
    {
        SetHandState(HandState.HandMovingBackToIdle);
    }

    public Transform GetRightHandTargetIK()
    {
        return rightHandTargetIK;
    }

    public void MoveRightHandTo(Transform target,Action OnHandReachedTargetSuccess,bool lockPlayerPosition = false)
    {
        if (target == null)
        {
            OnHandReachedTargetSuccess?.Invoke(); // null means its Was Gatherable Object
            return;
        }

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
            //firstPersonController.cameraCanMove = false;
            //firstPersonController.playerCanMove = false;

            rightHandRigLayer.weight = Mathf.MoveTowards(rightHandRigLayer.weight, 1f, lerpSpeed * Time.deltaTime);
            if (rightHandRigLayer.weight == 1 )
            {
                handTargetPos.DORotate(new Vector3(50, 0, 0), 0.3f).OnComplete(() =>
                {
                    SetHandState(HandState.HandMovingBackToIdle);
                    OnHandReachedTargetSuccess?.Invoke();
                });
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
        else if(currentHandState == HandState.HoldingTorch)
        {
            if (playerTorch.IsActive())
            {
                rightHandTargetIK.SetLocalPositionAndRotation(torchHoldPoint.localPosition, torchHoldPoint.localRotation);

                rightHandRigLayer.weight = Mathf.Lerp(rightHandRigLayer.weight, 1f, torchEquipSpeed * Time.deltaTime);
            }
            else
            {
                rightHandRigLayer.weight = Mathf.Lerp(rightHandRigLayer.weight, 0f, torchEquipSpeed * Time.deltaTime);
            }
        }

        //UpdateHandPos();



        if (playerTorch.IsActive())
        {
            SetHandState(HandState.HoldingTorch);
        }
    }

    private void UpdateHandPos()
    {
        if (handTargetPos == null) return;

        rightHandTargetIK.position = handTargetPos.position;
        rightHandTargetIK.rotation = handTargetPos.rotation;
    }

    private void SetHandState(HandState state)
    {
        currentHandState = state;
    }

    private void OnDisable()
    {
        Door.OnAnyDoorKnobAnimFinished -= Door_OnAnyDoorKnobAnimFinished;
    }
}
