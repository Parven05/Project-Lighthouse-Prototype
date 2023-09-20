using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField] private GatherableSO batterySO;   // Ref to the BatterySo 
    [SerializeField] private Light torchFlash;         // Ref to the BatterySo 
    private GatherableSO currentUsingBatterySO;
    [SerializeField] private float currentBatteryHealth = 0;
    private bool canUseTorch = false;
    private void Start()
    {
        currentUsingBatterySO = Instantiate(batterySO);            // Creating new instance So scene Pickup BAtteries Dont Affect
        InputManager.Instance.OnReloadKeyPerformed += InputManager_Instance_OnReloadKeyPerformed;

        currentBatteryHealth = currentUsingBatterySO.value;
    }

    private void InputManager_Instance_OnReloadKeyPerformed(object sender, EventArgs e)
    {
        if (Inventory.Instance.TryGetGatherableObject(batterySO,out GatherableSO newBatterySO))
        {
            RemoveOldBatery();
            LoadNewBattery(newBatterySO);
        }
        else
        {
            Debug.Log("Dont Have Batteries To Reload");
        }
    }

    private void Update()
    {
        currentBatteryHealth -= Time.deltaTime;

        if(currentBatteryHealth <  0)
        {
            currentBatteryHealth = 0;
            canUseTorch = false;
        }
        else
        {
            canUseTorch = true;
        }

        if(canUseTorch)
        {
            Debug.Log("Light working");
            SetActiveFlash(true);
        }
        else
        {
            Debug.Log("Light battery dead");
            SetActiveFlash(false);
        }
    }

    private void RemoveOldBatery()
    {
       currentUsingBatterySO = null;
       Debug.Log("Old Battery Removed");
    }
    private void LoadNewBattery(GatherableSO batterySO)
    {
       currentUsingBatterySO = batterySO;
       currentBatteryHealth = currentUsingBatterySO.value;
       Debug.Log("New Battery Loaded");
    }

    private void SetActiveFlash(bool active)
    {
        torchFlash.enabled = active;
    }

    private void OnDisable()
    {
       // Inventory.Instance.OnGatherableObjectModifiedInInventory -= CheckBatteryExist;
    }
}
