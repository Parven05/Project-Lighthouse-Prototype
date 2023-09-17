using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextShowUpPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI prompText;
    [SerializeField] private RectTransform panelTransform;
    public void SetShowUpText(string text)
    {
        prompText.text = text;
    }
    public RectTransform GetPanelTransform()
    {
        return panelTransform;
    }

}
