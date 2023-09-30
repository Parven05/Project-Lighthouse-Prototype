using DG.Tweening;
using System;
using System.Linq;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private GatherableSO validKeySO;
    [SerializeField] private Transform doorHinge;
    [SerializeField] private Transform doorKnob;
    [SerializeField] private Transform handTargetPos;

    [SerializeField] private float targetOpenAngle = -90f;
    [SerializeField] private float targetKnobAngle = 90f;
    [SerializeField] private float knobRotateDuration = 0.3f;
    [SerializeField] private float doorOpenDuration = 0.5f;
    [SerializeField] private float doorCloseDuration = 0.4f;

    [SerializeField] private Ease easeTypeOpen = Ease.Linear;
    [SerializeField] private Ease easeTypeClose = Ease.Linear;

    [SerializeField] private DoorType doorType;
    [SerializeField] private Transform doorFrameCenter;
    [SerializeField] private Transform victimTransform;

    private bool isOpenedDoor;
    private bool isGotKey;
    private bool canTrapVictim;
    private bool isVictimTrapped;

    [SerializeField] private float doorCloseDistance = 2f;
    [SerializeField] private float dotProductThreshold = 0.2f;

    public enum DoorType
    {
        NoKeyDoor,
        KeyDoor,
        GhostDoor
    }
    private void Awake()
    {
        SetActiveSelectedVisual(false);
    }

    public void Interact()
    {
       //var fpsControl = FindObjectOfType<FirstPersonController>();
       //fpsControl.cameraCanMove = false;
       DoorToggle();  
    }

    private void DoorToggle()
    {
        if(doorType == DoorType.NoKeyDoor || doorType == DoorType.GhostDoor)
        {
            isOpenedDoor = !isOpenedDoor;
        }
        else if(doorType == DoorType.KeyDoor)
        {
            if(HasValidDoorKeyInInventory() || isGotKey)
            {
                isOpenedDoor = !isOpenedDoor;
            }
        }
       

        switch(doorType)
        {
            case DoorType.NoKeyDoor:
                if (isOpenedDoor)
                {
                    // open Door
                    doorHinge.DOLocalRotate(new Vector3(0, targetOpenAngle, 0), doorOpenDuration).SetEase(easeTypeOpen);
                    DoKnobAnimation();
                    Debug.Log("Door Opned");
                }
                else
                {
                    //close Door
                    doorHinge.DOLocalRotate(new Vector3(0, 0, 0), doorCloseDuration).SetEase(easeTypeClose);
                    Debug.Log("Door Closed");
                }
                break; 
            
            case DoorType.KeyDoor:
                if (isOpenedDoor)
                {
                    // open Door
                    if(HasValidDoorKeyInInventory() || isGotKey)
                    {
                        UseKey();
                        doorHinge.DOLocalRotate(new Vector3(0, targetOpenAngle, 0), doorOpenDuration).SetEase(easeTypeOpen);
                        isGotKey = true;
                        Debug.Log("Door Opned");
                    }
                    else
                    {
                        Debug.Log("Need key");
                    }
                }
                else 
                {
                    //close Door
                    CloseDoor();
                    Debug.Log("Door Locked");
                    DoKnobAnimation();
                }
                break;
            case DoorType.GhostDoor:
                if (isOpenedDoor && !isVictimTrapped)
                {
                    // open Door
                    doorHinge.DOLocalRotate(new Vector3(0, targetOpenAngle, 0), doorOpenDuration).SetEase(easeTypeOpen);
                    DoKnobAnimation();
                    canTrapVictim = true;
                    Debug.Log("Door Opned");
                }
                else
                {
                    DoKnobAnimation(); // just Do knob Animation
                }
                break;
        }

       
    }

    private void Update()
    {
        if (canTrapVictim && victimTransform != null)
        {
            // Calculate Distanec
            float distanceToVictim = Vector3.Distance(transform.position, victimTransform.position);  
            // Calculate the direction from the door to the victim
            Vector3 doorToVictimDirection = victimTransform.position - transform.position;

            // Calculate the direction from the door to the center of the door frame
            Vector3 doorToFrameCenterDirection = doorFrameCenter.position - transform.position;

            // Normalize both directions for the dot product
            doorToVictimDirection.Normalize();
            doorToFrameCenterDirection.Normalize();

            // Calculate the dot product between the two directions
            float dotProduct = Vector3.Dot(doorToVictimDirection, doorToFrameCenterDirection);
            //Debug.Log("Dot Product Difference" +" " + dotProduct);
            //Debug.Log("Distance Difference" + " " + distanceToVictim);
            // Check if the dot product is Greater than your threshold
            if (dotProduct > dotProductThreshold && distanceToVictim > doorCloseDistance)
            {
                Debug.Log("Victim Crossed Door");
                canTrapVictim = false;
                isVictimTrapped = true;
                CloseDoor();
                // Perform your door-closing action here
            }
        }
    }

    private void CloseDoor()
    {
        doorHinge.DOLocalRotate(new Vector3(0, 0, 0), doorOpenDuration).SetEase(easeTypeClose); // Close Door
    }

    private void DoKnobAnimation()
    {
        doorKnob.DOLocalRotate(new Vector3(0, 0, targetKnobAngle), knobRotateDuration).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            doorKnob.DOLocalRotate(new Vector3(0, 0, 0), knobRotateDuration);
        });
    }

    private bool HasValidDoorKeyInInventory()
    {
        GatherableSO[] gatherableSOs = Inventory.Instance.GetGatheredObjectList().ToArray();
        var hasKey = gatherableSOs.Any(s => s.gatherableObjectName == validKeySO.gatherableObjectName);
        if (hasKey)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void UseKey()
    {
        GatherableSO[] gatherableSOs = Inventory.Instance.GetGatheredObjectList().ToArray();
        var key = gatherableSOs.Where(s => s.gatherableObjectName == validKeySO.gatherableObjectName).FirstOrDefault(); ;
        Inventory.Instance.RemoveGatherableObjectFromInventoryList(key);  // remove key from inventory
    }

    public void SetActiveSelectedVisual(bool activeStatus)
    {
        Outline[] outlines = GetComponentsInChildren<Outline>();
        foreach (Outline outline in outlines)
        {
            outline.enabled = activeStatus;
        }
    }

    public Transform GetInteractObjectPos()
    {
        return handTargetPos;
    }

}
