using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{    // This Input Manager Will Take All Resposible For Player Inputs Send Datas Through Events Decouped For Logic
    public static InputManager Instance { get; private set; }
    public event EventHandler OnInteractionKeyPerformed;     // This Event Will Be Invoked By Pressing E Key 
    public event EventHandler OnInventoyKeyPerformed;        // This Event Will Be Invoked By Pressing I Key 
    public event EventHandler OnTorchKeyPerformed;           // This Event Will Be Invoked By Pressing F Key 
    public event EventHandler OnReloadKeyPerformed;          // This Event Will Be Invoked By Pressing R Key 
    public event EventHandler OnJumpKeyPerformed;            // This Event Will Be Invoked By Pressing Space Key

    public PlayerInputActions inputActions;
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
    //[Space]
    //[Header("Keys for Object Examination Rotation")]
    //public KeyCode[] UpDirKeys;
    //public KeyCode[] DownDirKeys;
    //public KeyCode[] LeftDirKeys;
    //public KeyCode[] RightDirKeys;
    //[Space]
    //[Header("Keys for Object Examination Zoom")]
    public ExamineObjectZoomType examineObjectZoomType;
    //public KeyCode[] ZoomPlusKeys;
    //public KeyCode[] ZoomMinusKeys;
    private void Awake()
    {
        Instance = this;

        inputActions = new PlayerInputActions(); // initializing Input Action
        inputActions.Player.Enable();            // Enabling Player Input

        inputActions.Player.JumpKey.performed += Jump_performed;
        inputActions.Player.InteractionKey.performed += InteractionKey_performed;
        inputActions.Player.InventoryKey.performed += InventoryKey_performed;
        inputActions.Player.FlashLightKey.performed += FlashLightKey_performed;
        inputActions.Player.ReloadKey.performed += ReloadKey_performed;
    }

    private void ReloadKey_performed(InputAction.CallbackContext context)
    {
        OnReloadKeyPerformed?.Invoke(this, EventArgs.Empty);
    }

    private void FlashLightKey_performed(InputAction.CallbackContext context)
    {
        OnTorchKeyPerformed?.Invoke(this, EventArgs.Empty);
    }

    private void InventoryKey_performed(InputAction.CallbackContext context)
    {
        OnInventoyKeyPerformed?.Invoke(this, EventArgs.Empty);
    }

    private void InteractionKey_performed(InputAction.CallbackContext context)
    {
        OnInteractionKeyPerformed?.Invoke(this, EventArgs.Empty);
    }

    private void Jump_performed(InputAction.CallbackContext context)
    {
        OnJumpKeyPerformed?.Invoke(this, EventArgs.Empty);
    }

    private void Update()  // Old Input System
    {
        //if(Input.GetKeyDown(KeyCode.E))
        //{
        //    OnInteractionKeyPerformed?.Invoke(this, EventArgs.Empty);
        //    Debug.Log("OnInteractionKeyPerformed Invokation List" + " " + OnTorchKeyPerformed.GetInvocationList().Length);
        //}

        //if(Input.GetKeyDown(KeyCode.I))
        //{
        //    OnInventoyKeyPerformed?.Invoke(this, EventArgs.Empty);
        //    Debug.Log("OnInventoyKeyPerformed Invokation List" + " " + OnTorchKeyPerformed.GetInvocationList().Length);
        //}

        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    OnTorchKeyPerformed?.Invoke(this, EventArgs.Empty);
        //    Debug.Log("OnTorchKeyPerformed Invokation List" + " " + OnTorchKeyPerformed.GetInvocationList().Length);
        //}

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    OnReloadKeyPerformed?.Invoke(this, EventArgs.Empty);
        //    Debug.Log("OnReloadKeyPerformed Invokation List" + " " + OnTorchKeyPerformed.GetInvocationList().Length);
        //}

    }

    public Vector3 GetInputAxisXAndZ()
    {
        Vector3 inputAxis = new Vector3(inputActions.Player.Movement.ReadValue<Vector2>().x,0,
                                inputActions.Player.Movement.ReadValue<Vector2>().y);
        return inputAxis;
    }
    public Vector2 GetMousePosition()
    {
        return inputActions.Player.MouseInputPosition.ReadValue<Vector2>();
    }
    public Vector2 GetmouseDelta()
    {
        return inputActions.Player.MouseInputDelta.ReadValue<Vector2>();
    }

    public bool IsPressingZoomInKeyInKeyBoard()
    {
        //return ZoomPlusKeys.Any(key => Input.GetKey(key));
        return inputActions.Player.ZoomKeysKeyBoard.ReadValue<float>() > 0;
    }
    public bool IsPressingZoomOutKeyInKeyBoard()
    {
        //return ZoomMinusKeys.Any(key => Input.GetKey(key));
        return inputActions.Player.ZoomKeysKeyBoard.ReadValue<float>() < 0;
    }

    //public bool IsPressingZoomInKeyInMouse()
    //{
    //    //return ZoomPlusKeys.Any(key => Input.GetKey(key));
    //    return inputActions.Player.ZoomKeysMouse.ReadValue<float>() > 0;
    //}
    //public bool IsPressingZoomOutKeyMouse()
    //{
    //    //return ZoomMinusKeys.Any(key => Input.GetKey(key));
    //    return inputActions.Player.ZoomKeysMouse.ReadValue<float>() < 0;
    //}
    public bool IsPressingSprintKey()
    {
       return inputActions.Player.Sprint.ReadValue<float>() > 0.5f;
    }
    public bool IsPressingMouseRightClick()
    {
        return inputActions.Player.PointZoomKey.ReadValue<float>() == 1.0f;
    }
    
    private void OnDisable()
    {
        inputActions.Player.Disable();
    }
}
