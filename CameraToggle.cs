using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraToggle : MonoBehaviour
{
    public Camera playerOriginCam;
    Camera cameraCamera;
    bool activateSwitch = false;

    void Start()
    {
        if (playerOriginCam == null)
            throw new System.Exception("playerOriginCam MUST be set");

        cameraCamera = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        activateSwitch |= Input.GetKeyDown(KeyCode.J);
        if (activateSwitch)
        {
            Debug.Log("Cam switch requested");
            if (playerOriginCam.enabled)
            {
                playerOriginCam.enabled = false;
                cameraCamera.enabled = true;
            }
            else
            {
                playerOriginCam.enabled = true;
                cameraCamera.enabled = false;
            }
            activateSwitch = false;
        }
    }
}
