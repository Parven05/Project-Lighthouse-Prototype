using Micosmo.SensorToolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerRadiusSensor : MonoBehaviour
{
    [SerializeField] private Sensor rangeSensor;
    [SerializeField] private Sensor losSensor;
    [SerializeField] private Sensor fovColliderSensor;

    private static List<ILightAffectable> objectsInRangeSensor;
    private static List<ILightAffectable> objectsInLosSensor;
    private static List<ILightAffectable> objectsInFovColliderSensor;

    private void Awake()
    {
        objectsInRangeSensor = new List<ILightAffectable>();
        objectsInLosSensor = new List<ILightAffectable>();
        objectsInFovColliderSensor = new List<ILightAffectable>();
    }
    private void Update()
    {
        if (rangeSensor.Detections.Count > 0)
        {
            List<ILightAffectable> objectsToRemove = new(objectsInRangeSensor);

            foreach (var enemyHidePoint in rangeSensor.Detections)
            {
                if (enemyHidePoint.TryGetComponent(out ILightAffectable lightAffectable))
                {
                    if (!IsObjectInRadius(lightAffectable))
                    {
                        objectsInRangeSensor.Add(lightAffectable);
                        lightAffectable.SetOnPlayerRadious(true);
                    }
                    else
                    {
                        objectsToRemove.Remove(lightAffectable);
                    }
                }
            }

            // Remove objects that are no longer detected by the sensor
            foreach (var objectToRemove in objectsToRemove)
            {
                objectsInRangeSensor.Remove(objectToRemove);
                objectToRemove.SetOnPlayerRadious(false);
            }
        }
        else
        {
            // If no objects are detected by the sensor, clear the list and reset affected state
            foreach (var objectInPlayerRange in objectsInRangeSensor)
            {
                objectInPlayerRange.SetOnPlayerRadious(false);
            }
            objectsInRangeSensor.Clear();
        }


        if (losSensor.Detections.Count > 0)
        {
            List<ILightAffectable> objectsToRemove = new(objectsInLosSensor);

            foreach (var enemyHidePoint in losSensor.Detections)
            {
                if (enemyHidePoint.TryGetComponent(out ILightAffectable lightAffectable))
                {
                    if (!IsObjectInFOVtriggerRadius(lightAffectable))
                    {
                        objectsInLosSensor.Add(lightAffectable);
                        lightAffectable.SetOnPlayerFOV(true);
                    }
                    else
                    {
                        objectsToRemove.Remove(lightAffectable);
                    }
                }
            }

            // Remove objects that are no longer detected by the sensor
            foreach (var objectToRemove in objectsToRemove)
            {
                objectsInLosSensor.Remove(objectToRemove);
                objectToRemove.SetOnPlayerFOV(false);
            }
        }
        else
        {
            
            // If no objects are detected by the sensor, clear the list and reset affected state
            foreach (var objInPlayerFOV in objectsInLosSensor)
            {
                objInPlayerFOV.SetOnPlayerFOV(false);
            }
            objectsInLosSensor.Clear();
        }

        if (fovColliderSensor.Detections.Count > 0)
        {
            List<ILightAffectable> objectsToRemove = new(objectsInFovColliderSensor);

            foreach (var enemyHidePoint in fovColliderSensor.Detections)
            {
                if (enemyHidePoint.TryGetComponent(out ILightAffectable lightAffectable))
                {
                    if (!IsObjectInFovColliderSensor(lightAffectable))
                    {
                        objectsInFovColliderSensor.Add(lightAffectable);
                    }
                    else
                    {
                        objectsToRemove.Remove(lightAffectable);
                    }
                }
            }

            // Remove objects that are no longer detected by the sensor
            foreach (var objectToRemove in objectsToRemove)
            {
                objectsInFovColliderSensor.Remove(objectToRemove);
            }
        }
        else
        {
            // If no objects are detected by the sensor, clear the list and reset affected state
            objectsInFovColliderSensor.Clear();
        }

    }

    private bool IsObjectInRadius(ILightAffectable lightAffectable)
    {
        if (objectsInRangeSensor.Contains(lightAffectable))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsObjectInFOVtriggerRadius(ILightAffectable lightAffectable)
    {
        if (objectsInLosSensor.Contains(lightAffectable))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsObjectInFovColliderSensor(ILightAffectable lightAffectable)
    {
        if (objectsInFovColliderSensor.Contains(lightAffectable))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static List<EnemyHidePoint> GetLightNotAffectedWithInPlayerRadius()
    {
        var enemyHidePoints = objectsInRangeSensor
            .Where(lightAffectable => !lightAffectable.IsAffectedByLight)
            .OfType<EnemyHidePoint>()
            .ToList();

        return enemyHidePoints;
    }

    public static List<EnemyHidePoint> GetLightNotAffectedAndNotInPlayerFOV()
    {
        var enemyHidePoints = FindObjectsOfType<EnemyHidePoint>()    // After Give room id
            .Where(lightAffectable => !lightAffectable.IsAffectedByLight && !lightAffectable.IsOnPlayerFov)
            .OfType<EnemyHidePoint>()
            .ToList();

        return enemyHidePoints;
    }

    public static List<EnemyHidePoint> GetLightNotAffectedAndInPlayerFOV()
    {
        var enemyHidePoints = objectsInFovColliderSensor    // After Give room id
            .Where(lightAffectable => !lightAffectable.IsAffectedByLight && !lightAffectable.IsOnPlayerFov)
            .OfType<EnemyHidePoint>()
            .ToList();

        Debug.Log(enemyHidePoints.Count);

        return enemyHidePoints;
    }

    private void OnDisable()
    {
        rangeSensor.Clear();
        losSensor.Clear();
        fovColliderSensor.Clear();
    }

}
