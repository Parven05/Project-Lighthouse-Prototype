using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryIconTemplate : MonoBehaviour
{
    public static Action<GatherableSO> OnAnyObjectUsedAndRemoved;

    [SerializeField] private Button useBtn, removeBtn;
    [SerializeField] private TextMeshProUGUI itemQuantitytext;
    private string itemName;
    [SerializeField] private Image itemIconImage;

    private GatherableSO gatherableSO;
    private void Awake()
    {
        useBtn.onClick.AddListener(() =>
        {
            UseItem();
        });

        removeBtn.onClick.AddListener(() =>
        {
            Inventory.Instance.RemoveObjectFromInventory(gatherableSO);
            Destroy(this.gameObject);
        });
    }


    public void SetUpItemTemplatePropsUI(GatherableSO gatherableSO,int itemQuantity)
    {
        this.gatherableSO = gatherableSO; 
        itemName = gatherableSO.gatherableObjectName;
        itemIconImage.sprite = gatherableSO.gatherableImageSprite;
        if(itemQuantity > 1) // Object not multiple time Gatherable 
        {
            itemQuantitytext.text = itemQuantity.ToString();
        }
        else
        {
            // Disable item Count Text
            itemQuantitytext.gameObject.SetActive(false);
        }

    }

    public void UseItem()
    {
        if(TryUseItem(gatherableSO))
        {
            Debug.Log("item" + itemName + " " + " Used");
            OnAnyObjectUsedAndRemoved?.Invoke(gatherableSO);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("item" + itemName + " " + " Not Selected Any type Or Different item");
        }
        
    }

    private bool TryUseItem(GatherableSO gatherableSO)
    {
        GatherableObjectType gatherableObjectType = gatherableSO.gatherableType;
        switch (gatherableObjectType)
        {
            case GatherableObjectType.Healable:
                // Assuming you have a HealthSystem component on the player
                HealthSystem healthSystem = FindObjectOfType<FirstPersonController>().gameObject.GetComponent<HealthSystem>();
                healthSystem.AddHealth(gatherableSO.value);
                break;
            case GatherableObjectType.Usable:
                // Implement logic for Usable items here
                break;
            case GatherableObjectType.Collectable:
                // Implement logic for Collectable items here
                break;
            case GatherableObjectType.Equipable:
                // Implement logic for Equipable items here
                break;
            default:
                // Handle the case where the item type is not recognized
                return false;
        }

        // Return true to indicate that the item was successfully used
        return true;
    }

    //private void OnDisable()
    //{
    //    useBtn.onClick.RemoveAllListeners();
    //    removeBtn.onClick.RemoveAllListeners();
    //}

}
