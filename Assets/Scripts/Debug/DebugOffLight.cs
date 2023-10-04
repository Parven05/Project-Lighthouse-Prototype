using UnityEngine;

public class DebugOffLight : MonoBehaviour
{
    [SerializeField] private GameObject directionalLight;

    private void OnTriggerEnter(Collider other)
    {
        directionalLight.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        directionalLight.SetActive(true);
    }
}
