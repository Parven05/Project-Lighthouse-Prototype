using Micosmo.SensorToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TorchCollisionDetection : MonoBehaviour
{
    [SerializeField] private Sensor losSensor;
    [SerializeField] private Sensor fovSensor;
    [SerializeField] private Sensor triggerSensor;

    private List<ILightAffectable> objectsInLos;
    private List<ILightAffectable> objectsInFov;

    private void Awake()
    {
        objectsInLos = new List<ILightAffectable>();
        objectsInFov = new List<ILightAffectable>();
    }

    private void Update()
    {

        if (losSensor.Detections.Count > 0)
        {
            List<ILightAffectable> objectsToRemove = new List<ILightAffectable>(objectsInLos);

            foreach (var enemyHidePoint in losSensor.Detections)
            {
                if (enemyHidePoint.TryGetComponent(out ILightAffectable lightAffectable))
                {
                    if (!IsObjectInLOS(lightAffectable))
                    {
                        objectsInLos.Add(lightAffectable);
                        lightAffectable.SetLightAffected(true);
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
                objectsInLos.Remove(objectToRemove);
                objectToRemove.SetLightAffected(false);
            }
        }
        else
        {
            // If no objects are detected by the sensor, clear the list and reset affected state
            foreach (var lightAffectable in objectsInLos)
            {
                lightAffectable.SetLightAffected(false);
            }
            objectsInLos.Clear();
        }

        // ...


        if(fovSensor.Detections.Count > 0)
        {
            List<ILightAffectable> objectsToRemoveInFovSensor = new List<ILightAffectable>(objectsInFov);

            foreach (var enemyHidePoint in fovSensor.Detections)
            {
                if (enemyHidePoint.TryGetComponent(out ILightAffectable lightAffectable))
                {
                    if (!IsObjectInFOV(lightAffectable))
                    {
                       objectsInFov.Add(lightAffectable);
                    }
                    else
                    {
                        objectsToRemoveInFovSensor.Remove(lightAffectable);
                    }
                }
            }

            // Remove objects that are no longer detected by the sensor
            foreach (var objectToRemove in objectsToRemoveInFovSensor)
            {
                objectsInFov.Remove(objectToRemove);
            }
        }
        else
        {
            objectsInFov.Clear();
        }


        //Debug.Log(GetLightNotAffectedWithInTorchFOV().Count);


    }

    private bool IsObjectInLOS(ILightAffectable lightAffectable)
    {
        if(objectsInLos.Contains(lightAffectable))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool IsObjectInFOV(ILightAffectable lightAffectable)
    {
        if (objectsInFov.Contains(lightAffectable))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public List<EnemyHidePoint> GetLightNotAffectedWithInTorchFOV()
    {
        var enemyHidePoints = objectsInFov
            .Where(lightAffectable => !lightAffectable.IsAffectedByLight)
            .OfType<EnemyHidePoint>()
            .ToList();

        return enemyHidePoints;
    }

    private void OnDisable()
    {
        ClearDetectionsLightAffected();
    }

    public void ClearDetectionsLightAffected()
    {
        // clear Obs From List
        foreach (var lightAffectable in objectsInLos)
        {
            lightAffectable.SetLightAffected(false);
        }
        objectsInLos.Clear();

        foreach (var lightAffectable in objectsInFov)
        {
            lightAffectable.SetOnPlayerFOV(false);
        }
        objectsInFov.Clear();

        //Clear Objs From Sensor
        losSensor.SignalProcessors.Clear();
        fovSensor.SignalProcessors.Clear();
        triggerSensor.SignalProcessors.Clear();

        fovSensor.Clear();
        losSensor.Clear();
        triggerSensor.Clear();
        
    }
}
