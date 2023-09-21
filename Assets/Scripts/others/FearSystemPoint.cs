using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FearSystemPoint : MonoBehaviour
{
    [SerializeField] private List<Transform> intensiveDarkerPoints;

    private void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent(out HealthSystem healthSystem))
        {
            healthSystem.DecreaseHealth(0.5f * Time.deltaTime);
        }
    }
}
