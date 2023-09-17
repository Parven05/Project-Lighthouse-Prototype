using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryIconTemplate : MonoBehaviour
{
    public static Action<GatherableSO> OnAnyObjectUsedAndRemoved;

    [SerializeField] private TextMeshProUGUI itemQuantitytext;
    private string itemName;
    [SerializeField] private Image itemIconImage;

    private GatherableSO gatherableSO;


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
        Debug.Log("item" +  itemName +" "+ " Used");
        OnAnyObjectUsedAndRemoved?.Invoke(gatherableSO);
        Destroy(gameObject);
    }

    //private void OnDisable()
    //{
    //    OnAnyObjectUsedAndRemoved = null;
    //}
}
