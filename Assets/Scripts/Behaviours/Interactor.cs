using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    #region Events
    public delegate void OnGatherableObjectPicked(GatherableObject gatherableObject);  // While Picked creating Custom Delegate Event Ui Propably Subber.
    public OnGatherableObjectPicked onGatherableObjectPicked;
    public delegate void OnGatherableObjectDropped();                         // While Dropped creating Custom Delegate Event Ui Propably Subber.
    public OnGatherableObjectDropped oGatherableObjectDropped;
    #endregion

    #region Interaction Variables
    [SerializeField] private Transform cameraTransform;                                     // fps Camera ref 
    [SerializeField] private float interactRange = 2f;         // interation range can be Modify
    [SerializeField] private LayerMask interactLayerMask;      // interaction Layer
    [SerializeField] private Transform grabPoint;              // grabPoint that Can be Use to hold Obj.
    [SerializeField] private float grabPointZOffset;           // Z offset for grab pos
    private GatherableObject currentPickedGatherableObject;    //current holding Gatherable reference
    private GatherableObject currentInteractingObject;         // current Selected(Ray just Touched) Object


    #endregion

    #region Ui variables
    [SerializeField] private TextMeshProUGUI indigatorTextPrompt;  // this text Just Show Interactable Object Details
    #endregion

    #region Player Refs
    private FirstPersonController firstPersonController;
    #endregion

    private IInteractable interactedEnvObject;
    private PlayerIK playerIK;
    private void Awake()
    {
        firstPersonController = GetComponent<FirstPersonController>();   
        playerIK = GetComponent<PlayerIK>();
    }
    private void Start()
    {
        InputManager.Instance.OnInteractionKeyPerformed += InputManager_Instance_OnInteractionKeyPerformed;  // Subbing On Even
        InventoryUI.Instance.OnAddObjectToInventoryBtnClicked += Inventory_Instance_OnAddObjectToInventoryBtnClicked;

        indigatorTextPrompt.text = string.Empty;
    }

    private void Inventory_Instance_OnAddObjectToInventoryBtnClicked(object sender, EventArgs e)
    {
        indigatorTextPrompt.text = string.Empty;
    }

    private void Update()
    {
        UpdateGrabPointOffset();
    }

   
    private void UpdateGrabPointOffset()
    {
        // Setting offset to the Grab point
        Vector3 grabPointPos = grabPoint.localPosition;
        grabPointPos.z = grabPointZOffset;
        grabPoint.localPosition = grabPointPos;
    }

    private void FixedUpdate()
    {
        CheckInteractionUpdate();
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
            currentPickedGatherableObject.Drop();
            currentPickedGatherableObject = null;
            CanMovePlayer(true);
            CanMoveCamera(true);
            oGatherableObjectDropped?.Invoke();

            indigatorTextPrompt.text = string.Empty;
        }
        else
        {
           // if Holding Nothing Do Step for Hold New
           CastRay();
        }
    }  // This Method Just responsible for pick And Drop Validation


    private void CastRay() 
    {
        if (!Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, interactRange, interactLayerMask)) return;

        if(hit.collider != null)
        {
            if(hit.collider.TryGetComponent(out GatherableObject gatherableObject))
            {
                currentPickedGatherableObject = gatherableObject;
                gatherableObject.Grab(grabPoint, ResetCameraYPos);
                Debug.Log(gatherableObject.GetGatherableSO().gatherableObjectName);

                indigatorTextPrompt.text = gatherableObject.GetGatherableSO().gatherableObjectName;

                CanMovePlayer(false);
                CanMoveCamera(false);

                // invoking Event Sending Data Current Grabbed Gatherable SO 
                onGatherableObjectPicked?.Invoke(currentPickedGatherableObject);
     
            }

            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();
            if(interactable != null)
            {
                if(interactable is Door)
                {
                   playerIK.MoveRightHandTo(interactable.GetInteractObjectPos(),() =>
                   {
                       interactable?.Interact();   // After Anim Finisthed open door
                   });
                   
                }
            }
            
            
        }

    }  // this method Just Detect Object with Validation Only Calls By Input

    private void CheckInteractionUpdate()  // This Method Just Shows That Object Details which is Player Looking
    {
        
        Debug.DrawRay(cameraTransform.position, cameraTransform.forward * interactRange, Color.green);
        if (HasHoldingObject()) return;

        if (!Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, interactRange, interactLayerMask))
        {
            if (currentInteractingObject != null)
                currentInteractingObject.SetActiveSelectedVisual(false);   // resetting visuals
                currentInteractingObject = null;

            if(interactedEnvObject != null)
            {
                interactedEnvObject.SetActiveSelectedVisual(false);        // Resetting visuals
                interactedEnvObject = null;
            }

            return;
        }
        
        Debug.DrawRay(cameraTransform.position, cameraTransform.forward * interactRange, Color.red);

        if (hit.collider != null)
        {
            GatherableObject gatherableObject = hit.collider.GetComponentInParent<GatherableObject>();
            if(gatherableObject != null )
            {
                currentInteractingObject = gatherableObject;
                currentInteractingObject.SetActiveSelectedVisual(true);           // Showing Selected visual
            }

            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();
            interactedEnvObject = interactable;
            interactable?.SetActiveSelectedVisual(true);
        }
        
      
    }
    // Returns true When currentPickedOnj != null
    public bool HasHoldingObject()
    {
        return currentPickedGatherableObject != null;
    }

    public GatherableObject GetHoldingObject() // This Function returns Current holding Object
    {
        return currentPickedGatherableObject;
    }  

    private void CanMovePlayer(bool canMovePlayer)
    {
        firstPersonController.playerCanMove = canMovePlayer;
        firstPersonController.enableHeadBob = canMovePlayer;  // Disable Head bob
    }
    private void CanMoveCamera(bool canMoveCamera)
    {
        firstPersonController.cameraCanMove = canMoveCamera;
    }
    private void ResetCameraYPos()
    {
        firstPersonController.ResetCameraYRotation(0.3f);  // duration For reset
    }
    // Unsubbing Is Neccessary When Obj Disable And Enable this Might be Create 2 Listners or More
    private void OnDisable()
    {
        InputManager.Instance.OnInteractionKeyPerformed -= InputManager_Instance_OnInteractionKeyPerformed;
        InventoryUI.Instance.OnAddObjectToInventoryBtnClicked -= Inventory_Instance_OnAddObjectToInventoryBtnClicked;
    }

}
