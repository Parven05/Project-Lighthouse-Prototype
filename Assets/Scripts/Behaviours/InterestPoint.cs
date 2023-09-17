using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InterestPoint : MonoBehaviour
{
    [SerializeField] private string showDownDialoge;            // text That will exaplain Interest point 
    [SerializeField] private Transform panelTransformPrefab;    // prefab Container Used to show text
    private TextShowUpPanel showUpPanel;                        // show Up panel template
    private ObjectRotator objectRotator;
    private ObjectZoomer objectZoomer;
    private Interactor playerInteractor;
    private void Start()
    {
        objectRotator = FindObjectOfType<ObjectRotator>();
        objectZoomer = FindObjectOfType<ObjectZoomer>();
        playerInteractor = FindObjectOfType<Interactor>();

        PrepareShowUpPanel(); 
    }

    private void PrepareShowUpPanel()
    {
        var showUpPanelObj = Instantiate(panelTransformPrefab);
        showUpPanel = showUpPanelObj.GetComponent<TextShowUpPanel>();
        showUpPanel.SetShowUpText(showDownDialoge);
        showUpPanel.gameObject.SetActive(false);
        showUpPanel.GetPanelTransform().DOAnchorPos(new Vector2(0f, -300f), 0.1f);
    }

    private void OnMouseOver()
    {
        if(!objectRotator.IsRotatingObject() && !objectZoomer.IsZoomingObject() && playerInteractor.HasHoldingObject())
        {
            showUpPanel.gameObject.SetActive(true);
        } 
        
    }
    private void OnMouseExit()
    {
        showUpPanel.gameObject.SetActive(false);
    }
}

