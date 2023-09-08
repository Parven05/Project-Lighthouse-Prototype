using System;
using UnityEngine;

public class Interactor : MonoBehaviour
{

    #region Interaction Variables
    private Camera camera;                                     // fps Camera ref 
    [SerializeField] private float interactRange = 2f;         // interation range can be Modify
    [SerializeField] private LayerMask interactLayerMask;      // interaction Layer
    [SerializeField] private Transform grabPoint;              // grabPoint that Can be Use to hold Obj.
    private GatherableObject currentPickedGatherableObject;    //current holding Gatherable reference
    #endregion

    private Inventory inventory;                               // inventory Ref


    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        camera = GetComponentInChildren<Camera>();
    }
    private void Start()
    {
        InputManager.Instance.OnInteractionKeyPerformed += InputManager_Instance_OnInteractionKeyPerformed;  // Subbing On Even
    }

    private void InputManager_Instance_OnInteractionKeyPerformed(object sender, EventArgs e)
    {
        Interact();
    }

    private void Interact()
    {
        // Checks If player holding Any Object
        if(HasHoldingObject())   
        {
            Debug.Log("Return Old Position");
            currentPickedGatherableObject.SetParent(null,OnPickUpObjectSuccessfullyMoved);
            currentPickedGatherableObject = null;
        }
        else
        {
        // if Holding Nothing Do Step for Hold New
           CastRay();
        }
    }

    private void CastRay()
    {
        if (!Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, interactRange, interactLayerMask)) return;

        if(hit.collider != null)
        {
           GatherableObject gatherableObject = hit.collider.GetComponentInParent<GatherableObject>();   // Reason Getting From Parent, Parent Has The Script
           Debug.Log( gatherableObject.GetGatherableSO().gatherableObjectName);
           currentPickedGatherableObject = gatherableObject;
           Pickup(currentPickedGatherableObject);
        }

    }

    // Responsible For call Setparent in gatherable object 
    private void Pickup(GatherableObject gatherableObject)
    {
        gatherableObject.SetParent(grabPoint,OnPickUpObjectSuccessfullyMoved);
       
    }

    private void OnPickUpObjectSuccessfullyMoved(bool isReturning)
    {
        if (isReturning)
        {
            Debug.Log("InVentory Ui Closed Take Drop buttons ");
        }
        else
        {
            Debug.Log("InVentory Ui Showed Take Drop buttons ");
        }
    }

    // Returns true When currentPickedOnj != null
    private bool HasHoldingObject()
    {
        return currentPickedGatherableObject != null;
    }
    // Unsubbing Is Neccessary When Obj Disable And Enable this Might be Create 2 Listners or More
    private void OnDisable()
    {
        InputManager.Instance.OnInteractionKeyPerformed -= InputManager_Instance_OnInteractionKeyPerformed;
    }

}
