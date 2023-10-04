using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZonePoint : MonoBehaviour
{
    public enum LightType
    {
        UpToDownFaced,
        Movable,
        DownToUpFaced
    }

    [SerializeField] private LightType lightType = LightType.UpToDownFaced;
    [SerializeField] private LayerMask floorLayer;
    [SerializeField] private bool showGizMos= true;

    // Up to Down Light Props
    [SerializeField] private float safeZoneRadius = 2f;
    [SerializeField] private Vector3 targetColliderPosition;

    //private Ray ray;
    private void Start()
    {
        InitializeTriggerZoneSetup();
    }

    private void InitializeTriggerZoneSetup()
    {
        switch (lightType)
        {
            case LightType.UpToDownFaced:
                CreateSphereHealer(targetColliderPosition);
                break;

            case LightType.DownToUpFaced:

                break;

            case LightType.Movable:

                break;

        }
    }

    private void CreateSphereHealer(Vector3 targetPosition)
    {
        GameObject go = new("Safe_Zone_Point"); // Create the GameObject
        go.transform.SetParent(transform); // Set its parent
        
        // Setting Positions
        go.transform.localPosition = Vector3.zero;
        go.transform.position = targetPosition;

        // Creating Collider
        SphereCollider collider = go.AddComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = safeZoneRadius;

        // Adding Script
        var healer = go.AddComponent<Healer>();
        healer.ActivateHealing();

    }

    //private Vector3 GetFloorContactPoint()
    //{
    //    ray.origin = transform.position + transform.forward * 0.5f;   // Avoiding Current Object
    //    ray.direction = transform.forward;

    //    RaycastHit[] collider = new RaycastHit[1];

    //    int hitCount = Physics.RaycastNonAlloc(ray, collider, 20f, floorLayer);

    //    if (hitCount > 1)
    //    {
    //        if (collider.Length >= 1)
    //        {
    //            Debug.Log(collider[0].point);
    //            return collider[0].point;             // returning floor SurfacePosition
    //        }
    //    }
    //    return Vector3.zero;                  // If Nothing There Just return 0

    //}

    //private void FixedUpdate()
    //{
    //    Debug.DrawRay(transform.position + transform.forward * 0.5f, transform.forward * 20f, Color.red);
    //}


    private void OnDrawGizmos()
    {
        if(showGizMos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(targetColliderPosition, safeZoneRadius);
        }
        
    }

}