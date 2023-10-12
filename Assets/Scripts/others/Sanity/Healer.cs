using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour
{
    private bool canHeal = false;
    public void ActivateHealing()
    {
        canHeal = true;
    }
    public void DeActivateHealing()
    {
        canHeal = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (!canHeal) return;

        if (other.TryGetComponent(out PlayerFearSystem playerFearSystem))
        {
            Debug.Log("Player Entered Heal Point");
            playerFearSystem.SetPlayerFearState(PlayerFearSystem.PlayerFearState.Healing);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerFearSystem playerFearSystem))
        {
            Debug.Log("Player Exited Heal Point");
            playerFearSystem.SetPlayerFearState(PlayerFearSystem.PlayerFearState.Idle);
        }
    }
}
