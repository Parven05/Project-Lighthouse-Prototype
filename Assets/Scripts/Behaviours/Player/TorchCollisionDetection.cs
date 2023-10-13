using System;
using UnityEngine;

public class TorchCollisionDetection : MonoBehaviour
{
    public event EventHandler OnTorchFacingEnemy;
    public event EventHandler OnTorchNotFacingEnemy;

    [SerializeField] private bool isTorchFacingEnemy = false;

    //public bool IsTorchFacingEnemy()
    //{
    //    return isTorchFacingEnemy;
    //}

    public void SetEnemyDetected(bool isEnemyDetected)
    {
        isTorchFacingEnemy = isEnemyDetected;

        if(isEnemyDetected)
        {
            OnTorchFacingEnemy?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnTorchNotFacingEnemy?.Invoke(this, EventArgs.Empty);
        }
    }

}
