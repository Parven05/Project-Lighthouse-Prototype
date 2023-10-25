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

        LayerMask layerMask = LayerMask.NameToLayer("HealPoints");
        go.layer = layerMask;

        // Adding Script
        var healer = go.AddComponent<Healer>();
        healer.ActivateHealing();

    }

    private void OnDrawGizmos()
    {
        if(showGizMos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(targetColliderPosition, safeZoneRadius);
        }
        
    }

}