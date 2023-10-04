using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKnob : MonoBehaviour, IInteractable
{
    private Door door;

    private void Awake()
    {
        door = GetComponentInParent<Door>();
    }
    public void Interact()
    {
        door.Interact();
    }

    public Transform GetHandTargetTransform()
    {
       return door.GetHandTargetTransform();
    }

    public Transform[] GetHandFingersTargetTransforms()
    {
        return null;
    }
}
