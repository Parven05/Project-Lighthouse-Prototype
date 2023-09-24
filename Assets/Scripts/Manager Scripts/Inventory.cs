using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }
    public Action<List<GatherableSO>> OnGatherableObjectModifiedInInventory;  //When Object Added or Removed

    private List<GatherableSO> gatheredSoList;               // List For The Inventory Gatherables So Datas
    private GatherableObject playerSelectedGatherableObject;

    [SerializeField] private Interactor playerInteractor;

    private void Awake()
    {
        Instance = this;
        gatheredSoList = new List<GatherableSO>();
    }
    private void Start()
    {
        playerInteractor.onGatherableObjectPicked += ShowPickUpDropUI;
        playerInteractor.oGatherableObjectDropped += ClosePickUpDropUI;

        InventoryUI.Instance.OnAddObjectToInventoryBtnClicked += AddGatherableObjectToInventory;
        InventoryIconTemplate.OnAnyObjectUsedAndRemoved += RemoveGatherableObjectFromInventoryList;
    }

    public void RemoveGatherableObjectFromInventoryList(GatherableSO gatherableSO)
    {
       gatheredSoList.Remove(gatherableSO);
       OnGatherableObjectModifiedInInventory?.Invoke(gatheredSoList);
    }

    private void AddGatherableObjectToInventory(object sender, EventArgs e)
    {
       gatheredSoList.Add(playerSelectedGatherableObject.GetGatherableSO());

       OnGatherableObjectModifiedInInventory?.Invoke(gatheredSoList);

       ClosePickUpDropUI();
       Destroy(playerSelectedGatherableObject.gameObject);
    }

    private void ShowPickUpDropUI(GatherableObject gatherableObject)
    {
        SetCurrentHoldingObject(gatherableObject);
        InventoryUI.Instance.EnableAddObjectInventoryBtn();
    }

    private void SetCurrentHoldingObject(GatherableObject gatherableObject)
    {
        this.playerSelectedGatherableObject = gatherableObject;
    }

    private void ClosePickUpDropUI()
    {
        InventoryUI.Instance.DisableAddObjectInventoryBtn();
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
            RemoveGatherableObjectFromInventoryList(batterySo);
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
        playerInteractor.onGatherableObjectPicked -= ShowPickUpDropUI;
        playerInteractor.oGatherableObjectDropped -= ClosePickUpDropUI;

        InventoryUI.Instance.OnAddObjectToInventoryBtnClicked -= AddGatherableObjectToInventory;
        InventoryIconTemplate.OnAnyObjectUsedAndRemoved -= RemoveGatherableObjectFromInventoryList;
    }

    public GatherableObjectType GetObjectCatagory(GatherableSO gatherableSO)
    {
       return gatherableSO.gatherableType;
    }
}
