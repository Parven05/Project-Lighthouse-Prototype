using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // This Input Manager Will Take All Resposible For Player Inputs Send Datas Through Events Decouped For Logic
    public static InputManager Instance { get; private set; }
    public event EventHandler OnInteractionKeyPerformed;     // This Event Will Be Invoked By Pressing E Key 

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            OnInteractionKeyPerformed?.Invoke(this, EventArgs.Empty);
        }
    }
}
