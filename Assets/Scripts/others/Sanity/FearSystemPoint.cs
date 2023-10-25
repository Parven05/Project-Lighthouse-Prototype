using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FearSystemPoint : MonoBehaviour
{
    [SerializeField] private float maxDarknessDistance = 20f;
    [SerializeField] private PlayerFearSystem.FearLevel fearLevel;

    //[SerializeField] private float minFearIncreaseRate = 1.0f; // Rate at which fear increases per second
    //[SerializeField] private float maxFearIncreaseRate = 5.0f; // Rate at which fear increases per second

    //private Transform targetvictimTranform;
    //private PlayerFearSystem playerFearSystem;


    private void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent(out PlayerFearSystem playerFearSystem))
        {
            //Debug.Log("Player Entered");
            playerFearSystem.SetPlayerFearState(PlayerFearSystem.PlayerFearState.OnFear,fearLevel);
            
            //targetvictimTranform = playerFearSystem.gameObject.transform;
            //this.playerFearSystem = playerFearSystem;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerFearSystem playerFearSystem))
        {
            Debug.Log("Player Exited");
            playerFearSystem.SetPlayerFearState(PlayerFearSystem.PlayerFearState.Idle);
            //targetvictimTranform = null;
            //playerFearSystem = null;
        }
    }

    //private void Update()
    //{
    //    if (targetvictimTranform != null)
    //    {
    //        float deepDarkToVictimDistance = Vector3.Distance(transform.position, targetvictimTranform.position);

    //        if (deepDarkToVictimDistance < maxDarknessDistance)
    //        {
    //            // Calculate the fear increase rate based on distance
    //            float t = 1.0f - Mathf.Clamp01(deepDarkToVictimDistance / maxDarknessDistance); // Invert the clamp
    //            float currentFearIncreaseRate = Mathf.Lerp(minFearIncreaseRate, maxFearIncreaseRate, t);

    //            playerFearSystem.IncreaseFearLevel(currentFearIncreaseRate * Time.deltaTime); // Increase fear over time
    //        }

    //    }
    //}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, maxDarknessDistance);

    }
}
