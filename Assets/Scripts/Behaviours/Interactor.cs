using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(ObjectRotator))]
[RequireComponent(typeof(ObjectZoomer))]
public class Interactor : MonoBehaviour
{
    #region Events
    public delegate void OnGatherableObjectPicked(GatherableObject gatherableObject);  // While Picked creating Custom Delegate Event Ui Propably Subber.
    public OnGatherableObjectPicked onGatherableObjectPicked;
    public delegate void OnGatherableObjectDropped();                         // While Dropped creating Custom Delegate Event Ui Propably Subber.
    public OnGatherableObjectDropped oGatherableObjectDropped;
    #endregion

    #region Variables For Object Holding Type
    // there Is Two Type Of Object holding
    public enum ObjectHoldingType 
    {
        AlongCamera,            // Holding transform Setted As Child to Camera
        AlongPlayer             // Holding transform Setted As Child to Player
    }
    [SerializeField] private ObjectHoldingType holdingType;
    [SerializeField] private Transform cameraParentTransform;
    [SerializeField] private Transform playerParentTransform;
    #endregion

    #region Interaction Variables
    private new Camera camera;                                     // fps Camera ref 
    [SerializeField] private float interactRange = 2f;         // interation range can be Modify
    [SerializeField] private LayerMask interactLayerMask;      // interaction Layer
    [SerializeField] private Transform grabPoint;              // grabPoint that Can be Use to hold Obj.
    [SerializeField] private float grabPointZOffset;           // Z offset for grab pos
    private GatherableObject currentPickedGatherableObject;    //current holding Gatherable reference
    private GatherableObject currentInteractingObject;         // current Selected(Ray just Touched) Object

    private ObjectRotator objectRotator;                       // ref for Obj Rotator Handles rotation
    private ObjectZoomer objectZoomer;                         // ref for Object Zoomer Handles Zoom in out

    private Quaternion grabbedObjectTargetFaceRotation;        // facing oject rotation towards player
    private Vector3 grabbedObjectOldPosition;
    private Quaternion grabbedObjectOldRotation;

    private bool isInspecting;
    private float lerpSpeed = 0.2f;
    private float distanceTreshold = 0.1f;
    private float rotationDistanceThreshold = 0.1f;
    private bool isReachedTargetPosAndRot;
    #endregion

    #region Ui variables
    [SerializeField] private TextMeshProUGUI indigatorTextPrompt;  // this text Just Show Interactable Object Details
    #endregion

    #region Player Refs
    private FirstPersonController firstPersonController;
    #endregion

    private IInteractable interactedEnvObject;

    private void Awake()
    {
        camera = GetComponentInChildren<Camera>();
        firstPersonController = GetComponent<FirstPersonController>();   
        objectRotator = GetComponent<ObjectRotator>();
        objectZoomer = GetComponent<ObjectZoomer>();
        InitializeHoldingType();
    }

    #region Initializing Holding Type
    private void InitializeHoldingType()
    {
        if(holdingType == ObjectHoldingType.AlongCamera)  // Along With Camera
        {
            grabPoint.SetParent(cameraParentTransform,true); // Setting World Pos True Means We Dont Want To Move grab Pos
        }
        else // AlongWithPlayer
        {
            grabPoint.SetParent(playerParentTransform,true); // Setting World Pos True Means We Dont Want To Move grab Pos
        }
    }
    #endregion

    private void Start()
    {
        InputManager.Instance.OnInteractionKeyPerformed += InputManager_Instance_OnInteractionKeyPerformed;  // Subbing On Even
        InventoryUI.Instance.OnAddObjectToInventoryBtnClicked += Inventory_Instance_OnAddObjectToInventoryBtnClicked;

        indigatorTextPrompt.text = string.Empty;
    }

    private void Inventory_Instance_OnAddObjectToInventoryBtnClicked(object sender, EventArgs e)
    {
        Drop();
        indigatorTextPrompt.text = string.Empty;
    }

    private void Update()
    {
        UpdateGrabPointOffset();
        HandleGrabAndDropGrabbaleObject();
    }

