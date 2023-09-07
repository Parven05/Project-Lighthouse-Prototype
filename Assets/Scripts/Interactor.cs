using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private float interactRange = 1.5f;
    [SerializeField] private LayerMask interactLayerMask;
    private Inventory inventory;
    private GatherableObject currentInteractedGatherableObject;   // Current Selected Object 

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
    }
    private void Start()
    {
        InputManager.Instance.OnInteractionKeyPerformed += InputManager_Instance_OnInteractionKeyPerformed;  // Subbing On Even
    }

    private void InputManager_Instance_OnInteractionKeyPerformed(object sender, System.EventArgs e)
    {
        Interact();
    }

    private void Interact()
    {
        List<GatherableObject> nearbyGatherableObjects = new List<GatherableObject>();

        Collider[] colliders = Physics.OverlapSphere(transform.position, interactRange, interactLayerMask);
        foreach(Collider collider in colliders)
        {
            GatherableObject gatherableObject = collider.GetComponentInParent<GatherableObject>();
            if (gatherableObject != null)
            {
                Debug.Log("Objs Detected Count" + gatherableObject.GetGatherableSO().gatherableObjectName);
                nearbyGatherableObjects.Add(gatherableObject);
            }
        }
        // Set Current Nearby Gatherable Item
        if(nearbyGatherableObjects.Count > 0)
        {
            currentInteractedGatherableObject = GetNearGatherableObject(nearbyGatherableObjects);
            Debug.Log("Nearby Gatherable" + " " + currentInteractedGatherableObject.GetGatherableSO().gatherableObjectName);
        }
       
    }


    // Get Nearby Obj Using Linq Loop Also Can Usable But It more Readable 
    private GatherableObject GetNearGatherableObject(List<GatherableObject> nearbyGatherableObjects)
    {
        if (nearbyGatherableObjects.Count == 0)
        {
            Debug.Log("List is Empty");
            return null;
        }
           
        var nearGatherableObj = nearbyGatherableObjects.OrderBy(s => Vector3.Distance(transform.position, s.transform.position));  // Using Linq To Get Obj By Distance. 
        GatherableObject selectedGatherableObject = nearbyGatherableObjects[0];
        return selectedGatherableObject;
    }


    // Unsubbing Is Neccessary When Obj Disable And Enable this Might be Create 2 Listners or More
    private void OnDisable()
    {
        InputManager.Instance.OnInteractionKeyPerformed -= InputManager_Instance_OnInteractionKeyPerformed;
    }

}
