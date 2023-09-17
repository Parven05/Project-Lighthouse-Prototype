using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance { get; private set; }
    [SerializeField] private Transform depthofField;
    private void Awake()
    {
        Instance = this;
        SetPostProccessingExaminaionBlurEnabled(false);
    }
    private void Start()
    {
        InventoryUI.Instance.OnAddObjectToInventoryBtnClicked += AddGatherableObjectToInventory;
    }

    private void AddGatherableObjectToInventory(object sender, EventArgs e)
    {
        SetPostProccessingExaminaionBlurEnabled(false);
    }

    public void SetPostProccessingExaminaionBlurEnabled(bool enabledStatus)
    {
        depthofField.gameObject.SetActive(enabledStatus);
    }

    private void OnDisable()
    {
        InventoryUI.Instance.OnAddObjectToInventoryBtnClicked -= AddGatherableObjectToInventory;
    }
}
