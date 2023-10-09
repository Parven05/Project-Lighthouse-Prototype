using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private new Camera camera;
    [SerializeField] private Transform followTransform;
    [SerializeField] private float cameraFollowSpeed = 2f;
    private FirstPersonController fpsController;
    private Torch playerTorch;
 
    public enum TorchHoldingMouseMode
    {
        LockedMouse,
        OpenedMouse
    }

    public enum CameraLerpMode
    {
        WithLerp,
        WithoutLerp
    }

    [SerializeField] private TorchHoldingMouseMode mouseMode = TorchHoldingMouseMode.LockedMouse;
    [SerializeField] private CameraLerpMode cameraLerpMode = CameraLerpMode.WithoutLerp;
    private bool isEquippedTorch;

    private void Awake()
    {
        fpsController = GetComponent<FirstPersonController>();
        playerTorch = FindObjectOfType<Torch>();
        camera = Camera.main;
    }

    private void Update()
    {
        isEquippedTorch = playerTorch.IsActive();

        #region Getting Mouse World Position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        followTransform.position = ray.GetPoint(10);

        Debug.DrawLine(ray.origin, ray.GetPoint(10), Color.green);
        #endregion

        #region Controlling camera Pitch
        if (fpsController.cameraCanMove)
        {
            float cameraPitchX = fpsController.GetCameraPitchX();

            if (cameraLerpMode == CameraLerpMode.WithLerp)
            {
                camera.transform.localRotation = Quaternion.Lerp(camera.transform.localRotation, Quaternion.Euler(cameraPitchX, 0f, 0f), cameraFollowSpeed * Time.deltaTime);
            }
            else if (cameraLerpMode == CameraLerpMode.WithoutLerp)
            {
                camera.transform.localEulerAngles = new Vector3(cameraPitchX, 0f, 0);
            }
        }
        #endregion

        #region Controlling Camera State machine
        if (mouseMode == TorchHoldingMouseMode.LockedMouse)
        {
            if(Cursor.lockState == CursorLockMode.None)
            {
                FirstPersonController.SetCurserLockMode(true);
                Cursor.visible = true;
            }
        }
        else if(mouseMode == TorchHoldingMouseMode.OpenedMouse)
        {
            if (isEquippedTorch)
            {
                if(Cursor.lockState == CursorLockMode.Locked)
                {
                    FirstPersonController.SetCurserLockMode(false);
                    Cursor.visible = false;
                }
                
            }
            else
            {
                if (Cursor.lockState == CursorLockMode.None)
                {
                    Cursor.visible = true;
                    FirstPersonController.SetCurserLockMode(true);
                }
            }

        }
        #endregion

    }


}
