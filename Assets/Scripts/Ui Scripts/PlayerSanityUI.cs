using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSanityUI : MonoBehaviour
{
    [SerializeField] private Image indigationImage;
    [SerializeField] private TextMeshProUGUI playerCurrentFearLevelText;
    [SerializeField] private PlayerFearSystem playerFearSystem;

    private void Update()
    {
        if (indigationImage != null)
        {
            indigationImage.fillAmount = playerFearSystem.GetFearLevel() / playerFearSystem.GetFearMax();
        }
        else
        {
            Debug.LogError("No References For Ui Indigation Image");
        }


        if (playerCurrentFearLevelText != null)
        {
            playerCurrentFearLevelText.text = "Player Fear Level" + ":" + Mathf.RoundToInt(playerFearSystem.GetFearLevel());
        }
        else
        {
            Debug.LogError("No References For Ui Text");
        }
    }
}


