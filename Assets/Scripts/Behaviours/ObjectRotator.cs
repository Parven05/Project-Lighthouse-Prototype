using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    private bool canRotateObject = false;
    [SerializeField] float rotationSpeed = 2f;
    private Transform toRotateObjecttransform;
    private InputManager inputManager;
    private Vector3 dragOrigin;
    private Vector3 toRotateObjCurrentEulerAngles;
    private bool isRotating;
    private void Start()
    {
        inputManager = InputManager.Instance;
    }
    void Update()
    {

        if(!canRotateObject || toRotateObjecttransform == null) return;

        if(inputManager.examineControlType == InputManager.ExamineObjectRotateType.KeyPadControl)
        {
            float rotationX = 0f;
            float rotationY = 0f;

            if (inputManager.IsPressingUpDirectionKeys())
            {
                Debug.Log("Up");
                rotationX = -1f * rotationSpeed;
                isRotating = true;
            }
            else
            {
                isRotating = false;
            }
            if (inputManager.IsPressingDownDirectionKeys())
            {
                Debug.Log("Down");
                rotationX = 1f * rotationSpeed;
                isRotating = true;
            }
            else
            {
                isRotating = false;
            }
            if (inputManager.IsPressingLeftDirectionKeys())
            {
                Debug.Log("left");
                rotationY = -1f * rotationSpeed;
                isRotating = true;
            }
            else
            {
                isRotating = false;
            }
            if (inputManager.IsPressingRightDirectionKeys())
            {
                Debug.Log("right");
                rotationY = 1f * rotationSpeed;
                isRotating = true;
            }
            else
            {
                isRotating = false;
            }
            // Apply the rotation to the object
            toRotateObjecttransform.Rotate(rotationX, rotationY, 0f);
        }
        else if(inputManager.examineControlType == InputManager.ExamineObjectRotateType.MouseControl)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Capture the mouse position when the drag starts
                dragOrigin = Input.mousePosition;
                toRotateObjCurrentEulerAngles = toRotateObjecttransform.eulerAngles;
                isRotating = true;
            }

            if (Input.GetMouseButton(0))
            {
                // Calculate the difference in mouse position
                Vector3 difference = Input.mousePosition - dragOrigin;

                // Calculate the rotation angles based on the mouse movement
                float rotationX = -difference.y * rotationSpeed;
                float rotationY = difference.x * rotationSpeed;

                // Apply the rotation to the object
                Vector3 newEulerAngles = toRotateObjCurrentEulerAngles + new Vector3(rotationX, rotationY, 0f);
                toRotateObjecttransform.rotation = Quaternion.Euler(newEulerAngles);
                isRotating = true;

            }
            if(Input.GetMouseButtonUp(0))
            {
                isRotating = false;
            }

            //toRotateObjecttransform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"), 0) * Time.deltaTime * rotationSpeed);
        }
    
    }
    public bool IsRotatingObject()
    {
        return isRotating;
    }

    public void SetCanRotateObjectAndRotateAccess(Transform toRotateObject,bool canRotateObject)
    {
        this.toRotateObjecttransform = toRotateObject;
        this.canRotateObject = canRotateObject;
    }

}
