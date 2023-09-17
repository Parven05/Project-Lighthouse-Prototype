using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;
    public event EventHandler OnAddObjectToInventoryBtnClicked;

    [SerializeField] private Button addObjectToInventoryBtn;
    [SerializeField] private Transform invertoryObjectsUi;
    [SerializeField] private Transform inventoryIconTemplatePrefab;
    [SerializeField] private Transform iconHolderContainerRect;

    private bool isInventoryUiOpened=false;
    private void Awake()
    {
        Instance = this;

        addObjectToInventoryBtn.onClick.AddListener(() =>
        {
            OnAddObjectToInventoryBtnClicked?.Invoke(this, EventArgs.Empty);
        });

        invertoryObjectsUi.gameObject.SetActive(false);
    }
    private void Start()
    {
        DisableAddObjectInventoryBtn();

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
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            invertoryObjectsUi.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void DisableAddObjectInventoryBtn()
    {
        addObjectToInventoryBtn.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        var fpsControl = FindObjectOfType<FirstPersonController>();
        fpsControl.cameraCanMove = true;
        fpsControl.playerCanMove = true;
    }

    public void EnableAddObjectInventoryBtn()
    {
        addObjectToInventoryBtn.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnDisable()
    {
        addObjectToInventoryBtn.onClick.RemoveAllListeners();
        InputManager.Instance.OnInventoyKeyPerformed -= InputManager_Instance_OnInventoyKeyPerformed;
    }
}
