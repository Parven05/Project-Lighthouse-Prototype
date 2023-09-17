using System;
using System.Collections.Generic;
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

    private void RemoveGatherableObjectFromInventoryList(GatherableSO gatherableSO)
    {
       gatheredSoList.Remove(gatherableSO);
       OnGatherableObjectModifiedInInventory?.Invoke(gatheredSoList);
    }

    private void AddGatherableObjectToInventory(object sender, EventArgs e)
    {
       gatheredSoList.Add(playerSelectedGatherableObject.GetGatherableSO());
       Debug.Log("Successfully added To Inventory " + playerSelectedGatherableObject.GetGatherableSO().gatherableObjectName);

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

    private void OnDisable()
    {
        playerInteractor.onGatherableObjectPicked -= ShowPickUpDropUI;
        playerInteractor.oGatherableObjectDropped -= ClosePickUpDropUI;

        InventoryUI.Instance.OnAddObjectToInventoryBtnClicked -= AddGatherableObjectToInventory;
        InventoryIconTemplate.OnAnyObjectUsedAndRemoved -= RemoveGatherableObjectFromInventoryList;
    }
}
