using System;
using UnityEngine;

public class DebugOffLight : MonoBehaviour
{
    [SerializeField] private GameObject directionalLight;

    private bool isLightOn = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            isLightOn = !isLightOn;
            directionalLight.SetActive(isLightOn);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        directionalLight.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        directionalLight.SetActive(true);
    }
}
