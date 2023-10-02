using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
    public static EquipManager Instance;

    [SerializeField] private GatherableSO torchSO;
    [SerializeField] private Transform torchEquip;
    [SerializeField] private Transform equipmentsHolder;
    private bool isGotTorch = false;
    private bool isTorchActivated;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        InputManager.Instance.OnTorchKeyPerformed += InputManager_Instance_OnTorchKeyPerformed;

        DisableEquipedItems();
    }

    public void DisableEquipedItems()
    {
        foreach(Transform child in equipmentsHolder)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void EnableEquipedItems()
    {
        foreach(Transform child in equipmentsHolder)
        {
            child.gameObject.SetActive(true);
        }
    }

    private void InputManager_Instance_OnTorchKeyPerformed(object sender, System.EventArgs e)
    {
        TorchToggle();
    }

    private void TorchToggle()
    {
        if(HasTorchInInventory() || isGotTorch)
        {
           isTorchActivated = !isTorchActivated;
        }
        
        if(isTorchActivated)
        {
            if (HasTorchInInventory() || isGotTorch)
            {
                // Has Torch in inventory or already got 
                EquipTorch();
            }
        }
        else
        {
            SetActiveTorch(false);
        }

    }

    private void EquipTorch()
    {
       SetActiveTorch(true);
       RemoveTorchFromInventory();
       isGotTorch = true;
    }

    public void SetActiveTorch(bool active)
    {
        torchEquip.gameObject.SetActive(active);
    }

    private bool HasTorchInInventory()
    {

        GatherableSO[] gatherableSOs = Inventory.Instance.GetGatheredObjectList().ToArray();
        var hasTorch = gatherableSOs.Any(s => s == torchSO);
        if (hasTorch)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void RemoveTorchFromInventory()
    {
        if (isGotTorch) return;

        GatherableSO[] gatherableSOs = Inventory.Instance.GetGatheredObjectList().ToArray();
        var torch = gatherableSOs.Where(s => s.gatherableObjectName == torchSO.gatherableObjectName).FirstOrDefault(); ;
        Inventory.Instance.RemoveObjectFromInventory(torch);  // remove key from inventory
    }

    private void OnDisable()
    {
        InputManager.Instance.OnTorchKeyPerformed -= InputManager_Instance_OnTorchKeyPerformed;
    }
}
