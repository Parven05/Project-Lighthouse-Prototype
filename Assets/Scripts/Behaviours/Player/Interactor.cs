using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    #region Interaction Variables
    [SerializeField] private Transform cameraTransform;                                     // fps Camera ref 
    [SerializeField] private float interactRange = 2f;         // interation range can be Modify
    [SerializeField] private LayerMask interactLayerMask;      // interaction Layer
    #endregion

    // ik Session
    [SerializeField] private PlayerIK playerIK;


    private void Start()
    {
        InputManager.Instance.inputActions.Player.InteractionKey.performed += InteractionKey_performed;
    }

    private void InteractionKey_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        CastRay();
    }

    // this method Just Detect Object with Validation Only Calls By Input
    private void CastRay() 
    {
        if (!Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, interactRange, interactLayerMask)) return;

        if(hit.collider != null)
        {
            if(hit.collider.TryGetComponent(out IInteractable interactable))
            {
                playerIK.MoveRightHandTo(interactable.GetHandTargetTransform(), () =>
                {
                    interactable.Interact();   // interact through interface
                });
              
            }
        }

    }  

    private void OnDisable()
    {
        InputManager.Instance.inputActions.Player.InteractionKey.performed -= InteractionKey_performed;
    }

}
