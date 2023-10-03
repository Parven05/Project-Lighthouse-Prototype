using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour
{
    public static event EventHandler OnHealingStarted;
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
            playerFearSystem.NormalizeFearLevel();

            OnHealingStarted?.Invoke(this,EventArgs.Empty);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerFearSystem playerFearSystem))
        {
            Debug.Log("Player Exited Heal Point");
        }
    }
}
