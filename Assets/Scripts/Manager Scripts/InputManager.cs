using System;
using System.Linq;
using UnityEngine;

public class InputManager : MonoBehaviour
{    // This Input Manager Will Take All Resposible For Player Inputs Send Datas Through Events Decouped For Logic
    public static InputManager Instance { get; private set; }
    public event EventHandler OnInteractionKeyPerformed;     // This Event Will Be Invoked By Pressing E Key 
    public event EventHandler OnInventoyKeyPerformed;        // This Event Will Be Invoked By Pressing I Key 
    public event EventHandler OnTorchKeyPerformed;           // This Event Will Be Invoked By Pressing F Key 
    public event EventHandler OnReloadKeyPerformed;          // This Event Will Be Invoked By Pressing R Key 

    public enum ExamineObjectRotateType
    {
        MouseControl,
        KeyPadControl
    }
    public enum ExamineObjectZoomType
    {
        MouseControl,
        KeyPadControl
    }
    public ExamineObjectRotateType examineControlType;
    [Space]
    [Header("Keys for Object Examination Rotation")]
    public KeyCode[] UpDirKeys;
    public KeyCode[] DownDirKeys;
    public KeyCode[] LeftDirKeys;
    public KeyCode[] RightDirKeys;
    [Space]
    [Header("Keys for Object Examination Zoom")]
    public ExamineObjectZoomType examineObjectZoomType;
    public KeyCode[] ZoomPlusKeys;
    public KeyCode[] ZoomMinusKeys;
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

        if(Input.GetKeyDown(KeyCode.I))
        {
            OnInventoyKeyPerformed?.Invoke(this, EventArgs.Empty);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            OnTorchKeyPerformed?.Invoke(this, EventArgs.Empty);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            OnReloadKeyPerformed?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsPressingUpDirectionKeys()
    {
        return UpDirKeys.Any(key => Input.GetKey(key));
    }
    public bool IsPressingDownDirectionKeys()
    {
        return DownDirKeys.Any(key => Input.GetKey(key));
    }
    public bool IsPressingLeftDirectionKeys()
    {
        return LeftDirKeys.Any(key => Input.GetKey(key));
    }
    public bool IsPressingRightDirectionKeys()
    {
        return RightDirKeys.Any(key => Input.GetKey(key));
    }


    public bool IsPressingZoomInKey()
    {
        return ZoomPlusKeys.Any(key => Input.GetKey(key));
    }
    public bool IsPressingZoomOutKey()
    {
        return ZoomMinusKeys.Any(key => Input.GetKey(key));
    }
}