    private void HandleGrabAndDropGrabbaleObject()
    {
        if (!HasHoldingObject()) return;

        if(isInspecting && !isReachedTargetPosAndRot)
        {
            currentPickedGatherableObject.transform.SetPositionAndRotation(Vector3.Lerp(currentPickedGatherableObject.transform.position,
                grabPoint.position, lerpSpeed), Quaternion.Lerp(currentPickedGatherableObject.transform.rotation,
                grabbedObjectTargetFaceRotation, lerpSpeed));

            float distance = Vector3.Distance(currentPickedGatherableObject.transform.position, grabPoint.position);
            float facingRotationDistance = Quaternion.Angle(currentPickedGatherableObject.transform.rotation, grabbedObjectTargetFaceRotation);

            if (distance < distanceTreshold && facingRotationDistance < rotationDistanceThreshold)
            {
                Debug.Log("Grab pos Reached");
                isReachedTargetPosAndRot = true;
            }
        }
        else if(!isInspecting)
        {
            
            currentPickedGatherableObject.transform.SetPositionAndRotation(Vector3.Lerp(currentPickedGatherableObject.transform.position,
                grabbedObjectOldPosition, lerpSpeed), Quaternion.Lerp(currentPickedGatherableObject.transform.rotation,
                grabbedObjectOldRotation, lerpSpeed));

            float distance = Vector3.Distance(currentPickedGatherableObject.transform.position, grabbedObjectOldPosition);
            float rotationDistance = Quaternion.Angle(currentPickedGatherableObject.transform.rotation, grabbedObjectOldRotation);


            if (distance < distanceTreshold && rotationDistance < rotationDistanceThreshold)
            {
                Debug.Log("Old Pos Reached");
                SetCurrentGatheredObject(null);
            }
            
        }
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
            Drop();
            CanMovePlayer(true);
            CanMoveCamera(true);
            //SetCurrentGatheredObject(null);
            oGatherableObjectDropped?.Invoke();

            indigatorTextPrompt.text = string.Empty;
        }
        else
        {
        // if Holding Nothing Do Step for Hold New
           CastRay();
        }
    }  // This Method Just responsible for pick And Drop Validation

    private void SetCurrentGatheredObject(GatherableObject gatherableObject)
    {
        currentPickedGatherableObject = gatherableObject;
    }

    private void CastRay() 
    {
        if (!Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, interactRange, interactLayerMask)) return;

        if(hit.collider != null)
        {
            
            GatherableObject gatherableObject = hit.collider.GetComponentInParent<GatherableObject>();   // Reason Getting From Parent, Parent Has The Script
            if(gatherableObject != null)
            {
                Debug.Log(gatherableObject.GetGatherableSO().gatherableObjectName);
                indigatorTextPrompt.text = gatherableObject.GetGatherableSO().gatherableObjectName;
                SetGrabbedObjectReferences(gatherableObject);
                Grab();
                CanMovePlayer(false);
                ResetCameraYPos();
                CanMoveCamera(false);

                // invoking Event Sending Data Current Grabbed Gatherable SO
                onGatherableObjectPicked?.Invoke(currentPickedGatherableObject);
            }

            IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();
            interactable?.Interact();
            
        }

    }  // this method Just Detect Object with Validation Only Calls By Input

    private void SetGrabbedObjectReferences(GatherableObject gatherableObject)
    {
        SetCurrentGatheredObject(gatherableObject);
        grabbedObjectOldPosition = currentPickedGatherableObject.GetOrginPosition();
        grabbedObjectOldRotation = currentPickedGatherableObject.GetOrginRotation();
        grabbedObjectTargetFaceRotation = currentPickedGatherableObject.GetTargetFaceRotation();
    }

    private void CheckInteractionUpdate()  // This Method Just Shows That Object Details which is Player Looking
    {
        
        Debug.DrawRay(camera.transform.position, camera.transform.forward * interactRange, Color.green);
        if (HasHoldingObject()) return;

        if (!Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, interactRange, interactLayerMask))
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
        
        Debug.DrawRay(camera.transform.position, camera.transform.forward * interactRange, Color.red);

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
    public void Grab()
    {
        currentPickedGatherableObject.SetActiveSelectedVisual(false);
        PullGrabbableObject();
        EffectManager.Instance.SetPostProccessingExaminaionBlurEnabled(true);
        objectRotator.SetCanRotateObjectAndRotateAccess(currentPickedGatherableObject.gameObject.transform,true);
        objectZoomer.SetCanZoomObjectAndZoomAccess(currentPickedGatherableObject.gameObject.transform, true);
    }

    private void PullGrabbableObject()
    {
        isInspecting = true;
    }

    public void Drop()
    {
        LeaveGrabbedObject();
        EffectManager.Instance.SetPostProccessingExaminaionBlurEnabled(false);
        objectRotator.SetCanRotateObjectAndRotateAccess(null,false);
        objectZoomer.SetCanZoomObjectAndZoomAccess(null,false);
    }

    private void LeaveGrabbedObject()
    {
        isInspecting = false;
        isReachedTargetPosAndRot = false;
    }


    // Returns true When currentPickedOnj != null
    public bool HasHoldingObject()
    {
        return currentPickedGatherableObject != null;
    }

    public GatherableObject GetHoldingObject()
    {
        return currentPickedGatherableObject;
    }  // This Function returns Current holding Object

    private void CanMovePlayer(bool canMovePlayer)
    {
        firstPersonController.playerCanMove = canMovePlayer;
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
