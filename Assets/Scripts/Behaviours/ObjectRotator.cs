using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectRotator : MonoBehaviour
{
    private bool canRotateObject = false;
    [SerializeField] float rotationSpeed = 5f;
    private InputManager inputManager;
    private Vector3 dragOrigin;
    private Vector3 toRotateObjCurrentEulerAngles;
    private bool isRotating = false;
    private void Start()
    {
        inputManager = InputManager.Instance;

        inputManager.inputActions.Player.MouseLeftClick.performed += MouseLeftClick_performed;
        inputManager.inputActions.Player.MouseLeftClick.canceled += MouseLeftClick_canceled;
    }

    private void MouseLeftClick_canceled(InputAction.CallbackContext context)
    {
        // Called when the mouse button is released
        if (context.ReadValue<float>() == 0.0f)
        {
            isRotating = false;
        }
    }

    private void MouseLeftClick_performed(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() == 1.0f)
        {
            // Capture the mouse position when the drag starts
            //dragOrigin = new Vector3(inputManager.GetmouseDelta().x, inputManager.GetmouseDelta().y, 0);
            dragOrigin = inputManager.GetMousePosition();
            toRotateObjCurrentEulerAngles = transform.eulerAngles;
            isRotating = true;
        }
    }

    void Update()
    {

        if(!canRotateObject) return;

        if (inputManager.examineControlType == InputManager.ExamineObjectRotateType.KeyPadControl)
        {
            float rotationX = 0f;
            float rotationY = 0f;

            Vector3 inputValue = InputManager.Instance.GetInputAxisXAndZ(); // y is 0
            isRotating = false; // Reset isRotating at the beginning

            if (inputValue.z > 0)
            {
                Debug.Log("Pressing Up");
                rotationX = -1f * rotationSpeed;
                isRotating = true;
            }
            else if (inputValue.z < 0)
            {
                Debug.Log("Pressing Down");
                rotationX = 1f * rotationSpeed;
                isRotating = true;
            }

            if (inputValue.x < 0)
            {
                Debug.Log("Pressing left");
                rotationY = -1f * rotationSpeed;
                isRotating = true;
            }
            else if (inputValue.x > 0)
            {
                Debug.Log("Pressing right");
                rotationY = 1f * rotationSpeed;
                isRotating = true;
            }

            // Apply the rotation to the object
            transform.Rotate(rotationX, rotationY, 0f);
        }

        else if (inputManager.examineControlType == InputManager.ExamineObjectRotateType.MouseControl)
        {
               // shifting old input system to new
            //if (/*Input.GetMouseButton(0)*/inputManager.inputActions.Player.MouseLeftClick.triggered) 
            //{
            //    // Capture the mouse position when the drag starts
            //    dragOrigin = Input.mousePosition;
            //    toRotateObjCurrentEulerAngles = transform.eulerAngles;
            //    isRotating = true;
            //}
            //   // shifting old input system to new
            //if (/*Input.GetMouseButton(0)*/inputManager.inputActions.Player.MouseLeftClick.triggered)
            //{
            //    // Calculate the difference in mouse position
            //    Vector3 difference = Input.mousePosition - dragOrigin;

            //    // Calculate the rotation angles based on the mouse movement
            //    float rotationX = -difference.y * rotationSpeed;
            //    float rotationY = difference.x * rotationSpeed;

            //    // Apply the rotation to the object
            //    Vector3 newEulerAngles = toRotateObjCurrentEulerAngles + new Vector3(rotationX, rotationY, 0f);
            //    transform.rotation = Quaternion.Euler(newEulerAngles);
            //    isRotating = true;

            //}
            //if (/*Input.GetMouseButtonUp(0)*/inputManager.inputActions.Player.MouseLeftClick.phase == InputActionPhase.Canceled)
            //{
            //    isRotating = false;
            //}

            if (isRotating)
            {
                // Get the mouse delta movement from the Input Action
                //Vector2 mouseDelta = inputManager.GetmouseDelta();
                //Vector3 difference = new Vector3(inputManager.GetmouseDelta().x, inputManager.GetmouseDelta().y,0) - dragOrigin;
                Vector3 difference = new Vector3(inputManager.GetMousePosition().x, inputManager.GetMousePosition().y,0f) - dragOrigin;
                // Calculate the rotation angles based on the mouse movement
                float rotationX = -difference.y * rotationSpeed;
                float rotationY = difference.x * rotationSpeed;

                // Apply the rotation to the object
                Vector3 newEulerAngles = toRotateObjCurrentEulerAngles + new Vector3(rotationX, rotationY, 0f);
                transform.rotation = Quaternion.Euler(newEulerAngles);
            }

            //toRotateObjecttransform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"), 0) * Time.deltaTime * rotationSpeed);
        }
    
    }
    public bool IsRotatingObject()
    {
        return isRotating;
    }

    public void SetCanRotateObject(bool canRotateObject,float delayAccess = 0)
    {
        if(delayAccess == 0)
        {
            this.canRotateObject = canRotateObject;
        }
        else
        {
            StartCoroutine(SetCanRotateObjectWithDelay(canRotateObject,delayAccess));
        }
       
    }

    private IEnumerator SetCanRotateObjectWithDelay(bool canRotateObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        this.canRotateObject = canRotateObject;
    }


    private void OnDisable()
    {
        inputManager.inputActions.Player.MouseLeftClick.performed -= MouseLeftClick_performed;
        inputManager.inputActions.Player.MouseLeftClick.canceled -= MouseLeftClick_canceled;
    }
}
