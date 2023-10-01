using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InterestPoint : MonoBehaviour
{
    [SerializeField] private string showDownDialoge;            // text That will exaplain Interest point 
    [SerializeField] private Transform panelTransformPrefab;    // prefab Container Used to show text
    private TextShowUpPanel showUpPanel;                        // show Up panel template
    private ObjectRotator objectRotator;
    private ObjectZoomer objectZoomer;
    private Interactor playerInteractor;


    private void Awake()
    {
        objectRotator = GetComponentInParent<ObjectRotator>();
        objectZoomer = GetComponentInParent<ObjectZoomer>();
        playerInteractor = FindObjectOfType<FirstPersonController>().gameObject.GetComponent<Interactor>();

    }
    private void Start()
    {
        PrepareShowUpPanel();
    }

    private void PrepareShowUpPanel()
    {
        //Intantiating Panel And IniTializing Script
        var showUpPanelObj = Instantiate(panelTransformPrefab);
        showUpPanel = showUpPanelObj.GetComponent<TextShowUpPanel>();

        showUpPanel.SetShowUpText(showDownDialoge);
        // Disabling Panel At Start
        showUpPanel.gameObject.SetActive(false);
        // positioning Panel
        showUpPanel.GetPanelTransform().DOAnchorPos(new Vector2(0f, -300f), 0.1f);
    }

    //private void OnMouseOver()
    //{
    //    if(!objectRotator.IsRotatingObject() && !objectZoomer.IsZoomingObject() &&  playerInteractor.HasHoldingObject())
    //    {
    //        showUpPanel.gameObject.SetActive(true);
    //    }
    //    showUpPanel.gameObject.SetActive(true);
    //}

    internal void ShowPointDetails()
    {
        if (!objectRotator.IsRotatingObject() && !objectZoomer.IsZoomingObject() && playerInteractor.HasHoldingObject())
        {
            showUpPanel.gameObject.SetActive(true);
        }
    }

    internal void HidePointDetails()
    {
        showUpPanel.gameObject.SetActive(false);
    }

    //private void OnMouseExit()
    //{
    //    showUpPanel.gameObject.SetActive(false);
    //}


}

