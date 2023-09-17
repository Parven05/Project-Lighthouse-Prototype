using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectZoomer : MonoBehaviour
{
    private Camera cameraToZoom;
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float minZoom = 25f;
    [SerializeField] private float maxZoom = 60f;

    private Transform toZoomObjectTransform;

    private float oldObjectPos;
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
        if (!canZoomObject || toZoomObjectTransform == null) return;

        if(inputManager.examineObjectZoomType == InputManager.ExamineObjectZoomType.KeyPadControl)
        {
            float zoomInput = 0;

            if (inputManager.IsPressingZoomInKey())
            {
                zoomInput += 1.0f;
                isZooming = true;
            }
            else
            {
                isZooming = false;
            }

            if (inputManager.IsPressingZoomOutKey())
            {
                zoomInput -= 1.0f;
                isZooming = true;
            }
            else
            {
                isZooming = false;
            }

            if (zoomInput != 0)
            {
                Vector3 zoomDirection = (toZoomObjectTransform.position - Camera.main.transform.position).normalized;

                float currentDistance = Vector3.Distance(toZoomObjectTransform.position, Camera.main.transform.position);
                float newDistance = Mathf.Clamp(currentDistance + zoomInput * zoomSpeed, minZoom, maxZoom);

                toZoomObjectTransform.position = Camera.main.transform.position + zoomDirection * newDistance;
            }
        }
        else if(inputManager.examineObjectZoomType == InputManager.ExamineObjectZoomType.MouseControl)
        {

            float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
            Vector3 zoomDirection = (toZoomObjectTransform.position - Camera.main.transform.position).normalized;

            float currentDistance = Vector3.Distance(toZoomObjectTransform.position, Camera.main.transform.position);
            float newDistance = Mathf.Clamp(currentDistance + scrollDelta * zoomSpeed, minZoom, maxZoom);

            toZoomObjectTransform.position = Camera.main.transform.position + zoomDirection * newDistance;

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

    public void SetCanZoomObjectAndZoomAccess(Transform toZoomObjectTransform,bool canZoomObject)
    {
        this.toZoomObjectTransform = toZoomObjectTransform;
        this.canZoomObject = canZoomObject;
        if(!canZoomObject)
        {
            ResetObjectZoom();
        }
    }

    private void ResetObjectZoom()
    {
        
    }

    internal bool IsZoomingObject()
    {
        return isZooming;
    }
}
