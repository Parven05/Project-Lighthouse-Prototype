using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Image indigationImage;
    [SerializeField] private TextMeshProUGUI playerCurrentHealthText;
    [SerializeField] private HealthSystem playerHealthSystem;

    private void Update()
    {
        if(indigationImage != null)
        {
           indigationImage.fillAmount = playerHealthSystem.GetHealth() / playerHealthSystem.GetMaxHealth();
        }
        if(playerCurrentHealthText != null)
        {
           playerCurrentHealthText.text = "Player Health" +":"+ Mathf.RoundToInt(playerHealthSystem.GetHealth());
        }
    }
}
