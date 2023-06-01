using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera thirdCam;
    public CinemachineVirtualCamera firstCam;
    private bool isSwitched = false;

    private void Update()
    {
        if (Keyboard.current.vKey.wasPressedThisFrame)
        {
            isSwitched = !isSwitched;

            if (isSwitched)
            {
                thirdCam.Priority = 0;
                firstCam.Priority = 1;
            }
            else
            {
                thirdCam.Priority = 1;
                firstCam.Priority = 0;
            }
        }
    }
}
