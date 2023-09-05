using UnityEngine;

public class TorchLightToggle : MonoBehaviour
{
    [SerializeField] private GameObject lightSource;
    private bool isLightOn = false;

    private void Start()
    {
        lightSource.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isLightOn = !isLightOn;

            lightSource.SetActive(isLightOn);
        }
    }
}
