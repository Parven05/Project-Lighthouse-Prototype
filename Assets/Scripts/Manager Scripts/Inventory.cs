using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }
    public Action<List<GatherableSO>> OnGatherableObjectModifiedInInventory;  //When Object Added or Removed

    private List<GatherableSO> gatheredSoList;               // List For The Inventory Gatherables So Datas

    private void Awake()
    {
        Instance = this;
        gatheredSoList = new List<GatherableSO>();
       
    }
    private void Start()
    {
        InventoryIconTemplate.OnAnyObjectUsedAndRemoved += RemoveObjectFromInventory;
    }

    public void RemoveObjectFromInventory(GatherableSO gatherableSO)
    {
       gatheredSoList.Remove(gatherableSO);
       OnGatherableObjectModifiedInInventory?.Invoke(gatheredSoList);
    }

    public void AddObjectToInventory(GatherableObject gatherableObject)
    {
        gatheredSoList.Add(gatherableObject.GetGatherableSO());

        // Call Ui For Update Icons
        OnGatherableObjectModifiedInInventory?.Invoke(gatheredSoList);

        // Destroy That Gathered Object
        Destroy(gatherableObject.gameObject);
    }

    public List<GatherableSO> GetGatheredObjectList()
    {
        return gatheredSoList;
    }

    public bool TryGetGatherableObject(GatherableSO gatherableSOInput,out GatherableSO batterySO)
    {
        if(gatheredSoList.Any(s => s == gatherableSOInput)) // founded That Object 
        {
            var batterySo = gatheredSoList.Where(s => s == gatherableSOInput).FirstOrDefault(); // Get That Founded
            batterySO = batterySo;  // Pass Through Parameter
            RemoveObjectFromInventory(batterySo);
            return true;
        }
        else
        {
            batterySO = null;
            return false;
        }
    }

    private void OnDisable()
    {
        InventoryIconTemplate.OnAnyObjectUsedAndRemoved -= RemoveObjectFromInventory;
    }


}
