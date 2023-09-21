using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TorchUI : MonoBehaviour
{
    [SerializeField] private Image batteryIndigatorImage;
    [SerializeField] private Transform batteryIndigatorParent;
    [SerializeField] private Torch torch;

    private void Start()
    {
        torch.OnTorchEquiped += Torch_OnTorchEquiped;
        torch.OnTorchUnEquiped += Torch_OnTorchUnEquiped;

        SetActiveTorchIndigation(false);
    }

    private void Torch_OnTorchUnEquiped(object sender, System.EventArgs e)
    {
        SetActiveTorchIndigation(false);
    }

    private void Torch_OnTorchEquiped(object sender, System.EventArgs e)
    {
        SetActiveTorchIndigation(true);
    }

    private void LateUpdate()
    {
        if(torch != null && batteryIndigatorImage != null)
        {
            batteryIndigatorImage.fillAmount = torch.CurrentBatteryHealth() / torch.GetBatteryMaxHealth();
        }
        else
        {
            Debug.LogError("No Refs");
        }
    }

    private void SetActiveTorchIndigation(bool active)
    {
        batteryIndigatorParent.gameObject.SetActive(active);
    }

    private void OnDisable()
    {
        torch.OnTorchEquiped -= Torch_OnTorchEquiped;
        torch.OnTorchUnEquiped -= Torch_OnTorchUnEquiped;
    }
}
