using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    Transform GetInteractObjectPos();
    void Interact();

    void SetActiveSelectedVisual(bool activeStatus);
}
