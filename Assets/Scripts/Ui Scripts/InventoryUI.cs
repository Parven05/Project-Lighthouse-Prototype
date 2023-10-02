using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    [SerializeField] private Button inventoryBtn,exitBtn;
    [SerializeField] private Transform invertoryObjectsUi;
    [SerializeField] private Transform inventoryIconTemplatePrefab;
    [SerializeField] private Transform iconHolderContainerRect;

    private bool isInventoryUiOpened=false;
    private void Awake()
    {
        Instance = this;

        inventoryBtn.onClick.AddListener(() =>
        {
            ToggleInventoryUI();
        });

        exitBtn.onClick.AddListener(() =>
        {
            ToggleInventoryUI();
        });

        invertoryObjectsUi.gameObject.SetActive(false);
    }
    private void Start()
    {
        //DisableAddObjectInventoryBtn();
        DestroyOldTemplates();

        InputManager.Instance.OnInventoyKeyPerformed += InputManager_Instance_OnInventoyKeyPerformed;
        Inventory.Instance.OnGatherableObjectModifiedInInventory += UpdateUiIconTemplates;
    }

    private void UpdateUiIconTemplates(List<GatherableSO> gatheredObjSOlist)
    {
        DestroyOldTemplates();

        for (int i = 0; i < gatheredObjSOlist.Count; i++)
        {
            var iconTemplate = Instantiate(inventoryIconTemplatePrefab, iconHolderContainerRect);
            if (iconTemplate.TryGetComponent<InventoryIconTemplate>(out var inventoryIconScript))
            {
                inventoryIconScript.SetUpItemTemplatePropsUI(gatheredObjSOlist[i], 1);
            }
        }
    }

    private void DestroyOldTemplates()
    {
        foreach (Transform child in iconHolderContainerRect)
        {
            Destroy(child.gameObject);
        }
    }

    private void InputManager_Instance_OnInventoyKeyPerformed(object sender, EventArgs e)
    {
        ToggleInventoryUI();
    }

    private void ToggleInventoryUI()
    {
        isInventoryUiOpened = !isInventoryUiOpened;

        if (isInventoryUiOpened)
        {
            invertoryObjectsUi.gameObject.SetActive(true);
            inventoryBtn.gameObject.SetActive(false);
            FirstPersonController.SetCurserLockMode(false);
        }
        else
        {
            invertoryObjectsUi.gameObject.SetActive(false);
            inventoryBtn.gameObject.SetActive(true);
            FirstPersonController.SetCurserLockMode(true);
        }
    }

    //public void DisableAddObjectInventoryBtn()
    //{
    //    inventoryBtn.gameObject.SetActive(false);
    //    FirstPersonController.SetCurserLockMode(true);
    //    var fpsControl = FindObjectOfType<FirstPersonController>();
    //    fpsControl.cameraCanMove = true;
    //    fpsControl.playerCanMove = true;
    //}

    //public void EnableAddObjectInventoryBtn()
    //{
    //    inventoryBtn.gameObject.SetActive(true);
    //    FirstPersonController.SetCurserLockMode(false);
    //}

    private void OnDisable()
    {
        inventoryBtn.onClick.RemoveAllListeners();
        exitBtn.onClick.RemoveAllListeners();
        InputManager.Instance.OnInventoyKeyPerformed -= InputManager_Instance_OnInventoyKeyPerformed;
    }
}
