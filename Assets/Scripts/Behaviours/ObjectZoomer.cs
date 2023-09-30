using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectZoomer : MonoBehaviour
{
    private Camera cameraToZoom;
    [SerializeField] private float zoomSpeed = 0.5f;
    [SerializeField] private float minZoom = 0.5f;
    [SerializeField] private float maxZoom = 2f;

    private bool canZoomObject;
    private InputManager inputManager;
    private bool isZooming;
    private void Awake()
    {
       cameraToZoom = Camera.main;
    }
    private void Start()
    {
        inputManager = InputManager.Instance;
        if (inputManager.examineObjectZoomType == InputManager.ExamineObjectZoomType.KeyPadControl)
        {
            zoomSpeed /= 5;   // Key Are Too sensitive (fast ) so decresing Value / 
        }
    }

    private void Update()
    {
        if (!canZoomObject) return;

        if(inputManager.examineObjectZoomType == InputManager.ExamineObjectZoomType.KeyPadControl)
        {
            float zoomInput = 0;

            if (inputManager.IsPressingZoomInKeyInKeyBoard())
            {
                zoomInput += 0.1f;
                isZooming = true;
            }
            else
            {
                isZooming = false;
            }

            if (inputManager.IsPressingZoomOutKeyInKeyBoard())
            {
                zoomInput -= 0.1f;
                isZooming = true;
            }
            else
            {
                isZooming = false;
            }

            if (zoomInput != 0)
            {
                Vector3 zoomDirection = (transform.position - cameraToZoom.transform.position).normalized;

                float currentDistance = Vector3.Distance(transform.position, cameraToZoom.transform.position);
                float newDistance = Mathf.Clamp(currentDistance + zoomInput * zoomSpeed, minZoom, maxZoom);

                transform.position = cameraToZoom.transform.position + zoomDirection * newDistance;
            }
        }
        else if(inputManager.examineObjectZoomType == InputManager.ExamineObjectZoomType.MouseControl)
        {
            // Old To New Input System
            float scrollDelta = /*Input.GetAxis("Mouse ScrollWheel");*/ InputManager.Instance.inputActions.Player.ZoomKeysMouse.ReadValue<float>();
            Vector3 zoomDirection = (transform.position - cameraToZoom.transform.position).normalized;

            float currentDistance = Vector3.Distance(transform.position, cameraToZoom.transform.position);
            float newDistance = Mathf.Clamp(currentDistance + scrollDelta * zoomSpeed, minZoom, maxZoom);

            transform.position = cameraToZoom.transform.position + zoomDirection * newDistance;

            if(scrollDelta != 0)
            {
                isZooming = true;
            }
            else
            {
                isZooming = false;
            }
        }

    }

    public void SetCanZoomObject(bool canZoomObject,float accessDelay = 0)
    {
        if (accessDelay == 0)
        {
            this.canZoomObject = canZoomObject;
        }
        else
        {
            StartCoroutine(SetCanZoomObjectWithDelay(canZoomObject, accessDelay));
        }

    }

    private IEnumerator SetCanZoomObjectWithDelay(bool canZoomObject, float accessDelay)
    {
        yield return new WaitForSeconds(accessDelay);
        this.canZoomObject = canZoomObject;
    }

    public bool IsZoomingObject()
    {
        return isZooming;
    }
}
