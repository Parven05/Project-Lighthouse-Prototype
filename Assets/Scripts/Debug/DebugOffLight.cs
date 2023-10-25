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
        if(other.gameObject.TryGetComponent(out FirstPersonController _))
        {
            directionalLight.SetActive(false);
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out FirstPersonController _))
        {
            directionalLight.SetActive(true);
        }
    }
}
